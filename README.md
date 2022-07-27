#.NET API Assessment


##Prerequisite for DB
1. Open Microsoft SQL Server Management Studio.
2. Enter following Credentials -> 
	Server type : Database Engine
	Server name : (localdb)\MSSQLLocalDB
	Authentication : Windows Authentication
3. Press 'Connect'
4. Right click on Databases, press 'Attach'.
5. Under Databases to attach, click on the 'Add' button.
6. Navigate to the directory ..repos\DOTNET API Assessment\Database and locate SchoolDB.mdf
7. Click on 'SchoolDB.mdf' and click the Ok button
8. Click on Ok button again to add the database.

##Build and run
1. Navigate to the directory where it contains DOTNET API Assessment.sln. Double click to open the solution through Visual Studio.
2. Once done, under the build tab, click on build/rebuild solution.
3. Now you can run the build through IIS Express
4. The swagger UI will be shown on the browser page.
5. The test can begin!

##Design
1. Use of .Net API CORE
2. Main Program/Startup - Configuration/Hosting of API
3. Controllers - Actions specific to a model. In this case TeacherController.cs is only required in this assignment
4. Data Model - Data models of database tables to link to the MS SQL Local DB.
5. Model - Model of Objects 
6. User View Model - Custom template of parameters and return types.