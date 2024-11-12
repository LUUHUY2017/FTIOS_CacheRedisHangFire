using AMMS.Hanet.Applications.AppConfigs.V1.Models;
using AMMS.Hanet.Data;
using AMMS.Hanet.Datas.Databases;
using AMMS.Hanet.Datas.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using Shared.Core.Commons;
using Shared.Core.Loggers;

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
        if (data != null)
        {
            HanetParam.Token = new AccessToken()
            {
                access_token = data.AccessToken,
                refresh_token = data.RefreshToken,
                email = data.Email,
                userID = data.UserId,
                expire = data.Expire ?? 0,
                token_type = data.TokenType,

            };
            HanetParam.PlaceId = data.PlaceId;
        }
        else
        {
            data = new app_config();
            data.GrantType = "refresh_token";
        }
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
                dataUpdate.PlaceId = request.PlaceId?.Trim();

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
    /// <summary>
    /// Token lấy dữ liệu
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<app_config> GetToken()
    {
        try
        {
            var config = await _dbContext.app_config.FirstOrDefaultAsync();

            if (config == null)
            {
                return null;
            }

            var options = new RestClientOptions("https://oauth.hanet.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/token", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", config.GrantType);
            request.AddParameter("client_id", config.ClientId);
            request.AddParameter("client_secret", config.ClientScret);
            request.AddParameter("refresh_token", config.RefreshToken);

            RestResponse response = await client.ExecuteAsync(request);

            var content = response.Content;
            Console.WriteLine(response.Content);

            AccessToken accessToken = JsonConvert.DeserializeObject<AccessToken>(content);
            if (accessToken == null || string.IsNullOrEmpty(accessToken.access_token))
                return null;

            HanetParam.Token = accessToken;
            HanetParam.PlaceId = config.PlaceId;
            HanetParam.TimeExpireTick = accessToken.expire + DateTime.Now.Ticks;

            config.AccessToken = accessToken.access_token;
            config.RefreshToken = accessToken.refresh_token;
            config.Expire = accessToken.expire;
            config.TokenType = accessToken.token_type;
            await _dbContext.SaveChangesAsync();
            return config;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return null;
        }
    }

}
