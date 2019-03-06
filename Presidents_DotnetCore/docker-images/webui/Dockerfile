FROM microsoft/aspnetcore:1.1
ARG source
ARG ConnectionStrings__default
WORKDIR /app
EXPOSE 80
COPY ./published-for-docker .
ADD ./deploy-ef-migrations.bat /app/deploy-ef-migrations.bat
ADD ./setup-and-run.bat /app/setup-and-run.bat
# ENTRYPOINT ["dotnet", "Benday.Presidents.WebUi.dll"]
ENTRYPOINT ["setup-and-run.bat"]
