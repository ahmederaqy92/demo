set EfMigrationsNamespace=%1
set EfMigrationsDllName=%1.dll
set EfMigrationsDllDepsJson=%1.deps.json
set EfMigrationsDllRuntimeConfigJson=%1.runtimeconfig.json
set DllDir=%cd%
set PathToNuGetPackages=%USERPROFILE%\.nuget\packages
set PathToEfDll=%PathToNuGetPackages%\microsoft.entityframeworkcore.tools.dotnet\1.0.1\tools\netcoreapp1.0\ef.dll

dotnet exec --depsfile .\%EfMigrationsDllDepsJson% --additionalprobingpath %PathToNuGetPackages% --runtimeconfig %EfMigrationsDllRuntimeConfigJson% %PathToEfDll% database update --assembly .\%EfMigrationsDllName% --startup-assembly .\%EfMigrationsDllName% --project-dir . --content-root %DllDir% --data-dir %DllDir% --verbose --root-namespace %EfMigrationsNamespace%