//////////
Identity
/////////
add-migration Init_Indentity -context IdentityContext -Project Server.Infrastructure -o Datas/Identity/MsSqlMigrations/Identity
add-migration Init_Indentity -context IdentityContext -Project Server.Infrastructure -o Datas/Identity/MySqlMigrations/Identity
Remove-Migration -context IdentityContext -Project Server.Infrastructure
update-database -context IdentityContext



dotnet ef migrations add  Init_Indentity -c IdentityContext -p Server.Infrastructure -o Datas/Identity/MsSqlMigrations/Identity
dotnet ef database update -c IdentityContext



//////////
IdentityServer4
/////////
add-migration InitialIdentityServerConfigurationDbMigration -context ConfigurationDbContext -Project Server.Infrastructure -o Datas/Identity/MsSqlMigrations/ConfigurationDb
add-migration InitialIdentityServerConfigurationDbMigration -context ConfigurationDbContext -Project Server.Infrastructure -o Datas/Identity/MySqlMigrations/ConfigurationDb
update-database -context ConfigurationDbContext

add-migration InitialIdentityServerPersistedGrantDbMigration -context PersistedGrantDbContext -Project Server.Infrastructure -o Datas/Identity/MsSqlMigrations/PersistedGrantDb
add-migration InitialIdentityServerPersistedGrantDbMigration -context PersistedGrantDbContext -Project Server.Infrastructure -o Datas/Identity/MySqlMigrations/PersistedGrantDb
update-database -context PersistedGrantDbContext




// master




add-migration Init_Indentity -context IdentityContext -Project Server.Infrastructure -o Datas/Identity/MySqlMigrations/Identity
update-database -context IdentityContext

add-migration InitialIdentityServerConfigurationDbMigration -context ConfigurationDbContext -Project Server.Infrastructure -o Datas/Identity/MySqlMigrations/ConfigurationDb
update-database -context ConfigurationDbContext

add-migration InitialIdentityServerPersistedGrantDbMigration -context PersistedGrantDbContext -Project Server.Infrastructure -o Datas/Identity/MySqlMigrations/PersistedGrantDb
update-database -context PersistedGrantDbContext


add-migration MasterData_Init -context MasterDataDbContext -Project Server.Infrastructure -o Datas/MasterData/MySqlMigrations
 
update-database -context MasterDataDbContext

// Tạo Db HangFire
CREATE DATABASE amms_vehiclecount_hangfire;

