using AMMS.Hanet.Datas.Entities;
using Microsoft.EntityFrameworkCore;

namespace AMMS.Hanet.Datas.Databases;

public interface IViettelDbContext
{
    public DbSet<Student> Student { get; set; }

    Task<int> SaveChangesAsync();
}
