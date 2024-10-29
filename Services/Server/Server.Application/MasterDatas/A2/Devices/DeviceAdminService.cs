using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Application.MasterDatas.A2.Devices.Models;
using Server.Application.MasterDatas.A2.Devices.Models.Commons;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Devices;
using Server.Core.Interfaces.A2.Devices.RequeResponsessts;
using Server.Core.Interfaces.A2.Devices.Requests;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;
using static IdentityServer4.Models.IdentityResources;

namespace Server.Application.MasterDatas.A2.Devices;

public class DeviceAdminService
{
    private readonly IMapper _mapper;
    private readonly IDeviceRepository _deviceRepository;
    protected readonly IMasterDataDbContext _dbContext;
    public DeviceAdminService(IMapper mapper, IDeviceRepository deviceRepository, IMasterDataDbContext dbContext)
    {
        _mapper = mapper;
        _deviceRepository = deviceRepository;
        _dbContext = dbContext;
    }

    public async Task<List<A2_Device>> GetAll()
    {
        var data = await _deviceRepository.GetAllAsync();
        return data;
    }

    public async Task<Result<A2_Device>> GetById(string id)
    {
        var data = await _deviceRepository.GetByIdAsync(id);
        if (data.Data?.DeviceParam == DeviceConst.Admin)
            return data;
        return new Result<A2_Device>(null, "Không tìm thấy", false);
    }

    public async Task<Result<List<DeviceResponse>>> GetDevices(DeviceFilterRequest req)
    {
        try
        {
            bool activedVal = req.Actived == "1";

            var _data = await (from o in _dbContext.A2_Device
                               where o.Actived == activedVal
                                    && o.DeviceParam == DeviceConst.Admin
                                    && (!string.IsNullOrEmpty(req.OrganizationId) ? req.OrganizationId == o.OrganizationId : true)
                                    && (!string.IsNullOrEmpty(req.DeviceModel) ? req.DeviceModel == o.DeviceModel : true)
                                    && (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "DeviceName" ? o.DeviceName.Contains(req.Key) : true)
                                    //&& (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "DeviceCode" ? o.DeviceCode.Contains(req.Key) : true)
                                    //&& (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "DeviceDescription" ? o.DeviceDescription.Contains(req.Key) : true)
                                    && (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "SerialNumber" ? o.SerialNumber.Contains(req.Key) : true)
                                    && (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "IPAddress" ? o.IPAddress.Contains(req.Key) : true)
                               select new DeviceResponse(o))
                               .ToListAsync();
            if (_data.Any())
                return new Result<List<DeviceResponse>>(_data, "Thành công!", true);
            return new Result<List<DeviceResponse>>(null, "Không tìm thấy bản ghi!", false); ;
        }
        catch (Exception e)
        {
            return new Result<List<DeviceResponse>>(null, $"Lỗi: {e.Message}", true);
        }
    }

    public async Task<Result<DeviceSelectModel>> GetDeviceSelect()
    {
        try
        {
            var DeviceSelected = await _dbContext.A2_Device.Where(x => x.Actived == true && x.DeviceParam == DeviceConst.Admin).ToListAsync();
            var SelectedId = DeviceSelected.Select(x => x.Id).ToList();
            var DeviceUnSelected = await _dbContext.A2_Device.Where(x => x.Actived == true && !SelectedId.Contains(x.Id)).ToListAsync();

            var data = new DeviceSelectModel()
            {
                DeviceSelected = DeviceSelected,
                DeviceUnSelected = DeviceUnSelected,
            };
            return new Result<DeviceSelectModel>(data, "Thành công!", true);
        }
        catch (Exception e)
        {
            return new Result<DeviceSelectModel>(null, $"Lỗi: {e.Message}", true);
        }
    }

    public async Task<Result<A2_Device>> Update(DeviceRequest request)
    {
        try
        {
            var modelUpdate = (await _deviceRepository.GetByFirstAsync(x => x.Id == request.Id)).Data;
            if (modelUpdate?.DeviceParam != DeviceConst.Admin)
            {
                //thêm thiết bị 
                modelUpdate.DeviceParam = DeviceConst.Admin;
            }
            else
            {
                if (string.IsNullOrEmpty(request.DeviceName)
                    || string.IsNullOrEmpty(request.OrganizationId)
                   )
                {
                    return new Result<A2_Device>(null, "Bạn phải nhập đầy đủ các trường yêu cầu!", false);
                }
                //cập nhật thiết bị
                modelUpdate.Actived = request.Actived;
                modelUpdate.DeviceName = request.DeviceName;
                modelUpdate.OrganizationId = request.OrganizationId;
                modelUpdate.IPAddress = request.IpAddress;
                modelUpdate.PortConnect = request.PortConnect;
                modelUpdate.DeviceID = request.DeviceID;
                modelUpdate.HTTPPort = request.HTTPPort;
                modelUpdate.Password = request.Password;
                modelUpdate.DeviceDescription = request.DeviceDescription;
            }

            var relval = await _deviceRepository.UpdateAsync(modelUpdate);
            return relval;
        }
        catch (Exception ex)
        {
            return new Result<A2_Device>(null, $"Lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<A2_Device>> UnSelected(DeviceRequest request)
    {
        try
        {
            var modelDel = (await _deviceRepository.GetByIdAsync(request.Id)).Data;
            modelDel.DeviceParam = null;
            modelDel.Actived = true;
            var relval = await _deviceRepository.UpdateAsync(modelDel);
           
            return relval;
        }
        catch (Exception ex)
        {
            return new Result<A2_Device>(null, $"Lỗi: {ex.Message}", false);
        }
    }
    public async Task<Result<A2_Device>> Active(ActiveRequest request)
    {
        try
        {
            var modelDel = await _deviceRepository.GetByIdAsync(request.Id);
            if (modelDel == null)
                return new Result<A2_Device>(null, "Không tìm thấy dữ liệu!", false);

            var response = await _deviceRepository.ActiveAsync(request);

            return response;
        }
        catch (Exception ex)
        {
            return new Result<A2_Device>(null, $"Lỗi: {ex.Message}", false);
        }
    }
    public async Task<Result<A2_Device>> InActive(InactiveRequest request)
    {
        try
        {
            var modelDel = await _deviceRepository.GetByIdAsync(request.Id);
            if (modelDel == null)
                return new Result<A2_Device>(null, "Không tìm thấy dữ liệu!", false);

            var response = await _deviceRepository.InactiveAsync(request);

            return response;
        }
        catch (Exception ex)
        {
            return new Result<A2_Device>(null, $"Lỗi: {ex.Message}", false);
        }
    }
}
