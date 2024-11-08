using EventBus.Messages;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.API.APIs.MileSights.V1.Model;
using Server.Core.Entities.A3;
using Server.Core.Entities.GIO;
using Server.Core.Interfaces.GIO.VehicleInOuts;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;
using System.Drawing;

namespace Server.API.APIs.MileSights.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]

public class MileSightController : ControllerBase
{
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly IConfiguration _configuration;
    private readonly IMasterDataDbContext _db;
    private readonly IGIOVehicleInOutRepository _vehicelInOut;

    public MileSightController(IConfiguration configuration
    , IEventBusAdapter eventBusAdapter,
        IMasterDataDbContext dbContext,
        IGIOVehicleInOutRepository vehicelInOut
    )
    {
        _configuration = configuration;
        _eventBusAdapter = eventBusAdapter;
        _db = dbContext;
        _vehicelInOut = vehicelInOut;
    }

    /// <summary>
    /// Nhận với phương thức get chỉ nhận biển số không nhận ảnh
    /// </summary>
    /// <param name="camera"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("LprNotify")]
    public IActionResult LprNotify(string camera, string source, string description)
    {
        return Ok();
    }


    /// <summary>
    /// Nhận với phương thức Post có nhận ảnh
    /// </summary>
    /// <param name="camera"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("LprNotify")]
    public async Task<IActionResult> LprNotify(string sn)
    {
        string mes = $"Camera {sn} ";
        try
        {
            sn = sn.ToUpper();

            var device = _db.Device.FirstOrDefault(o => o.SerialNumber.ToUpper() == sn || o.MacAddress.ToUpper() == sn);

            if (device != null)
            {
                var lane = _db.Lane.FirstOrDefault(o => o.Id == device.LaneId);
                if (lane != null)
                {
                    //mes += $", Vị trí: {lane.LaneName}";
                }


                DateTime now = DateTime.Now;
                MilesightLrpCameraMessage message;
                using (StreamReader stream = new StreamReader(Request.Body))
                {
                    string body = await stream.ReadToEndAsync();
                    message = JsonConvert.DeserializeObject<MilesightLrpCameraMessage>(body);
                    string plate = message.plate.Replace(" ", "").Replace("-", "").Replace(".", "").ToUpper();
                    mes += $", Biển số nhận dạng: {message.plate}";

                    var vehicleInOut = new VehicleInOut()
                    {
                        Actived = true,
                        CreatedDate = now,

                        LaneInId = lane?.Id,
                        LaneOutId = lane?.Id,

                        DateTimeIn = now,
                        DateTimeOut = now,
                        PlateNumber = plate,
                        VehicleCode = plate,
                        VehicleName = message.plate,
                        PlateNumberIdentification = plate,
                        PlateNumberIdentificationOut = plate,
                        VehicleNoInDay = 1,
                    };

                    if (device.DeviceType == "CAMERAIN")
                    {
                        mes += $", {lane?.LaneName}";
                        vehicleInOut.VehicleInOutStatus = 1;
                        vehicleInOut.Note = mes;
                    }
                    else
                    {
                        mes += $", {lane?.LaneName}";
                        vehicleInOut.VehicleInOutStatus = 2;
                        vehicleInOut.Note = mes;
                    }

                    var retval = await _vehicelInOut.CreateAsync(vehicleInOut);
                    if (retval.Succeeded)
                    {
                        var imageFolder = Common.GetImageDatePathFolder(now);
                        var imageFullFolder = Common.GetImageDateFullFolder(now);
                        string referenceId = retval.Data.Id;

                        // Lưu hình toàn cảnh
                        if (!string.IsNullOrWhiteSpace(message.full_image))
                        {
                            var imageArea = new Images();
                            imageArea.ImageIndex = 1;
                            imageArea.ReferenceId = referenceId;
                            imageArea.TimeWeight = 1;
                            imageArea.IsPlateNumberImage = false;

                            string imageName = Guid.NewGuid().ToString() + ".jpg";
                            imageArea.ImageFolder = imageFolder;
                            imageArea.ImageName = imageName;

                            try
                            {
                                Image img = Common.Base64ToImage(message.full_image);
                                string fileName = imageFullFolder + imageName;
                                if (System.IO.File.Exists(fileName))
                                    System.IO.File.Delete(fileName);
                                img.Save(fileName);
                            }
                            catch (Exception e)
                            { }

                            await _db.Image.AddAsync(imageArea);
                        }

                        // Lưu hình biển số
                        if (!string.IsNullOrWhiteSpace(message.plate_image))
                        {
                            var imagePlate = new Images();
                            imagePlate.ReferenceId = referenceId;
                            imagePlate.ImageIndex = 1;
                            imagePlate.TimeWeight = 1;
                            imagePlate.IsPlateNumberImage = true;

                            string imageName = Guid.NewGuid().ToString() + ".jpg";
                            imagePlate.ImageFolder = imageFolder;
                            imagePlate.ImageName = imageName;

                            try
                            {
                                Image img = Common.Base64ToImage(message.plate_image);
                                string fileName = imageFullFolder + imageName;
                                if (System.IO.File.Exists(fileName))
                                    System.IO.File.Delete(fileName);
                                img.Save(fileName);
                            }
                            catch (Exception e)
                            { }

                            await _db.Image.AddAsync(imagePlate);
                        }
                        await _db.SaveChangesAsync();
                    }
                }
            }
            else
                mes += $", Chưa được khai báo";
        }
        catch (Exception e)
        {
            mes += $", Message: {e.Message}, InnerException: {e.InnerException}, StackTrace: {e.StackTrace}";
        }
        return Ok(new { message = mes });
    }

    //void ProcessSkipToDc(Guid vehicleId)
    //{
    //    var lists = _db.A3VehicleInOut.Where(o => (o.VehicleInOutStatus == 2 || o.VehicleInOutStatus == 3) && o.VehicleId == vehicleId).OrderBy(o => o.DateTimeIn);
    //    if (lists != null && lists.Count() > 0)
    //    {
    //        foreach (var o1 in lists)
    //            o1.SkipToDc = true;

    //        _db.SaveChanges();
    //    }
    //}
}
