using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using DOTNET_API_Assessment.DataModel;
using DOTNET_API_Assessment.UserViewModel;
using DOTNET_API_Assessment.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
// This class contains all of the api calls for Teachers
namespace DOTNET_API_Assessment.Controllers
{
    //[Route("api/")]
    [ApiController]
    [Produces("application/json")]
    public class TeacherController : ControllerBase
    {
        SchoolDBContext db = new SchoolDBContext();

        // POST api/register
        [HttpPost("/api/register")]
        public CustomResponseViewModel Register([FromBody] RegisterViewModel r)
        {
            CustomResponseViewModel cr = new CustomResponseViewModel();

            try
            {
                string teacherEmail = r.teacher;
                List <TeacherModel> t = new List<TeacherModel>();
                List <StudentModel> s = new List<StudentModel>();

                //Validate with database to see if the teacher exist
                var teacherIsExist = db.Set<Teacher>().FirstOrDefault(w => w.Email == teacherEmail);
                if (teacherIsExist == null)
                {
                    cr.statusCode = 200;
                    cr.message = "Teacher not found for " + teacherEmail;
                    return cr;
                }
                else
                {
                    t = db.Set<Teacher>().Where(w => w.Email == teacherEmail).Select(c => new TeacherModel
                    {
                        UserID = c.Id,
                        Email = c.Email
                    }).ToList();
                }

                //Validate with database to see if the student exist
                for (int count=0; count < r.students.Count; count++)
                {
                    var studentIsExist = db.Set<Student>().FirstOrDefault(w => w.Email == r.students[count]);
                    if (studentIsExist == null)
                    {
                        cr.statusCode = 200;
                        cr.message = "Student not found for " + r.students[count];
                        return cr;
                    }
                    else
                    {
                        //Validate with database to see if the student is already registered with teacher
                        s = db.Set<Student>().Where(w => w.Email == r.students[count]).Select(c => new StudentModel
                        {
                            UserID = c.Id,
                            Email = c.Email
                        }).ToList();

                        var registrationIsExist = db.Set<Registration>().FirstOrDefault(w => w.TeacherId == t[0].UserID && w.StudentId == s[0].UserID);
                        if (registrationIsExist == null)
                        {
                            //Create into Registration table
                            var registration = new Registration();
                            registration.TeacherId = t[0].UserID;
                            registration.StudentId = s[0].UserID;
                            db.Registrations.Add(registration);
                            db.SaveChanges();

                            cr.statusCode = 204;
                            cr.message = "Registration is successful.";
                        }
                        else
                        {
                            cr.statusCode = 200;
                            cr.message = "Student is already registered for " + r.students[count] + " with Teacher " + teacherEmail;
                            return cr;
                        }
                    }
                }

                return cr;
            }
            catch (Exception ex)
            {
                cr.statusCode = 400;
                cr.message = "Error when registering student. Error : " + ex;
                return cr;
            }
        }

        // GET api/commonstudents
        [HttpGet("/api/commonstudents")]
        public CustomResponseViewModel GetListOfCommonStudents([FromQuery] string queryString)
        {
            CustomResponseViewModel cr = new CustomResponseViewModel();
            List<string> teachersEmails = new List<string>();
            List<StudentModel> s = new List<StudentModel>();
            List<RegistrationModel> r = new List<RegistrationModel>();
            List<int> commonRegistrationList = new List<int>();

            try
            {
                //To split up the queries to include multiple same id query
                string[] queries = queryString.Split("&");

                foreach (string emailQuery in queries)
                {
                    //replace the encoded @ to the normal form
                    string properEmail = emailQuery.Replace("%40", "@");

                    //Add the teacher email into the list
                    teachersEmails.Add(properEmail.Substring(properEmail.LastIndexOf('=') + 1));
                }

                //get all teachers details
                var teachersList = db.Set<Teacher>().Where(t => teachersEmails.Contains(t.Email)).ToList();
                var teachersListIds = teachersList.Select(p => p.Id).ToList();

                //If there is only 1 teacher
                if (teachersList.Count == 1)
                {
                    r = db.Set<Registration>().Where(t => teachersListIds.Contains(t.TeacherId)).Select(c => new RegistrationModel
                    {
                        Id = c.Id,
                        TeacherId = c.TeacherId,
                        StudentId = c.StudentId
                    }).ToList();

                    //get all common student details
                    s = db.Set<Student>().Where(s => (r.Select(p => p.Id).ToList()).Contains(s.Id)).Select(c => new StudentModel
                    {
                        UserID = c.Id,
                        Email = c.Email
                    }).ToList();
                }
                else
                {
                    for (int count = 0; count < teachersList.Count; count++)
                    {
                        if (count == 0)
                        {
                            r = db.Set<Registration>().Where(t => teachersListIds.Contains(teachersList[count].Id)).Select(c => new RegistrationModel
                            {
                                Id = c.Id,
                                TeacherId = c.TeacherId,
                                StudentId = c.StudentId
                            }).ToList();
                        }
                        else
                        {
                            var tempR = db.Set<Registration>().Where(t => teachersListIds.Contains(teachersList[count].Id)).Select(c => new RegistrationModel
                            {
                                Id = c.Id,
                                TeacherId = c.TeacherId,
                                StudentId = c.StudentId
                            }).ToList();

                            //get student ids which are registered to all given teachers
                            commonRegistrationList = r.Select(x => x.TeacherId).Intersect(tempR.Select(x => x.TeacherId)).ToList();
                        }
                    }

                    //get all common student details
                    s = db.Set<Student>().Where(p => commonRegistrationList.Any(p2 => p2 == p.Id)).Select(c => new StudentModel
                    {
                        UserID = c.Id,
                        Email = c.Email
                    }).ToList();
                }

                cr.statusCode = 200;
                cr.message = "Retrieve successfully.";
                cr.students = new List<string>();
                foreach (StudentModel student in s)
                {
                    cr.students.Add(student.Email);
                }
                return cr;
            }
            catch (Exception ex)
            {
                cr.statusCode = 400;
                cr.message = "Error when retrieving common students' emails. Error : " + ex;
                return cr;
            }
        }

