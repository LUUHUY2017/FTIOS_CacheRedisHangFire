﻿using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleJobs;
using Server.Core.Interfaces.A2.ScheduleJobs.Reponses;
using Server.Core.Interfaces.A2.ScheduleJobs.Requests;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Infrastructure.Repositories.A2.ScheduleJobs;

public class ScheduleJobRepository : IScheduleJobRepository
{
    private readonly IMasterDataDbContext _db;
    public ScheduleJobRepository(IMasterDataDbContext biDbContext)
    {
        _db = biDbContext;
    }

    public async Task<Result<ScheduleJob>> ActiveAsync(ActiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.ScheduleJob.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = true;
                _db.ScheduleJob.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<ScheduleJob>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ScheduleJob>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<ScheduleJob>> InActiveAsync(InactiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.ScheduleJob.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = false;
                _db.ScheduleJob.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<ScheduleJob>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ScheduleJob>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<List<ScheduleJobReportResponse>> GetAlls(ScheduleJobFilterRequest request)
    {
        try
        {
            bool active = request.Actived == "1";
            var _data = await (from _do in _db.ScheduleJob
                               join _or in _db.Organization on _do.OrganizationId equals _or.Id into OT
                               from or in OT.DefaultIfEmpty()
                               where _do.Actived == active
                               && (!string.IsNullOrWhiteSpace(request.OrganizationId) ? _do.OrganizationId == request.OrganizationId : true)
                               && (!string.IsNullOrWhiteSpace(request.Key) && request.ColumnTable == "ScheduleJobName" ? _do.ScheduleJobName.Contains(request.Key) : true)
                               && (!string.IsNullOrWhiteSpace(request.ScheduleNote) ? _do.ScheduleNote == request.ScheduleNote || _do.ScheduleNote == null : true)
                               select new ScheduleJobReportResponse()
                               {
                                   Id = _do.Id,
                                   CreatedBy = _do.CreatedBy,
                                   Actived = _do.Actived,
                                   CreatedDate = _do.CreatedDate,
                                   LastModifiedDate = _do.LastModifiedDate != null ? _do.LastModifiedDate : _do.CreatedDate,
                                   OrganizationId = _do.OrganizationId,
                                   ScheduleJobName = _do.ScheduleJobName,
                                   ScheduleNote = _do.ScheduleNote,
                                   ScheduleSequential = _do.ScheduleSequential,
                                   ScheduleTime = _do.ScheduleTime,
                                   ScheduleType = _do.ScheduleType,
                                   OrganizationName = or != null ? or.OrganizationName : null,

                               }).ToListAsync();
            return _data;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task<Result<ScheduleJob>> GetById(string id)
    {
        string message = "";
        try
        {
            var _order = _db.ScheduleJob.FirstOrDefault(o => o.Id == id);
            return new Result<ScheduleJob>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ScheduleJob>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<ScheduleJob>> UpdateAsync(ScheduleJob data)
    {
        string message = "";
        try
        {
            var _order = _db.ScheduleJob.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _db.ScheduleJob.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new ScheduleJob();
                data.CopyPropertiesTo(_order);
                _db.ScheduleJob.Add(_order);
                message = "Thêm mới thành công";
            }

            var retVal = await _db.SaveChangesAsync();
            return new Result<ScheduleJob>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ScheduleJob>(data, "Lỗi: " + ex.ToString(), false);
        }
    }


}
