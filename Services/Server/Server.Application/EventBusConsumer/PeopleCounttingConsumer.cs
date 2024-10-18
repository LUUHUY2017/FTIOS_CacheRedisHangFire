using MassTransit;
using Server.Application.PeopleCountDataServices;
using Shared.Core;

namespace Server.Application.EventBusConsumer;

public class PeopleCounttingConsumer : IConsumer<DeviceData>
{
    //private readonly PeopleCounttingService _peopleCounttingService;
    private readonly PeopleCountDataService _peopleCountDataService;

    public PeopleCounttingConsumer(
        //PeopleCounttingService peopleCounttingService
          PeopleCountDataService peopleCountDataService
        )
    {
        //_peopleCounttingService = peopleCounttingService;
        _peopleCountDataService = peopleCountDataService;
    }
    public async Task Consume(ConsumeContext<DeviceData> context)
    {
        //await _peopleCounttingService.ProcessData(context.Message);

        //await _peopleCountDataService.ProcessCountingData(context.Message);
    }
}
