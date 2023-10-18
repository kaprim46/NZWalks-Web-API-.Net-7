using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksApi.CustomActionFilters;
using NZWalksApi.Data;
using NZWalksApi.Models.Domain;
using NZWalksApi.Models.DTO;
using NZWalksApi.Repository.IRepository;

namespace NZWalksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(IRegionRepository regionRepository,
                                 IMapper mapper,
                                 ILogger<RegionsController> logger)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpGet]
        //[Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> GetAll()
        {
            //_logger.LogInformation("GetAll Regions Action Method was invoked");
            //_logger.LogWarning("This is a warning log");
            
            try
            {
                //throw new Exception("This is a custon exception");

                var regions = await _regionRepository.GetAllAsync();

                //var regionsDTO = new List<RegionDTO>();
                //foreach (var item in regions)
                //{
                //    regionsDTO.Add(new RegionDTO
                //    {
                //        Id = item.Id,
                //        Name = item.Name,
                //        Code = item.Code,
                //        RegionImageUrl = item.RegionImageUrl
                //    });
                //}
                //convert the object to a json data
                //_logger.LogInformation($"Finsished GetAll Regions request with data: {JsonSerializer.Serialize(regions)}");
                var regionsDTO = _mapper.Map<List<RegionDTO>>(regions);

                return Ok(regionsDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            
        }


        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> GetRegion([FromRoute] Guid id)
        {
            var region = await _regionRepository.GetAsync(q => q.Id == id);
            if (region == null)
            {
                return NotFound();
            }
            //RegionDTO regionDTO = new()
            //{
            //    Id = region.Id,
            //    Name = region.Name,
            //    Code = region.Code,
            //    RegionImageUrl = region.RegionImageUrl
            //};
            RegionDTO regionDTO = _mapper.Map<RegionDTO>(region);

            return Ok(regionDTO);
        }

        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateRegion([FromBody] RegionCreateDTO regionCreateDTO)
        {
            //var region = new Region
                //{
                //    Name = regionCreateDTO.Name,
                //    Code = regionCreateDTO.Code,
                //    RegionImageUrl = regionCreateDTO.RegionImageUrl
                //};
                var region = _mapper.Map<Region>(regionCreateDTO);
                await _regionRepository.AddAsync(region);
                //var regionDTO = new RegionDTO
                //{
                //    Id = region.Id,
                //    Code = region.Code,
                //    Name = region.Name,
                //    RegionImageUrl = region.RegionImageUrl
                //};
                var regionDTO = _mapper.Map<RegionDTO>(region);
                return CreatedAtAction("GetRegion", new { id = regionDTO.Id}, regionDTO);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateRegion([FromRoute]Guid id, [FromBody]RegionUpdateDTO regionUpdateDTO)
        {
                var region = await _regionRepository.GetAsync(q => q.Id == id);
                if (region == null)
                {
                    return NotFound();
                }

            region.Code = regionUpdateDTO.Code;
            region.Name = regionUpdateDTO.Name;// without using update because entityframwork tracked this entity
            region.RegionImageUrl = regionUpdateDTO.RegionImageUrl;

            await _regionRepository.UpdateAsync(region);
                
                //var regionDTO = new RegionDTO
                //{
                //    Id = region.Id,
                //    Code = region.Code,
                //    Name = region.Name,
                //    RegionImageUrl = region.RegionImageUrl
                //};
                var regionDTO = _mapper.Map<RegionDTO>(region);
                return Ok(regionDTO);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            var region = await _regionRepository.GetAsync(q => q.Id == id);
            if (region == null)
            {
                return NotFound();
            }

            await _regionRepository.RemoveAsync(region);

            //var regionDTO = new RegionDTO
            //{
            //    Id = region.Id,
            //    Code = region.Code,
            //    Name = region.Name,
            //    RegionImageUrl = region.RegionImageUrl
            //};
            var regionDTO = _mapper.Map<RegionDTO>(region);
            return Ok(regionDTO);
        }
    }
}
