using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;

namespace AMMS.ZkAutoPush.Datas.Databases.Seeds;

public static class ContextSeed
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        CreateUserType(modelBuilder);
    }

    private static void CreateUserType(ModelBuilder modelBuilder)
    {
         
    }
}



