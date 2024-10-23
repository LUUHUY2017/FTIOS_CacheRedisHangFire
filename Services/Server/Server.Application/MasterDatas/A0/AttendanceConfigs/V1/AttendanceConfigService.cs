using AutoMapper;
using Server.Application.MasterDatas.A0.AttendanceConfigs.V1.Models;
using Server.Core.Entities.A0;
using Server.Core.Interfaces.A0;
using Shared.Core.Commons;
using Shared.Core.Identity.Object;

namespace Server.Application.MasterDatas.A0.AttendanceConfigs.V1;

public class AttendanceConfigService
{
    private readonly IAttendanceConfigRepository _attendanceConfigRepository;
    private readonly IMapper _mapper;
    public AttendanceConfigService(IAttendanceConfigRepository attendanceConfigRepository, IMapper mapper)
    {
        _attendanceConfigRepository = attendanceConfigRepository;
        _mapper = mapper;
    }

    public async Task<Result<A0_AttendanceConfig>> SaveAsync(AttendanceConfigRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                var dataAdd = _mapper.Map<A0_AttendanceConfig>(request);
                var retVal = await _attendanceConfigRepository.AddAsync(dataAdd);
                return retVal;
            }
            else
            {
                var data = await _attendanceConfigRepository.GetByIdAsync(request.Id);
                var dataUpdate = data.Data;
                dataUpdate.EndpointIdentity = request.EndpointIdentity;
                dataUpdate.EndpointGateway = request.EndpointGateway;
                dataUpdate.AccountName = request.AccountName;
                dataUpdate.Password = request.Password;
                //dataUpdate.MorningStartTime = request.MorningStartTime;
                //dataUpdate.MorningEndTime = request.MorningEndTime;
                //dataUpdate.MorningLateTime = request.MorningLateTime;
                //dataUpdate.MorningBreakTime = request.MorningBreakTime;
                //dataUpdate.AfternoonStartTime = request.AfternoonStartTime;
                //dataUpdate.AfternoonEndTime = request.AfternoonEndTime;
                //dataUpdate.AfternoonLateTime = request.AfternoonLateTime;
                //dataUpdate.AfternoonBreakTime = request.AfternoonBreakTime;

                var retVal = await _attendanceConfigRepository.UpdateAsync(dataUpdate);
                return retVal;
            }
        }
        catch (Exception ex)
        {
            return new Result<A0_AttendanceConfig>(null, $"Lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<AttendanceConfigResponse>> DeleteAsync(DeleteRequest request)
    {
        try
        {
            var item = await _attendanceConfigRepository.GetByIdAsync(request.Id);
            var retVal = await _attendanceConfigRepository.DeleteAsync(request);
            var itemMap = _mapper.Map<AttendanceConfigResponse>(item);

            return new Result<AttendanceConfigResponse>(itemMap, retVal.Message, false);
        }
        catch (Exception ex)
        {
            return new Result<AttendanceConfigResponse>(null, $"Lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<List<AttendanceConfigResponse>>> Gets()
    {
        try
        {
            var retVal = await _attendanceConfigRepository.GetAllAsync();

            var listMap = _mapper.Map<List<AttendanceConfigResponse>>(retVal);
            return new Result<List<AttendanceConfigResponse>>(listMap, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<List<AttendanceConfigResponse>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<AttendanceConfigResponse>> GetFirstOrDefault()
    {
        try
        {
            var retVal = await _attendanceConfigRepository.GetByFirstAsync(x  => x.Actived == true);

            var itemMap = _mapper.Map<AttendanceConfigResponse>(retVal.Data);
            return new Result<AttendanceConfigResponse>(itemMap, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<AttendanceConfigResponse>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

}
