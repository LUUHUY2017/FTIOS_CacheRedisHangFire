using AMMS.Hanet.Applications.AppConfigs.V1.Models;
using AMMS.Hanet.Datas.Databases;
using AMMS.Hanet.Datas.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Commons;

namespace AMMS.Hanet.Applications.AppConfigs.V1;

public class AppConfigService
{
    private readonly IDeviceAutoPushDbContext _dbContext;
    private readonly IMapper _mapper;
    public AppConfigService(IDeviceAutoPushDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<app_config>> GetFirstOrDefault()
    {
        var data = await _dbContext.app_config.FirstOrDefaultAsync();
        return new Result<app_config>(data, "Thành công!", true);
    }

    public async Task<Result<app_config>> AddOrEdit(AppConfigRequest request)
    {
        try
        {
            var dataUpdate = await _dbContext.app_config.FirstOrDefaultAsync();
            if (dataUpdate == null)
            {
                var dataAdd = _mapper.Map<app_config>(request);
                dataAdd.Id = Guid.NewGuid().ToString();
                var result = await _dbContext.app_config.AddAsync(dataAdd);
                var check = await _dbContext.SaveChangesAsync();
                if (check != 0)
                {
                    return new Result<app_config>(result.Entity, "Thành công", true);
                }
                else
                {
                    return new Result<app_config>(null, "Có lỗi xảy ra!", false);
                }
            }
            else
            {
                dataUpdate.UserId = request.UserId;
                dataUpdate.Email = request.Email;
                dataUpdate.AccessToken = request.AccessToken;
                dataUpdate.RefreshToken = request.RefreshToken; 
                dataUpdate.TokenType = request.TokenType;
                dataUpdate.Expire = request.Expire;
                dataUpdate.ClientScret = request.ClientScret;
                dataUpdate.GrantType = request.GrantType;
                dataUpdate.ClientId = request.ClientId;

                var result = _dbContext.app_config.Update(dataUpdate);
                var check = await _dbContext.SaveChangesAsync();
                if (check != 0)
                {
                    return new Result<app_config>(result.Entity, "Thành công", true);
                }
                else
                {
                    return new Result<app_config>(null, "Có lỗi xảy ra!", false);
                }
            }
        }
        catch (Exception ex)
        {
            return new Result<app_config>(null, $"Có lỗi: {ex.Message}", false);
        }
    }


}
