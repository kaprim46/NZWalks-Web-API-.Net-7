using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksApi.Data;
using NZWalksApi.Models.Domain;
using NZWalksApi.Models.DTO;
using NZWalksApi.Repository.IRepository;

namespace NZWalksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DifficultyController : ControllerBase
    {
        private readonly IDifficultyRepository _difficultyRepository;
        private readonly IMapper _mapper;

        public DifficultyController(IDifficultyRepository difficultyRepository, IMapper mapper)
        {
            _difficultyRepository = difficultyRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var difficulties = await _difficultyRepository.GetAllAsync();

            var difficultiesDTO = _mapper.Map<List<DifficultyDTO>>(difficulties);
            return Ok(difficultiesDTO);
        }


        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetDifficulty([FromRoute] Guid id)
        {
            var difficulty = await _difficultyRepository.GetAsync(q => q.Id == id);
            if (difficulty == null)
            {
                return NotFound();
            }

            DifficultyDTO difficultyDTO = _mapper.Map<DifficultyDTO>(difficulty);
            return Ok(difficultyDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDifficulty([FromBody] DifficultyCreateDTO difficultyCreateDTO)
        {
            if (ModelState.IsValid)
            {
                var difficulty = _mapper.Map<Difficulty>(difficultyCreateDTO);

                await _difficultyRepository.AddAsync(difficulty);

                var difficultyDTO = _mapper.Map<DifficultyDTO>(difficulty);
                return CreatedAtAction("GetDifficulty", new { id = difficultyDTO.Id}, difficultyDTO);
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateDifficulty([FromRoute]Guid id, [FromBody]DifficultyUpdateDTO difficultyUpdateDTO)
        {
            var difficulty = await _difficultyRepository.GetAsync(q => q.Id == id);
            if (difficulty == null)
            {
                return NotFound();
            }

            //difficulty.Name = difficultyUpdateDTO.Name;// without using update because entityframwork tracked this entity

            await _difficultyRepository.UpdateAsync(difficulty);

            var difficultyDTO = _mapper.Map<DifficultyDTO>(difficulty);
            return Ok(difficultyDTO);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteDifficulty([FromRoute] Guid id)
        {
            var difficulty = await _difficultyRepository.GetAsync(q => q.Id == id);
            if (difficulty == null)
            {
                return NotFound();
            }

            await _difficultyRepository.RemoveAsync(difficulty);

            var difficultyDTO = _mapper.Map<DifficultyDTO>(difficulty);

            return Ok(difficultyDTO);
        }
    }
}
