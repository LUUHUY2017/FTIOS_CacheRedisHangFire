using AMMS.Hanet.Data.Response;
using MassTransit;
using Shared.Core.Loggers;

namespace AMMS.Hanet.Applications.V1.Consummer;

public class HANET_Checkin_DataConsummer : IConsumer<Hanet_Checkin_Data>
{
    public HANET_Checkin_DataConsummer()
    {
    }
    public async Task Consume(ConsumeContext<Hanet_Checkin_Data> context)
    {
        try
        {
            if (context == null || context.Message == null)
                return;

            Hanet_Checkin_Data data = context.Message;

            if (data == null)
                return;
            //await _zK_TA_DataService.AddTA_Data(data);
        }
        catch (Exception ex)
        {
            Logger.Warning(ex.Message);
        }
    }
}
