﻿using AMMS.VIETTEL.SMAS.Applications.Services.Organizations.V1.Models;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Identity.Entities;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.Organizations;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Shared.Core.Commons;

namespace AMMS.VIETTEL.SMAS.Applications.Services.Organizations.V1;

public class OrganizationService
{
    public string? UserId { get; set; }
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IOrganizationRepository _organizationRepository;
    private readonly IMapper _mapper;
    public OrganizationService(IOrganizationRepository organizationRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _organizationRepository = organizationRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Result<Organization>> SaveAsync(OrganizationRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                var dataAdd = _mapper.Map<Organization>(request);
                var retVal = await _organizationRepository.AddAsync(dataAdd);
                return retVal;
            }
            else
            {
                var data = await _organizationRepository.GetByIdAsync(request.Id);
                var dataUpdate = data.Data;
                dataUpdate.OrganizationName = request.OrganizationName;
                dataUpdate.OrganizationAddress = request.OrganizationAddress;
                dataUpdate.OrganizationCode = request.OrganizationCode;
                dataUpdate.OrganizationDescription = request.OrganizationDescription;

                var retVal = await _organizationRepository.UpdateAsync(dataUpdate);
                return retVal;
            }
        }
        catch (Exception ex)
        {
            return new Result<Organization>(null, $"Lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<OrganizationResponse>> DeleteAsync(DeleteRequest request)
    {
        try
        {
            var item = await _organizationRepository.GetByIdAsync(request.Id);
            var retVal = await _organizationRepository.DeleteAsync(request);
            var itemMap = _mapper.Map<OrganizationResponse>(item);

            return new Result<OrganizationResponse>(itemMap, retVal.Message, false);
        }
        catch (Exception ex)
        {
            return new Result<OrganizationResponse>(null, $"Lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<List<OrganizationResponse>>> Gets()
    {
        try
        {
            var retVal = await _organizationRepository.GetAllAsync();

            var listMap = _mapper.Map<List<OrganizationResponse>>(retVal);
            return new Result<List<OrganizationResponse>>(listMap, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<List<OrganizationResponse>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<List<OrganizationResponse>>> GetForUser()
    {
        try
        {
            var retVal = await _organizationRepository.GetAllAsync();
            var userOrg = (await _userManager.FindByIdAsync(UserId))?.OrganizationId;

            if (userOrg != "" && userOrg != null && userOrg != "0")
            {
                retVal = retVal.Where(x => x.Id == userOrg).ToList();
            }
            var listMap = _mapper.Map<List<OrganizationResponse>>(retVal);
            return new Result<List<OrganizationResponse>>(listMap, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<List<OrganizationResponse>>(null, $"Có lỗi: {ex.Message}", false);
        }
    }

    public async Task<Result<OrganizationResponse>> GetFirstOrDefault()
    {
        try
        {
            var retVal = await _organizationRepository.GetByFirstAsync(x => x.Actived == true);

            var itemMap = _mapper.Map<OrganizationResponse>(retVal.Data);
            return new Result<OrganizationResponse>(itemMap, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<OrganizationResponse>(null, $"Có lỗi: {ex.Message}", false);
        }
    }





}
