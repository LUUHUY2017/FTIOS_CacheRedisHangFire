using System.Xml.Linq;

namespace AMMS.Hanet.Data.Response
{
    public class Hanet_User_Data
    {
        public string? personID { get; set; }
        public string? name { get; set; }
        public string? placeID { get; set; }
        public string? title { get; set; }
        public int? type { get; set; }
        public string? avatar { get; set; }
        public string? sex { get; set; }
        public double? age { get; set; }
        public string? aliasID { get; set; }
        public string? phone { get; set; }
        public string? enable { get; set; }
        public string? departmentID { get; set; }
        public string? dob { get; set; }
    }
}
