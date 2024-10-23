using AMMS.ZkAutoPush.Data;
using MassTransit;

namespace AMMS.ZkAutoPush.Applications.V1;

public class ZK_TA_DataConsummer : IConsumer<ZK_TA_DATA>
{
    public ZK_TA_DataConsummer()
    {

    }
    public Task Consume(ConsumeContext<ZK_TA_DATA> context)
    {
        //Xử lý chèn vào CSDL nên sử dụng IMediator
        throw new NotImplementedException();
    }
}
