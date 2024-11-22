using Server.Core.Entities.A2;

namespace Server.Core.Interfaces.A2.Devices.RequeResponsessts;
public class DeviceResponse : Device
{
    public DeviceResponse()
    {
        
    }
    public DeviceResponse(Device o)
    {
        Id = o.Id;
        Actived = o.Actived;
        CreatedBy = o.CreatedBy;
        CreatedDate = o.CreatedDate;
        DeviceIn = o.DeviceIn;
        DeviceName = o.DeviceName;
        SerialNumber = o.SerialNumber;
        DeviceID = o.DeviceID;
        Password = o.Password;

        ActiveKey = o.ActiveKey;
        DeviceCode = o.DeviceCode;
        DeviceDescription = o.DeviceDescription;
        DeviceInfo = o.DeviceInfo;
        DeviceInput = o.DeviceInput;
        IPAddress = o.IPAddress;
        MacAddress = o.MacAddress;
        HTTPPort = o.HTTPPort;
        PortConnect = o.PortConnect;
        ConnectionStatus = o.ConnectionStatus;
        BrandName = o.BrandName;
        DeviceModel = o.DeviceModel;
        DeviceType = o.DeviceType;
        OrganizationId = o.OrganizationId;
        UserCount = o.UserCount;
        FaceCount = o.FaceCount;
        LastModifiedDate = o.LastModifiedDate;
    }
}
