FROM microsoft/mssql-server-windows
ADD create-presidents-user.sql /initial-scripts/create-presidents-user.sql
ADD run-scripts.bat /initial-scripts/run-scripts.bat
RUN /initial-scripts/run-scripts.bat
