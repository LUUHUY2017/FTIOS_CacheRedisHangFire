namespace AMMS.VIETTEL.SMAS.Applications.Services.VTSmart.Responses
{
    public class ClassResponse
    {
        public string? Id { get; set; }
        public string? ClassName { get; set; }
        public string? SchoolYearId { get; set; }
        public string? SchoolLevelCode { get; set; }
        public string? SgradeLevelCode { get; set; }
        public string? GradeLevel { get; set; }
        public bool? HasMorningStudy { get; set; }
        public bool? HasAfternoonStudy { get; set; }
        public bool? HasEveningStudy { get; set; }
        public bool? HasRightToUpdate { get; set; }
        public string? SchoolPlaceId { get; set; }
        public int? SortOrder { get; set; }
        public string? SyncCode { get; set; }
        public string? LearnModeCode { get; set; }
        public bool? IsContinuingEducation { get; set; }
        public string? TeacherId { get; set; }
        public string? TeacherName { get; set; }
        //public ExtraProperties extraProperties { get; set; }
        public int? Semester { get; set; }
        public int? TotalStudent { get; set; }
        public int? TotalFemale { get; set; }
        public int? TotalMale { get; set; }
    }

    public class GradeClasseReponse
    {
        public string GradeLevelCode { get; set; }
        public List<ClassResponse> Classes { get; set; }
    }
}
