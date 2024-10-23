﻿using AMMS.ZkAutoPush.Datas.Databases;
using AMMS.ZkAutoPush.Datas.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.Core.Loggers;

namespace AMMS.ZkAutoPush.Applications.V1;
public class ZK_RP_DataService
{

    private readonly DeviceAutoPushDbContext _deviceAutoPushDbContext;
    public ZK_RP_DataService(DeviceAutoPushDbContext deviceAutoPushDbContext)
    {
        _deviceAutoPushDbContext = deviceAutoPushDbContext;
    }

}
