using MongoDB.Driver;
using Server.Core.MongoDB;
using Server.Core.MongoDB.Entities;
using Server.Core.MongoDB.Repositories;

namespace Server.Infrastructure.MongoDB.Repositories;

public class MongoDB_POC_DataInOutEventRepository : IPOC_DataInOutEventRepository
{
    private readonly IBIMongoDbDataContext _context;
    public MongoDB_POC_DataInOutEventRepository(IBIMongoDbDataContext context)
    {
        _context = context;
    }


    public async Task<List<POC_DataInOutEvent>> GetAlls(string serialNumber)
    {
        FilterDefinition<POC_DataInOutEvent> filter = Builders<POC_DataInOutEvent>.Filter.Eq(p => p.SerialNumber, serialNumber);
        return await _context
            .POC_DataInOutEvents
            .Find(filter)
            .ToListAsync();
    }

    public async Task<POC_DataInOutEvent> CreateAsync(POC_DataInOutEvent data)
    {
        await _context.POC_DataInOutEvents.InsertOneAsync(data);
        return data;
    }

    public async Task<List<POC_DataInOutEvent>> CreateAsync(List<POC_DataInOutEvent> datas)
    {
        await _context.POC_DataInOutEvents.InsertManyAsync(datas);
        return datas;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        FilterDefinition<POC_DataInOutEvent> filter = Builders<POC_DataInOutEvent>.Filter.Eq(p => p.Id, id);
        DeleteResult deleteResult = await _context
            .POC_DataInOutEvents
            .DeleteOneAsync(filter);
        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }

    public async Task<bool> UpdateAsync(POC_DataInOutEvent data)
    {
        var updateResult = await _context
         .POC_DataInOutEvents
         .ReplaceOneAsync(p => p.Id == data.Id, data);
        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> UpdateAsync(List<POC_DataInOutEvent> datas)
    {
        if (datas != null && datas.Count > 0)
        {
            foreach (var data in datas)
                await UpdateAsync(data);
        }
        return true;
    }
    public async Task<POC_DataInOutEvent> Get(string serialNumber, long unixStartTime)
    {
        var builder = Builders<POC_DataInOutEvent>.Filter;
        var filter = builder.Empty;

        //var serialNumberFilter = builder.Regex(x => x.SerialNumber, new BsonRegularExpression(serialNumber));//
        var serialNumberFilter = builder.Eq(x => x.SerialNumber, serialNumber);
        filter &= serialNumberFilter;

        var startTimeFilter = builder.Eq(x => x.UnixStartTime, unixStartTime);
        filter &= startTimeFilter;

        return await _context
            .POC_DataInOutEvents
            .Find(filter)
            .FirstOrDefaultAsync();
    }

    public async Task<List<POC_DataInOutEvent>> Get_Unprocess(string serialNumber, long unixStartTime)
    {
        var builder = Builders<POC_DataInOutEvent>.Filter;
        var filter = builder.Empty;


        var serialNumberFilter = builder.Eq(x => x.SerialNumber, serialNumber);
        filter &= serialNumberFilter;

        var dataProcessedFilter = builder.Eq(x => x.DataProcessed, false);
        filter &= dataProcessedFilter;

        var unixStartTimeFilter = builder.Gt(x => x.UnixStartTime, unixStartTime);
        filter &= unixStartTimeFilter;

        return await _context
            .POC_DataInOutEvents
            .Find(filter)
            .Sort(Builders<POC_DataInOutEvent>.Sort.Ascending("UnixStartTime"))
            .ToListAsync();
    }

}
