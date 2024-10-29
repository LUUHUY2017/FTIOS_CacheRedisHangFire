add-migration Init_DB1037 -context DeviceAutoPushDbContext -Project AMMS.ZkAutoPush -o Datas/Migrations/MySql
Remove-Migration -context DeviceAutoPushDbContext -Project AMMS.ZkAutoPush
update-database -context DeviceAutoPushDbContext
