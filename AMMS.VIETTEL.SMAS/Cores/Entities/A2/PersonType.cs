using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMS.VIETTEL.SMAS.Cores.Entities.A2
{
    public class PersonType : EntityBase
    {
        public string? Name { get; set; }
        public string? Note { get; set; }
    }
}
