add-migration Init_DB_Hanet_241106091711444 -context DeviceAutoPushDbContext -Project AMMS.Hanet -o Datas/Migrations/MySql
Remove-Migration -context DeviceAutoPushDbContext -Project AMMS.Hanet
update-database -context DeviceAutoPushDbContext
