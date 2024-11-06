using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;

namespace AMMS.VIETTEL.SMAS.Infratructures.Databases.Seeds;

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



