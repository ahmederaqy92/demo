To create the database schema for the presidents database:
1) open a command prompt window
2) cd to the 'src\Benday.Presidents.Api' directory
3) deploy the database using entity framework migrations using the following command

dotnet ef database update -s ..\Benday.Presidents.WebUi --context Benday.Presidents.Api.DataAccess.PresidentsDbContext

NOTE: the database connection strings are stored in appsettings.json and assume that you are 
deploying your database to a default sql server instance on your local machine.  