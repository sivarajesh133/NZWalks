﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{

    [ApiController]
    [Route("Regions")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {

            var regions=await regionRepository.GetAllAsync();
            //return DTO region
            /* var regionsDTO = new List<Models.DTO.Region>();
              regions.ToList().ForEach(region=>
               {
                   var regionDTO = new Models.DTO.Region()
                   {
                       Id = region.Id,
                       Name = region.Name,
                       Code = region.Code,
                       Area = region.Area,
                       Lat = region.Lat,
                       Long = region.Long,
                       Population = region.Population,
                   };
                   regionsDTO.Add(regionDTO);
            });*/

            var regionsDTO =mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsDTO);
            
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
           var region= await regionRepository.GetAsync(id);
            if(region == null)
            {
                return NotFound();
            }
            var regionDTO =mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //Request(incoming region) to Domain Model
           var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population,
            };
           //var region =mapper.Map<Models.Domain.Region>(addRegionRequest);
             //Passing details to repository

              region = await regionRepository.AddAsync(region);


            //Converting domain to DTO
            /* var regionDTO = new Models.DTO.Region()
             {
                 Id=region.Id,
                 Code = region.Code,
                 Area = region.Area,
                 Lat = region.Lat,
                 Long = region.Long,
                 Name = region.Name,
                 Population = region.Population,
             };*/

            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            return  CreatedAtAction(nameof(GetRegionAsync), new {id=regionDTO.Id}, regionDTO);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
           var region= await regionRepository.DeleteAsync(id);

            return Ok(region);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population,
            };

            region=await regionRepository.UpdateAsync(id, region);
            if(region==null)
            {
                return NotFound();
            }
            var regionDTO= mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);
        }
    }
}
