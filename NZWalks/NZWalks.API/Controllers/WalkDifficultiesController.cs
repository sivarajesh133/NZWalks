using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("WalkDifficulties")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultiesRepository walkDifficultiesRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultiesRepository walkDifficultiesRepository, IMapper mapper)
        {
            this.walkDifficultiesRepository = walkDifficultiesRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultiesAsync()
        {
           var walkDifficulties=await  walkDifficultiesRepository.GetAllAsync();
           return Ok(mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulties));
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
          var walkDifficulty= await walkDifficultiesRepository.GetAsync(id);
           if(walkDifficulty==null)
            {
                return NotFound();
            }
           return Ok(mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty));   
        }
        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync(Models.DTO.WalkDifficultyAddRequest walkDifficultyAddRequest)
        {
            //Request validation
            if (!ValidateAddWalkDifficultyAsync(walkDifficultyAddRequest))
            {
                return BadRequest(ModelState);
            }
            var walkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = walkDifficultyAddRequest.Code,
            };
            walkDifficulty =await walkDifficultiesRepository.AddAsync(walkDifficulty);
            if(walkDifficulty== null)
            {
                return NotFound();
            }
            var walkDifficultyDTO =mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);

        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync(Guid id, Models.DTO.WalkDifficultyUpdateRequest walkDifficultyUpdateRequest)
        {
            //Validate request
            if(!ValidateUpdateWalkDifficultyAsync(walkDifficultyUpdateRequest))
            {
                return BadRequest(ModelState);
            }

             var walkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = walkDifficultyUpdateRequest.Code,
            };
            
            walkDifficulty=await walkDifficultiesRepository.UpdateAsync(id, walkDifficulty);
            if(walkDifficulty == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty));  
        }

        

        [HttpDelete]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
           var walkDifficulty= await walkDifficultiesRepository.DeleteAsync(id);
           return Ok(mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty));
        }

        #region Private methods
         private bool ValidateAddWalkDifficultyAsync(Models.DTO.WalkDifficultyAddRequest walkDifficultyAddRequest)
        {
            if (walkDifficultyAddRequest == null)
            {
                ModelState.AddModelError(nameof(walkDifficultyAddRequest), $"{nameof(walkDifficultyAddRequest)} Request cannot be empty");
                return false;
                
            }
            if(string.IsNullOrWhiteSpace(walkDifficultyAddRequest.Code))
            {
                ModelState.AddModelError(nameof(walkDifficultyAddRequest.Code), $"{nameof(walkDifficultyAddRequest.Code)} cannot be null or empty or white space");
            }
            if(ModelState.ErrorCount>0)
            {
                return false;
            }
            return true;
        }

        private bool ValidateUpdateWalkDifficultyAsync(Models.DTO.WalkDifficultyUpdateRequest walkDifficultyUpdateRequest)
        {
            if (walkDifficultyUpdateRequest == null)
            {
                ModelState.AddModelError(nameof(walkDifficultyUpdateRequest), $"{nameof(walkDifficultyUpdateRequest)} Request cannot be empty");
                return false;

            }
            if (string.IsNullOrWhiteSpace(walkDifficultyUpdateRequest.Code))
            {
                ModelState.AddModelError(nameof(walkDifficultyUpdateRequest.Code), $"{nameof(walkDifficultyUpdateRequest.Code)} cannot be null or empty or white space");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
