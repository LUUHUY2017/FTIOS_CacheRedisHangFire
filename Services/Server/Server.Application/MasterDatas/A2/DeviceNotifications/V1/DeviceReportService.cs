using Microsoft.EntityFrameworkCore;
using Server.Application.MasterDatas.A2.DeviceNotifications.V1.Models.Reports;
using Server.Core.Entities.A2;
using Server.Infrastructure.Datas.MasterData;

namespace Server.Application.MasterDatas.A2.DeviceNotifications.V1;

public class DeviceReportService
{
    private readonly IMasterDataDbContext _dbContext;
    public DeviceReportService(IMasterDataDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<DeviceStatusWarningModel>> GetsConnectDeviceReport(string orgId, string status)
    {
        try
        {
            var data = new List<Device>();
            if (!string.IsNullOrEmpty(orgId) && orgId != "0")
            {
                if (status == "Online")
                {
                    data = await _dbContext.Device.Where(x => (x.Actived == true)
                                                        && (x.ConnectionStatus == true)
                                                        && (x.OrganizationId == orgId)
                                                    )
                                                    .ToListAsync();
                }
                if (status == "Offline")
                {
                    data = await _dbContext.Device.Where(x => (x.Actived == true)
                                                        && (x.ConnectionStatus != true)
                                                        && (x.OrganizationId == orgId)
                                                    )
                                                    .ToListAsync();
                }
            }
            else
            {
                if (status == "Online")
                {
                    data = await _dbContext.Device.Where(x => (x.Actived == true)
                                                        && (x.ConnectionStatus == true)
                                                    )
                                                    .ToListAsync();
                }
                if (status == "Offline")
                {
                    data = await _dbContext.Device.Where(x => (x.Actived == true)
                                                        && (x.ConnectionStatus != true)
                                                    )
                                                    .ToListAsync();
                }
            }
            var result = (from d in data
                          join o in _dbContext.Organization
                          on d.OrganizationId equals o.Id
                          select new DeviceStatusWarningModel(d, o))
                          .OrderBy(x => x.OrganizationName)
                          .ThenBy(x => x.SerialNumber)
                          .ToList();
            return result;
        }
        catch (Exception ex)
        {
            return new List<DeviceStatusWarningModel>();
        }
    }
}
