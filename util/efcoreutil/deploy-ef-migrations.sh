#!/bin/bash         

# chmod a+x /where/i/saved/it/hello_world.sh


EfMigrationsNamespace=$1
BuildFlavor=$2
EfMigrationsDllName=$1.dll
EfMigrationsDllDepsJson=$1.deps.json
DllDir=$PWD
EfMigrationsDllDepsJsonPath=$PWD/bin/$BuildFlavor/netcoreapp1.0/$EfMigrationsDllDepsJson
PathToNuGetPackages=$HOME/.nuget/packages
  PathToEfDll=$PathToNuGetPackages/microsoft.entityframeworkcore.tools.dotnet/1.0.0/tools/netcoreapp1.0/ef.dll

dotnet exec --depsfile $EfMigrationsDllDepsJsonPath --additionalprobingpath $PathToNuGetPackages $PathToEfDll database update --assembly ./$EfMigrationsDllName --startup-assembly ./$EfMigrationsDllName --project-dir . --content-root $DllDir --data-dir $DllDir --verbose --root-namespace $EfMigrationsNamespace
