﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NTTLapso.Models.General;
using NTTLapso.Models.PetitionStatus;
using NTTLapso.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NTTLapso.Controllers
{
    [ApiController]
    [Route("NTTLapso/PetitionState")]
    public class PetitionStateController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<PetitionStateController> _logger;
        private PetitionStateService _service;
        public PetitionStateController(ILogger<PetitionStateController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _service = new PetitionStateService(config);
        }


        [HttpPost]
        [Route("List")]
        [Authorize]
        public async Task<ListPetitionStatusResponse> List(IdValue? request) {
            ListPetitionStatusResponse response = new ListPetitionStatusResponse();
            List<PetitionStatusDataResponse> responseList = new List<PetitionStatusDataResponse>();
            try
            {
                responseList = await _service.List(request);
                response.IsSuccess = true;
                response.Data = responseList;
                response.Error = null;
            }
            catch (System.Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;
            }

            return response;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize]
        public async Task<PeticionStatusResponse> Create(CreatePetitionStatusRequest request)
        {
            PeticionStatusResponse response = new PeticionStatusResponse();

            try
            {
                await _service.Create(request);
                response.IsSuccess = true;
            }catch(Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;

            }

            return response;
        }

        [HttpPost]
        [Route("Edit")]
        [Authorize]
        public async Task<PeticionStatusResponse> Edit(EditPetitionStatusRequest request)
        {
            PeticionStatusResponse response = new PeticionStatusResponse();

            try
            {
                await _service.Edit(request);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;

            }

            return response;
        }

        [HttpGet]
        [Route("Delete")]
        [Authorize]
        public async Task<PeticionStatusResponse> Delete(int Id)
        {
            PeticionStatusResponse response = new PeticionStatusResponse();

            try
            {
                await _service.Delete(Id);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;

            }

            return response;
        }
    }
}
