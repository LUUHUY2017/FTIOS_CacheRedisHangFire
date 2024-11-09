using Server.Core.Entities.A2;

namespace Server.Application.MasterDatas.A2.Students.V1.Model
{
    public class DtoStudentResponse : Student
    {
        public string? ImageBase64 { get; set; }
        public bool? IsFace { get; set; }
        public string? IsFaceName { get; set; }
    }
}