        // POST api/suspend
        [HttpPost("/api/suspend")]
        public CustomResponseViewModel Suspend([FromBody] SuspendViewModel s)
        {
            CustomResponseViewModel cr = new CustomResponseViewModel();

            try
            {
                string studentEmail = s.student;

                //check if student exist
                var studentIsExist = db.Set<Student>().FirstOrDefault(w => w.Email == studentEmail);
                if (studentIsExist == null)
                {
                    cr.statusCode = 200;
                    cr.message = "Student not found for " + studentEmail;
                    return cr;
                }
                else
                {
                    //Update the student status to suspended
                    var student = db.Set<Student>().FirstOrDefault(w => w.Email == studentEmail);
                    student.IsSuspended = "Y";
                    db.SaveChanges();

                    cr.statusCode = 204;
                    cr.message = "Student has been suspended successful.";
                    return cr;
                }
            }
            catch (Exception ex)
            {
                cr.statusCode = 400;
                cr.message = "Error when suspending student. Error : " + ex;
                return cr;
            }
        }

        // POST api/retrievefornotifications
        [HttpPost("/api/retrievefornotifications")]
        public CustomResponseViewModel GetListOfStudentWhoCanReceiveNotification([FromBody] NotificationViewModel n)
        {
            CustomResponseViewModel cr = new CustomResponseViewModel();
            List<TeacherModel> t = new List<TeacherModel>();
            List<StudentModel> s = new List<StudentModel>();
            List<RegistrationModel> r = new List<RegistrationModel>();
            cr.students = new List<string>();

            try
            {
                string teacherEmail = n.teacher;
                string notification = n.notification;

                //Validate with database to see if the teacher exist
                var teacherIsExist = db.Set<Teacher>().FirstOrDefault(w => w.Email == teacherEmail);
                if (teacherIsExist == null)
                {
                    cr.statusCode = 400;
                    cr.message = "Teacher not found for " + teacherEmail;
                    return cr;
                }
                else
                {
                    t = db.Set<Teacher>().Where(w => w.Email == teacherEmail).Select(c => new TeacherModel
                    {
                        UserID = c.Id,
                        Email = c.Email
                    }).ToList();

                    var teachersListIds = t.Select(p => p.UserID).ToList();
                    //get all student ids registed with this teacher
                    r = db.Set<Registration>().Where(a => teachersListIds.Contains(a.TeacherId)).Select(c => new RegistrationModel
                    {
                        Id = c.Id,
                        TeacherId = c.TeacherId,
                        StudentId = c.StudentId
                    }).ToList();

                    //get all student details
                    s = db.Set<Student>().Where(s => (r.Select(p => p.Id).ToList()).Contains(s.Id) && s.IsSuspended == "N").Select(c => new StudentModel
                    {
                        UserID = c.Id,
                        Email = c.Email
                    }).ToList();

                    foreach (StudentModel student in s)
                    {
                        cr.students.Add(student.Email);
                    }
                }

                if (notification.Contains("@"))
                {
                    //To split up the queries into message and @
                    //First index till the first @
                    string message = notification.Substring(0, notification.IndexOf("@"));

                    //From the index of the first @ till the end of the string
                    string addEmailsString = notification.Substring(notification.IndexOf("@") + 1);

                    //Split the string to get parts of the email
                    string[] addStudentEmails = addEmailsString.Split("@");
                    string fullEmail = "", firstPartOfEmail = "", secondPartOfEmail = "";

                    //to join the strings into a proper email
                    for (int count = 0; count < addStudentEmails.Length; count++)
                    {
                        if (count % 2 == 0)
                        {
                            firstPartOfEmail = addStudentEmails[count];
                        }
                        else
                        {
                            secondPartOfEmail = addStudentEmails[count];
                            fullEmail = firstPartOfEmail + "@" + secondPartOfEmail;
                            cr.students.Add(fullEmail);
                        }
                    }
                }

                cr.statusCode = 200;
                cr.message = "Retrieval of a list of students who can receive a given notification was successful.";
                return cr;
            }
            catch (Exception ex)
            {
                cr.statusCode = 400;
                cr.message = "Error when retrieving of a list of students who can receive a given notification. Error : " + ex;
                return cr;
            }
        }
    }
}
