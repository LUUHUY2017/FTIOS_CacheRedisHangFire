namespace Server.Application.Services.VTSmart.Responses
{
    #region Req
    public class SyncDataRequest
    {
        public string? Id { get; set; }
        public string? SecretKey { get; set; }
        public string? SchoolCode { get; set; }
        public string? SchoolYearCode { get; set; }
        /// <summary>
        /// bỏ v2
        /// </summary>
        public string? ClassCode { get; set; }

        /// <summary>
        /// add v2
        /// </summary>
        public string? ClassId { get; set; }
        public string? ProvinceCode { get; set; }


        public DateTime? AbsenceDate { get; set; }
        public int? Section { get; set; }
        public int? FormSendSMS { get; set; }
        public List<StudentAbsence>? StudentAbsences { get; set; }
    }
    public class ExtraProperties
    {
        public bool? IsLate { get; set; }
        public bool? IsOffSoon { get; set; }
        public bool? IsOffPeriod { get; set; }
        public DateTime? LateTime { get; set; }
        public DateTime? OffSoonTime { get; set; }
        public bool? PeriodI { get; set; }
        public bool? PeriodII { get; set; }
        public bool? PeriodIII { get; set; }
        public bool? PeriodIV { get; set; }
        public bool? PeriodV { get; set; }
        public bool? PeriodVI { get; set; }
        public DateTime? AbsenceTime { get; set; }

    }

    public class StudentAbsence
    {
        public string? StudentCode { get; set; }
        public string? Value { get; set; }
        public ExtraProperties? ExtraProperties { get; set; }
    }

    public class StudentSmasApiRequest
    {
        public string? secretKey { get; set; }
        public string? schoolCode { get; set; }
        public string? schoolYearCode { get; set; }
        public string? provinceCode { get; set; }
    }

    public class StudenSmasApiResponse
    {
        public string? StudentCode { get; set; }
        public string? StudentName { get; set; }
        public string? ClassName { get; set; }
        public string? ClassId { get; set; }
        public string? BirthDay { get; set; }
    }

    public class StudentDataApiResponse
    {
        public List<StudenSmasApiResponse> Responses { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
    #endregion


    #region Res
    public class SyncDataResponse
    {
        public List<ResponseParams> Responses { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
    public class ResponseParams
    {
        public string studentCode { get; set; }
        public int section { get; set; }
        public DateTime absenceDate { get; set; }
        public string value { get; set; }
        public ExtraProperties extraProperties { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
    }
    #endregion

}
