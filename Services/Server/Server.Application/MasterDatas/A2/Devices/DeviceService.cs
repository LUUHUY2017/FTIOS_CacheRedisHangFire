using AMMS.DeviceData.RabbitMq;
using AutoMapper;
using EventBus.Messages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Server.Application.MasterDatas.A2.Devices.Models;
using Server.Application.MasterDatas.A2.Devices.Models.Commons;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Devices;
using Server.Core.Interfaces.A2.Devices.RequeResponsessts;
using Server.Core.Interfaces.A2.Devices.Requests;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Application.MasterDatas.A2.Devices
{
    public class DeviceService
    {
        private readonly IMapper _mapper;
        private readonly IEventBusAdapter _eventBusAdapter;
        private readonly IDeviceRepository _deviceRepository;
        protected readonly IMasterDataDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public DeviceService(
            IMapper mapper,
            IEventBusAdapter eventBusAdapter,
            IDeviceRepository deviceRepository,
            IMasterDataDbContext dbContext,
            IConfiguration configuration
            )
        {
            _mapper = mapper;
            _eventBusAdapter = eventBusAdapter;
            _deviceRepository = deviceRepository;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<List<Device>> GetAll()
        {
            var data = await _deviceRepository.GetAllAsync();
            return data;
        }

        public async Task<Result<List<Device>>> GetsForDeviceModel(string deviceModel)
        {
            try
            {
                var data = await _deviceRepository.GetAllAsync(x => x.DeviceModel == deviceModel);
                return new Result<List<Device>>(data, $"Thành công!", true);
            }
            catch (Exception ex)
            {
                return new Result<List<Device>>(null, $"Có lỗi: {ex.Message}", false);
            }

        }

        public async Task<Result<Device>> GetById(string id)
        {
            var data = await _deviceRepository.GetByIdAsync(id);
            return data;
        }

        public async Task<List<DeviceResponse>> GetDevices(DeviceFilterRequest request)
        {
            var data = await _deviceRepository.GetFilters(request);
            return data;
        }

        public async Task<List<DeviceResponse>> GetByOrgId(bool actived, string organizationId)
        {
            var data = await _deviceRepository.GetByOrgId(actived, organizationId);
            return data;
        }

        public async Task<Result<Device>> UpdateV2(DeviceRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.DeviceName) || string.IsNullOrEmpty(request.IpAddress)
                    || string.IsNullOrEmpty(request.HTTPPort.ToString())
                    )
                {
                    return new Result<Device>(null, "Bạn phải nhập đầy đủ các trường yêu cầu!", false);
                }

                var model = _mapper.Map<Device>(request);
                var relval = await _deviceRepository.UpdateAsyncV2(model);


                if (relval.Succeeded)
                    return new Result<Device>(relval.Data, $"Thao tác thành công!", true);
                else
                    return new Result<Device>(null, $"Có lỗi khi cập nhật: " + relval.Messages, false);
            }
            catch (Exception ex)
            {
                return new Result<Device>(null, $"Lỗi: {ex.Message}", false);
            }
        }

        public async Task<Result<Device>> Update(DeviceRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.DeviceName)
                       || string.IsNullOrEmpty(request.SerialNumber)
                       || string.IsNullOrEmpty(request.DeviceModel)
                       )
                {
                    return new Result<Device>(null, "Bạn phải nhập đầy đủ các trường yêu cầu!", false);
                }
                var checkSeri = await _deviceRepository.GetByFirstAsync(x => x.SerialNumber == request.SerialNumber);

                if (string.IsNullOrEmpty(request.Id))
                {
                    //thêm thiết bị 
                    if (checkSeri.Data != null)
                    {
                        return new Result<Device>(null, "SerialNumber đã có vui lòng tạo mới!", false);
                    }
                    var modelAdd = _mapper.Map<Device>(request);
                    var result = await _deviceRepository.AddAsync(modelAdd);
                    if (result.Succeeded)
                    {
                        TA_Device devive = new TA_Device
                        {
                            DeviceModel = modelAdd.DeviceModel,
                            Id = modelAdd.Id,
                            DeviceName = modelAdd.DeviceName,
                            SerialNumber = modelAdd.SerialNumber,

                        };
                        var param = JsonConvert.SerializeObject(devive);
                        RB_ServerRequest item = new RB_ServerRequest()
                        {
                            Id = result.Data.Id,
                            Action = ServerRequestAction.ActionAdd,
                            RequestType = ServerRequestType.Device,
                            DeviceId = result.Data.Id,
                            SerialNumber = result.Data.SerialNumber,
                            DeviceModel = result.Data.DeviceModel,
                            SchoolId = result.Data.OrganizationId,
                            RequestParam = param
                        };
                        var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}{EventBusConstants.Server_Auto_Push_S2D}");
                        await aa.Send(item);
                    }
                    return result;
                }
                else
                {
                    //cập nhật thiết bị
                    var modelUpdate = (await _deviceRepository.GetByFirstAsync(x => x.Id == request.Id)).Data;
                    if (checkSeri.Data != null && checkSeri.Data.Id != modelUpdate.Id)
                    {
                        return new Result<Device>(null, "SerialNumber đã có vui lòng tạo mới!", false);
                    }
                    modelUpdate.Actived = request.Actived;
                    modelUpdate.DeviceName = request.DeviceName;
                    modelUpdate.SerialNumber = request.SerialNumber;
                    modelUpdate.DeviceModel = request.DeviceModel;


                    return await _deviceRepository.UpdateAsync(modelUpdate);
                }
            }
            catch (Exception ex)
            {
                return new Result<Device>(null, $"Lỗi: {ex.Message}", false);
            }
        }


        public async Task<Result<int>> Delete(DeleteRequest request)
        {
            try
            {
                var modelDel = await _deviceRepository.GetByIdAsync(request.Id);
                if (modelDel == null)
                    return new Result<int>(0, "Không tìm thấy dữ liệu!", false);

                var response = await _deviceRepository.DeleteAsync(request);
                if (response.Succeeded)
                {
                    TA_Device devive = new TA_Device
                    {
                        DeviceModel = modelDel.Data.DeviceModel,
                        Id = modelDel.Data.Id,
                        DeviceName = modelDel.Data.DeviceName,
                        SerialNumber = modelDel.Data.SerialNumber,

                    };
                    var param = JsonConvert.SerializeObject(devive);
                    RB_ServerRequest item = new RB_ServerRequest()
                    {
                        Id = modelDel.Data.Id,
                        Action = ServerRequestAction.ActionDelete,
                        RequestType = ServerRequestType.Device,
                        DeviceId = modelDel.Data.Id,
                        SerialNumber = modelDel.Data.SerialNumber,
                        DeviceModel = modelDel.Data.DeviceModel,
                        SchoolId = modelDel.Data.OrganizationId,
                        RequestParam = param,
                    };
                    var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}{EventBusConstants.Server_Auto_Push_S2D}");
                    await aa.Send(item);
                }
                return response;
            }
            catch (Exception ex)
            {
                return new Result<int>(0, $"Lỗi: {ex.Message}", false);
            }
        }

        public async Task<Result<Device>> Active(ActiveRequest request)
        {
            try
            {
                var modelDel = await _deviceRepository.GetByIdAsync(request.Id);
                if (modelDel == null)
                    return new Result<Device>(null, "Không tìm thấy dữ liệu!", false);

                var response = await _deviceRepository.ActiveAsync(request);

                return response;
            }
            catch (Exception ex)
            {
                return new Result<Device>(null, $"Lỗi: {ex.Message}", false);
            }
        }
        public async Task<Result<Device>> InActive(InactiveRequest request)
        {
            try
            {
                var modelDel = await _deviceRepository.GetByIdAsync(request.Id);
                if (modelDel == null)
                    return new Result<Device>(null, "Không tìm thấy dữ liệu!", false);

                var response = await _deviceRepository.InactiveAsync(request);

                return response;
            }
            catch (Exception ex)
            {
                return new Result<Device>(null, $"Lỗi: {ex.Message}", false);
            }
        }

        public async Task<Result<List<object>>> GetDeviceBrands()
        {
            return new Result<List<object>>(DeviceBrandConst.BrandNames, "Thành công", true);
        }

    }
}
