using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DOTNET_API_Assessment.Controllers;
using DOTNET_API_Assessment.UserViewModel;
using NUnit.Framework;

namespace DOTNET_API_Assessment.NUnitTest
{
    class TeacherControllerTest
    {
        [Test]
        [Description("Test case 1 - Retrieve 3 expected students common to a given list of teachers - 1 teacher only")]
        public void Retrieve_Common_Students_Email_One_Teacher()
        {
            string querystring = "?teacher=teacherken%40gmail.com";

            // Arrange
            CustomResponseViewModel expectedCR = new CustomResponseViewModel();
            expectedCR.statusCode = 200;
            expectedCR.message = "Retrieve successfully.";
            expectedCR.students = new List<string>();
            expectedCR.students.Add("studentjon@gmail.com");
            expectedCR.students.Add("studenthon@gmail.com");
            expectedCR.students.Add("student_only_under_teacher_ken@gmail.com");

            var teacherController = new TeacherController();
            // Act
            CustomResponseViewModel actualCR = new CustomResponseViewModel();
            actualCR = teacherController.GetListOfCommonStudents(querystring);
            // Assert
            Assert.AreEqual(expectedCR.statusCode, actualCR.statusCode);
            Assert.AreEqual(expectedCR.message, actualCR.message);
            Assert.AreEqual(expectedCR.students, actualCR.students);
        }

        [Test]
        [Description("Test case 2 - Retrieve 2 expected students common to a given list of teachers - 2 teacher only")]
        public void Retrieve_Common_Students_Email_Two_Teacher()
        {
            string querystring = "?teacher=teacherken%40gmail.com&teacher=teacherjoe%40gmail.com";

            // Arrange
            CustomResponseViewModel expectedCR = new CustomResponseViewModel();
            expectedCR.statusCode = 200;
            expectedCR.message = "Retrieve successfully.";
            expectedCR.students = new List<string>();
            expectedCR.students.Add("studentjon@gmail.com");
            expectedCR.students.Add("studenthon@gmail.com");

            var teacherController = new TeacherController();
            // Act
            CustomResponseViewModel actualCR = new CustomResponseViewModel();
            actualCR = teacherController.GetListOfCommonStudents(querystring);
            // Assert
            Assert.AreEqual(expectedCR.statusCode, actualCR.statusCode);
            Assert.AreEqual(expectedCR.message, actualCR.message);
            Assert.AreEqual(expectedCR.students, actualCR.students);
        }
    }
}
