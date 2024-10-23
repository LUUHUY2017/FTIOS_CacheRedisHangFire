namespace Server.Application.Services.VTSmart.Responses
{
    public class SchoolResponse
    {
        public string? Name { get; set; }
        public string? IdentifiCode { get; set; }
        public string? Code { get; set; }
        public string? AbbreviationName { get; set; }
        public string? PrincipalName { get; set; }
        public string? PrincipalPhone { get; set; }
        public DateTime? FoundedDay { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Fax { get; set; }
        public int? Level { get; set; }
        public string? Web { get; set; }
        public string? Email { get; set; }
        public string? TaxCode { get; set; }
        public string? Course { get; set; }
        public string? Agency { get; set; }
        public string? ManagementUnitId { get; set; }
        public string? ManagementUnitName { get; set; }
        public string? TypeCode { get; set; }
        public string? TypeName { get; set; }
        public string? EducationTypeCode { get; set; }
        public string? EducationTypeName { get; set; }
        public int? DetailedTrainingType { get; set; }
        public string? AreaCode { get; set; }
        public string? AreaName { get; set; }
        public string? ProvinceCode { get; set; }
        public string? ProvinceName { get; set; }
        public string? DistrictCode { get; set; }
        public string? DistrictName { get; set; }
        public string? WardCode { get; set; }
        public string? WardName { get; set; }
        public string? StandardLevelCode { get; set; }
        public string? StandardLevelName { get; set; }
        public string? PoorAreaCode { get; set; }
        public string? PoorAreaName { get; set; }
        public bool? IsActive { get; set; }
        public string? ImageSrc { get; set; }
        public string? LastLogin { get; set; }
        public bool? IsFirstLogin { get; set; }
        public int? TenantType { get; set; }
        //public ExtraProperties? extraProperties { get; set; }
        public List<SchoolLevel>? SchoolLevels { get; set; }
        //public List<object>? schoolPlaces { get; set; }
        public string? RegionalPolicyCode { get; set; }
        public string? RegionalPolicyName { get; set; }
        public string? ProjectTypeCode { get; set; }
        public string? ProjectTypeName { get; set; }
        public bool? IsContinuingEducation { get; set; }
        public bool? IsSchoolHasClassIsContinuingEducation { get; set; }
        public List<string>? CurrentUserRoles { get; set; }
        public string? UserName { get; set; }
        public int? SynTenantId { get; set; }
        public string? SchoolLevelCode { get; set; }
        public string? Description { get; set; }
        public string? ImageCoverPath { get; set; }
        public string? ImageAvatarPath { get; set; }
        public List<object>? LstImageIntroPath { get; set; }
        public string? ClientIds { get; set; }
        public string? Id { get; set; }
    }




    public class SchoolLevel
    {
        public string? Id { get; set; }
        public string? SchoolLevelCode { get; set; }
        public string? SchoolLevelName { get; set; }
        public int? NumberLession { get; set; }
        public int? Sort { get; set; }
    }


}
