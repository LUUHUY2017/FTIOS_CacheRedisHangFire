using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Server.Core.Entities.A0
{
    public class A0_Person : EntityBase
    {
        [MaxLength(50)]
        public string? BusinessUnitId { get; set; }

        [MaxLength(50)]
        public string? OrganizationId { get; set; }

        [MaxLength(50)]
        public string? PersonCode { get; set; }

        [MaxLength(50)]
        public string? FirstName { get; set; }
        [MaxLength(100)]
        public string? LastName { get; set; }

        [MaxLength(250)]
        public string? PersonImage { get; set; }

        /// <summary>
        /// Giấy phép lái xe
        /// </summary>
        [MaxLength(50)]
        public string? DriverLicenseNo { get; set; }
        public string? Note { get; set; }
        [MaxLength(500)]
        public string? PersonNote { get; set; }

        public Int64 PersonNo { get; set; }

        [MaxLength(50)]
        public string? PersonAccountNumber { get; set; }

        [MaxLength(20)]
        public string? PersonGender { get; set; }

        [MaxLength(50)]
        public string? CitizenId { get; set; }
        [MaxLength(50)]
        public string? PhoneNUmber { get; set; }
        [MaxLength(250)]
        public string? Email { get; set; }

        [MaxLength(50)]
        public string? PersonTypeId { get; set; }
        public A0_PersonType? PersonType { get; set; }

        [MaxLength(50)]
        public string? Language { get; set; }

        public DateTime? BirthDate { get; set; }

        [MaxLength(500)]
        public string? CurentAddress { get; set; } = "";
        [MaxLength(500)]
        public string? Address { get; set; } = "";
    }
}