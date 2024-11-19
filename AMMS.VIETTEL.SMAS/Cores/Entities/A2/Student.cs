using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace AMMS.VIETTEL.SMAS.Cores.Entities.A2;

public class Student : EntityBase
{
    /// <summary>
    /// Mã SMAS
    /// </summary>
    /// 

    [MaxLength(50)]
    public string? StudentCode { get; set; }

    /// <summary>
    /// Mã SMAS bỏ kí tự
    /// </summary>
    [MaxLength(250)]
    public string? EthnicCode { get; set; }

    [MaxLength(250)]
    public string? FullName { get; set; }
    [MaxLength(250)]
    public string? Name { get; set; }

    public string? DateOfBirth { get; set; }
    [MaxLength(250)]
    public string? GenderCode { get; set; }
    public string? ImageSrc { get; set; }

    public bool? IsExemptedFull { get; set; }
    public string? StatusCode { get; set; }
    public string? Status { get; set; }
    [MaxLength(250)]
    public string? FullNameOther { get; set; }

    [MaxLength(250)]
    public string? PolicyTargetCode { get; set; }
    [MaxLength(250)]
    public string? PriorityEncourageCode { get; set; }
    [MaxLength(250)]
    public string? IdentifyNumber { get; set; }
    [MaxLength(50)]
    public string? StudentClassId { get; set; }
    public int? SortOrder { get; set; }

    public int? SortOrderByClass { get; set; }

    [MaxLength(50)]
    public string? PersonId { get; set; }

    [MaxLength(250)]
    public string? GradeCode { get; set; }
    public string? ClassId { get; set; }
    [MaxLength(50)]
    public string? ClassName { get; set; }
    /// <summary>
    /// mã SSO
    /// </summary>
    [MaxLength(250)]
    public string? SyncCode { get; set; }
    [MaxLength(250)]
    public string? SyncCodeClass { get; set; }
    [MaxLength(250)]
    public string? SchoolCode { get; set; }

    //public Person? Person { get;set; }
}
