namespace Server.Application.Services.VTSmart.Responses
{
    #region Req
    public class SyncDataRequest
    {
        public string? id { get; set; }
        public string? secretKey { get; set; }
        public string? schoolCode { get; set; }

        public DateTime? absenceDate { get; set; }
        public int? section { get; set; }
        public int? formSendSMS { get; set; }
        /// <summary>
        /// Kiểu nộp học sinh:
        //1. Nộp theo mã SSO
        //2. Nộp theo mã SMAS  - hiện tại
        /// </summary>
        public int? studentCodeType { get; set; }
        public List<StudentAbsence>? studentAbsenceByDevices { get; set; }
    }
    public class ExtraProperties
    {
        public bool? isLate { get; set; } = false;
        public bool? isOffSoon { get; set; } = false;
        public bool? isOffPeriod { get; set; } = false;
        public DateTime? lateTime { get; set; }
        public DateTime? offSoonTime { get; set; }
        public bool? periodI { get; set; } = false;
        public bool? periodII { get; set; } = false;
        public bool? periodIII { get; set; } = false;
        public bool? periodIV { get; set; } = false;
        public bool? periodV { get; set; } = false;
        public bool? periodVI { get; set; } = false;
        public DateTime? absenceTime { get; set; }

    }

    public class StudentAbsence
    {
        public string? studentCode { get; set; }
        public string? value { get; set; }
        public ExtraProperties? extraProperties { get; set; }
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
        /// <summary>
        /// Mã EduFlatform
        /// </summary>
        public string? SyncCodeSSO { get; set; }
        /// <summary>
        /// Mã CSDL ngành
        /// </summary>
        public string? SyncCode { get; set; }
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
