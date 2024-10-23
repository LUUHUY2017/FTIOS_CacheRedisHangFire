using AMMS.ZkAutoPush.Data;
using MassTransit;

namespace AMMS.ZkAutoPush.Applications.V1;

public class ZK_RP_DATAConsummer : IConsumer<ZK_RP_DATA>
{
    public ZK_RP_DATAConsummer()
    {

    }
    public Task Consume(ConsumeContext<ZK_RP_DATA> context)
    {
        //Xử lý chèn vào CSDL nên sử dụng IMediator
        throw new NotImplementedException();
    }
}
