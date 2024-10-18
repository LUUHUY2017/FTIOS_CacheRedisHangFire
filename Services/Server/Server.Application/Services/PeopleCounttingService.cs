using EventBus.Messages;
using EventBus.Messages.Common;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Server.Core.Entities;
using Server.Core.MongoDB.Entities;
using Server.Core.MongoDB.Repositories;
using Server.Infrastructure;
using Shared.Core;

namespace Server.Application.Services;

//public class PeopleCounttingService
//{
//    private readonly IDeviceCacheService _deviceCacheService;
//    private readonly IPOC_DataInOutEventRepository _repo;
//    private readonly BiDbContext _BiDbContext;


//    private readonly IBus _bus;
//    Uri _uri;
//    private readonly EventBusSettings _eventBusSettings;
//    private readonly IEventBusAdapter _eventBusAdapter;

//    public PeopleCounttingService(IDeviceCacheService deviceCacheService
//        , IPOC_DataInOutEventRepository repo
//        , BiDbContext biDbContext

//        , IBus bus, IConfiguration configuration, IOptions<EventBusSettings> eventBusSettings, IEventBusAdapter eventBusAdapter
//        )
//    {
//        _deviceCacheService = deviceCacheService;
//        _repo = repo;
//        _BiDbContext = biDbContext;


//        _bus = bus;
//        _eventBusSettings = eventBusSettings.Value;
//        _eventBusAdapter = eventBusAdapter;

//        _uri = new Uri($"rabbitmq://{_eventBusSettings.HostAddress}/{EventBusConstants.BrickstreamXMLData}");
//    }
//    public async Task ProcessData(DeviceData deviceData)
//    {
//        //Cập nhật thiết bị online, thời gian kết nối, địa chỉ ip, thông số thiết bị
//        // sử dụng redis https://github.com/microsoftarchive/redis/releases/tag/win-3.0.504

//        Terminal terminal;
//        //_dataCacheService.RemoveData("brickstream_device");
//        var cacheData = _deviceCacheService.Get_Terminal_BrickStream();
//        if (cacheData == null)
//        {
//            cacheData = await _BiDbContext.Terminals.ToListAsync();

//            _deviceCacheService.Set_Terminal_BrickStream(cacheData);
//            cacheData = _deviceCacheService.Get_Terminal_BrickStream();
//        }
//        if (cacheData != null)
//        {
//            string sn = deviceData.Propertie.SerialNumber;
//            terminal = cacheData.FirstOrDefault(x => x.SerialNumber == sn);

//            if (terminal != null)
//            {
//                terminal.LastTimeUpdateSocket = DateTime.Now;
//                if (deviceData.ReceivedTime != null)
//                    terminal.LastTimeReceivedData = deviceData.ReceivedTime;

//                if (terminal.OnlineStatus != "Online")
//                {
//                    terminal.OnlineStatus = "Online";
//                    //Gửi thông báo Đẩy dữ liệu vào Queue
//                    await _eventBusAdapter.GetSendEndpointAsync(EventBusConstants.DeviceOnlineOffline_Queue).Result.Send(terminal);
//                }

//                _deviceCacheService.Set_Terminal_BrickStream(cacheData);
//            }
//        }



//        //Cập nhật dữ liệu vào db

//        try
//        {
//            if (deviceData != null)
//            {
//                if (deviceData.Propertie != null)
//                {
//                    var _pro = deviceData.Propertie;

//                    #region Cập nhật dữ liệu người vào ra và CSDL
//                    if (deviceData.PeopleCountingDatas != null && deviceData.PeopleCountingDatas.Count > 0)
//                    {
//                        var terminals = _deviceCacheService.Get_Terminal_BrickStream();

//                        var listdata = new List<POC_DataInOutEvent>();
//                        foreach (var d in deviceData.PeopleCountingDatas)
//                        {
//                            var data_old = await _repo.Get(_pro.SerialNumber, d.UnixStartTime);
//                            if (data_old == null)
//                            {
//                                listdata.Add(new POC_DataInOutEvent()
//                                {
//                                    DataProcessed = false,
//                                    EndTime = d.EndTime.AddHours(7),
//                                    Id = Guid.NewGuid().ToString(),
//                                    Interval = 1,
//                                    NumToEnter = d.Enters,
//                                    NumToExit = d.Exits,
//                                    SerialNumber = _pro.SerialNumber,
//                                    StartTime = d.StartTime.AddHours(7),
//                                    UnixStartTime = d.UnixStartTime,
//                                });
//                            }

//                            if(terminals!=null)
//                            {
//                                var termianl = terminals?.FirstOrDefault(o => o.SerialNumber == _pro.SerialNumber);
//                                if (termianl != null)
//                                {
//                                    termianl.LastTimeUpdateDataMongoDB = d.StartTime;
//                                }
//                            }
                            

//                        }
//                        if (listdata.Count() > 0)
//                        {
//                            await _repo.CreateAsync(listdata);
//                            _deviceCacheService.Set_Terminal_BrickStream(terminals);
//                        }
//                    }
//                    #endregion
//                }

//            }
//        }
//        catch (Exception e)
//        {
//            //_logger.LogError($"Sent email event error: {e.StackTrace}");
//        }

//    }


//}
