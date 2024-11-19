using Shared.Core.Commons;
using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMS.VIETTEL.SMAS.Cores.Entities.A2;

[Table("ScheduleJob")]
public class ScheduleJob : EntityBase
{
    [MaxLength(500)]
    public string? ScheduleJobName { get; set; }
    /// <summary>
    /// Thời gian chạy
    /// </summary>
    public TimeSpan? ScheduleTime { get; set; }
    /// <summary>
    /// Tuần suất gửi: 1 phút 1 lần, 1 ngày 1 lần, 
    /// </summary>
    /// 
    [MaxLength(250)]
    public string? ScheduleSequential { get; set; }

    /// <summary>
    /// Loại đồng bộ: Học sinh, Điểm danh
    /// </summary>
    [MaxLength(250)]
    public string? ScheduleType { get; set; }
    [MaxLength(500)]
    public string? ScheduleNote { get; set; }
}


public class ListScheduleCategory
{
    public static ObjectString Minutely = new ObjectString() { Id = "Minutely", Name = "Mỗi phút" };
    public static ObjectString _5Minutely = new ObjectString() { Id = "5M", Name = "Mỗi 5 phút" };
    public static ObjectString _10Minutely = new ObjectString() { Id = "10M", Name = "Mỗi 10 phút" };
    public static ObjectString _20Minutely = new ObjectString() { Id = "20M", Name = "Mỗi 20 phút" };
    public static ObjectString _30Minutely = new ObjectString() { Id = "30M", Name = "Mỗi 30 phút" };
    public static ObjectString _40Minutely = new ObjectString() { Id = "40M", Name = "Mỗi 40 phút" };
    public static ObjectString _50Minutely = new ObjectString() { Id = "50M", Name = "Mỗi 50 phút" };



    public static ObjectString Hourly = new ObjectString() { Id = "Hourly", Name = "Hàng giờ" };
    public static ObjectString Daily = new ObjectString() { Id = "Daily", Name = "Hàng ngày", Type = "1" };
    public static ObjectString Weekly = new ObjectString() { Id = "Weekly", Name = "Hàng tuần", Type = "1" };
    public static ObjectString Monthly = new ObjectString() { Id = "Monthly", Name = "Hàng tháng", Type = "1" };
    public static ObjectString Yearly = new ObjectString() { Id = "Yearly", Name = "Hàng năm", Type = "1" };


    public static ObjectString _5seconds = new ObjectString() { Id = "5s", Name = "Mỗi 5 giây" };
    public static ObjectString _10seconds = new ObjectString() { Id = "10s", Name = "Mỗi 10 giây" };
    public static ObjectString _20seconds = new ObjectString() { Id = "20s", Name = "Mỗi 20 giây" };
    public static ObjectString _30seconds = new ObjectString() { Id = "30s", Name = "Mỗi 30 giây" };
    public static ObjectString _40seconds = new ObjectString() { Id = "40s", Name = "Mỗi 40 giây" };
    public static ObjectString _50seconds = new ObjectString() { Id = "50s", Name = "Mỗi 50 giây" };

    public static List<ObjectString> SequentialMaxTypes
    {
        get
        {
            var _data = new List<ObjectString>();
            _data.Add(Minutely);
            _data.Add(Hourly);
            _data.Add(Daily);
            _data.Add(Weekly);
            _data.Add(Monthly);
            _data.Add(Yearly);

            return _data;
        }
    }


    public static List<ObjectString> SequentialMinTypes
    {
        get
        {
            var _data = new List<ObjectString>();

            _data.Add(_5Minutely);
            _data.Add(_10Minutely);
            _data.Add(_20Minutely);
            _data.Add(_30Minutely);
            _data.Add(_40Minutely);
            _data.Add(_50Minutely);

            _data.Add(_5seconds);
            _data.Add(_10seconds);
            _data.Add(_20seconds);
            _data.Add(_30seconds);
            _data.Add(_40seconds);
            _data.Add(_50seconds);

            return _data;
        }
    }
    public static List<ObjectString> SequentialTypes
    {
        get
        {
            var _data = new List<ObjectString>();
            _data.AddRange(SequentialMaxTypes);
            _data.AddRange(SequentialMinTypes);
            _data = _data.Distinct().ToList();
            return _data;
        }
    }
    /// <summary>
    /// Loại lập lịch
    /// </summary>
    public static List<ObjectString> ScheduleTypes = new List<ObjectString>()
    {
         new ObjectString() { Id = "DONGBOHOCSINH", Name = "Đồng bộ học sinh" },
         new ObjectString() { Id = "DONGBODIEMDANH", Name = "Đồng bộ điểm danh" },
    };
}