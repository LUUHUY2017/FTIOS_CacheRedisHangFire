using AMMS.Hanet.Datas.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Datas;

namespace AMMS.Hanet.Datas.Databases;


public class ViettelDbContext : BaseDbContext, IViettelDbContext
{
    // This constructor is used of runit testing
    public ViettelDbContext() : base()
    {
    }
    public ViettelDbContext(DbContextOptions<ViettelDbContext> options, IMediator mediator) : base(options, mediator)
    {
    }

    public DbSet<Student> Student { get; set; }
}
