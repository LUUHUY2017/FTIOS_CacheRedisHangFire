﻿using AMMS.Hanet.Datas.Entities;
using Microsoft.EntityFrameworkCore;

namespace AMMS.Hanet.Datas.Databases;

public interface IDeviceAutoPushDbContext
{ 
    public DbSet<hanet_terminal> hanet_terminal { get; set; }
    public DbSet<hanet_app_config> app_config { get; set; }
    public DbSet<hanet_commandlog> hanet_commandlog { get; set; }
    public DbSet<hanet_transaction> hanet_transaction { get; set; }

    Task<int> SaveChangesAsync();
}
