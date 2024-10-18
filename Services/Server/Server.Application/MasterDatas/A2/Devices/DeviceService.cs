using AutoMapper;
using Server.Application.MasterDatas.A2.Devices.Models;
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
        private readonly IDeviceRepository _deviceRepository;
        protected readonly IMasterDataDbContext _dbContext;
        public DeviceService(IMapper mapper, IDeviceRepository deviceRepository, IMasterDataDbContext dbContext)
        {
            _mapper = mapper;
            _deviceRepository = deviceRepository;
            _dbContext = dbContext;
        }

        public async Task<List<A2_Device>> GetAll()
        {
            var data = await _deviceRepository.GetAll();
            return data;
        }

        public async Task<Result<A2_Device>> GetById(string id)
        {
            var data = await _deviceRepository.GetById(id);
            return data;
        }
        public async Task<List<DeviceResponse>> GetDevices(DeviceFilterRequest request)
        {
            var data = await _deviceRepository.Gets(request);
            return data;
        }
        public async Task<Result<A2_Device>> Update(DeviceRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.DeviceName) || string.IsNullOrEmpty(request.IpAddress)
                    || string.IsNullOrEmpty(request.HTTPPort.ToString())
                    )
                {
                    return new Result<A2_Device>(null, "Bạn phải nhập đầy đủ các trường yêu cầu!", false);
                }

                var model = _mapper.Map<A2_Device>(request);
                var relval = await _deviceRepository.UpdateAsync(model);


                if (relval.Succeeded)
                    return new Result<A2_Device>(relval.Data, $"Thao tác thành công!", true);
                else
                    return new Result<A2_Device>(null, $"Có lỗi khi cập nhật: " + relval.Messages, false);
            }
            catch (Exception ex)
            {
                return new Result<A2_Device>(null, $"Lỗi: {ex.Message}", false);
            }
        }


        //public async Task<Result<int>> Delete(string id)
        //{
        //    try
        //    {
        //        var modelDel = await _deviceRepository.GetByIdAsync(Guid.Parse(id));
        //        if (modelDel == null)
        //        {
        //            return new Result<int>(0, "Không tìm thấy dữ liệu!", false);
        //        }
        //        else
        //        {
        //            var response = await _deviceRepository.DeleteAsync(modelDel);
        //            if (response != null)
        //            {
        //                await _deviceCacheService.RemoveData(modelDel.SerialNumber);
        //                return new Result<int>(1, "Thành công", true);
        //            }

        //        }
        //        return new Result<int>(0, $"Có lỗi xảy ra!", false);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Result<int>(0, $"Lỗi: {ex.Message}", false);
        //    }
        //}


    }
}
