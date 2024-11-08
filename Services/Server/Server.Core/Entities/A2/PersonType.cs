using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Entities.A2
{
    public class PersonType : EntityBase
    {
        public string? Name { get; set; }
        public string? Note { get; set; }
    }
}
