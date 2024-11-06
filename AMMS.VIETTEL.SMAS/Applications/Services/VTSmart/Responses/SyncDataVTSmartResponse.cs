namespace AMMS.VIETTEL.SMAS.Applications.Services.VTSmart.Responses
{
    #region Req
    public class SyncDataRequest
    {
        public string? SecretKey { get; set; }
        public string? SchoolCode { get; set; }
        public string? SchoolYearCode { get; set; }
        public string? ClassCode { get; set; }
        public DateTime? AbsenceDate { get; set; }
        public int? Section { get; set; }
        public int? FormSendSMS { get; set; }
        public List<StudentAbsenceByDevice>? StudentAbsenceByDevices { get; set; }
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

    public class StudentAbsenceByDevice
    {
        public string? StudentCode { get; set; }
        public string? Value { get; set; }
        public ExtraProperties? ExtraProperties { get; set; }
    }
    #endregion


    #region Res
    public class SyncDataResponse
    {
        public List<ResponseParams> responses { get; set; }
        public bool isSuccess { get; set; }
        public string message { get; set; }
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
