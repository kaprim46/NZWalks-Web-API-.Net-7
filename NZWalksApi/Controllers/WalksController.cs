using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksApi.CustomActionFilters;
using NZWalksApi.Models.Domain;
using NZWalksApi.Models.DTO;
using NZWalksApi.Repository.IRepository;

namespace NZWalksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalksRepository _walksRepository;
        private readonly IMapper _mapper;

        public WalksController(IWalksRepository walksRepository, IMapper mapper)
        {
            _walksRepository = walksRepository;
            _mapper = mapper;
        }

        //tps://localhost:7094/api/Walks?filterQuery=walk&isAscending=false
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]string? filterQuery, bool isAscending = true, [FromQuery] int pageNumber = 1,
                                               [FromQuery] int pageSize = 1000 )
        {

                var walks = await _walksRepository.GetAllAsync(includeProperties: "Region,Difficulty");
                //filter
                if (!string.IsNullOrEmpty(filterQuery))
                    walks = await _walksRepository.GetAllAsync(q => q.Name.Contains(filterQuery), includeProperties: "Region,Difficulty");
                //sorting
                walks = isAscending ? walks.OrderBy(q => q.Name).ToList() : walks.OrderByDescending(q => q.Name).ToList();
                //pagination
                var skipResult = (pageNumber - 1) * pageSize;
                walks = walks.Skip(skipResult).Take(pageSize).ToList();

            //Create an exception
            //throw new Exception("This is a new exception");

                var walksDTO = _mapper.Map<List<WalksDTO>>(walks);
                return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetWalk([FromRoute] Guid id)
        {
            var walk = await _walksRepository.GetAsync(q => q.Id == id, includeProperties: "Region,Difficulty");
            if (walk == null)
            {
                return BadRequest();
            }
            var walkDTO = _mapper.Map<WalksDTO>(walk);
            return Ok(walkDTO);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateWalk([FromBody] WalksCreateDTO walksCreateDTO)
        {
                var walk = _mapper.Map<Walk>(walksCreateDTO);
                await _walksRepository.AddAsync(walk);
                var walkDTO = _mapper.Map<WalksDTO>(walk);
                return CreatedAtAction("GetWalk", new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid id, [FromBody] WalksUpdateDTO walksUpdateDTO)
        {
                var walk = await _walksRepository.GetAsync(q => q.Id == id);
                if (walk == null)
                {
                    return NotFound();
                }

                var walkUpdated = _mapper.Map<Walk>(walksUpdateDTO);
                walkUpdated.Id = walk.Id;
                await _walksRepository.UpdateAsync(walkUpdated);
                var walkDTO = _mapper.Map<WalksDTO>(walk);
                return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var walk = await _walksRepository.GetAsync(q => q.Id == id);
            if (walk == null)
            {
                return BadRequest();
            }
            await _walksRepository.RemoveAsync(walk);
            var walkDTO = _mapper.Map<WalksDTO>(walk);
            return Ok(walkDTO);
        }
    }
}
