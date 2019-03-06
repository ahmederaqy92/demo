ECHO OFF
PAUSE
ECHO ***
ECHO deploying ef migrations
CALL deploy-ef-migrations Benday.Presidents.WebUi PresidentsDbContext
CALL deploy-ef-migrations Benday.Presidents.WebUi ApplicationDbContext
ECHO ***
ECHO starting webui app
CALL dotnet .\Benday.Presidents.WebUi.dll
ECHO webui app exited