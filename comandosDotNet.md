correr aplicacion con recarga activa:  dotnet run watch
compilar proyecto: dotnet build

paquete de disenio vistas: dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design

generador scafolding ayuda: dotnet-aspnet-codegenerator controller -h

Scafolding controllador: dotnet-aspnet-codegenerator controller -name NombreCntrolador -m modelo -dc dbContex -l Layout -outDir directorio

crear dbContext: dotnet ef dbcontext scaffold "Server=(localdb)\\mssqllocaldb;Database=DB;Trusted_Connection=True;MultipleActiveResultSets=true" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --context-dir Data

actualizar migraciob db: dotnet ef database update
crear proyecto mvc con autenticacion y db sql server: dotnet new  mvc -au Individual --use-local-db -o nombreProyecot
correr migracion: dotnet ef migration
