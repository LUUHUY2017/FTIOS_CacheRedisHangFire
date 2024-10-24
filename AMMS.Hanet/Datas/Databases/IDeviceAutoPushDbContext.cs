using AMMS.Hanet.Datas.Entities;
using Microsoft.EntityFrameworkCore;

namespace AMMS.Hanet.Datas.Databases;

public interface IDeviceAutoPushDbContext
{ 
    public DbSet<hanet_terminal> hanet_terminal { get; set; }


    Task<int> SaveChangesAsync();
}
