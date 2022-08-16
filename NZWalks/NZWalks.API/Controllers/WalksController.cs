using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("Walks")]
    public class WalksController :Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultiesRepository walkDifficultiesRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultiesRepository walkDifficultiesRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultiesRepository = walkDifficultiesRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalkAsync()
        {
           var walks= await walkRepository.GetAllWalksAsync();
           var walksDTO= mapper.Map<List<Models.DTO.Walk>>(walks);
            return Ok(walksDTO);
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            var walk =await walkRepository.GetWalksAsync(id);
            var walkDTO=mapper.Map<Models.DTO.Walk>(walk);
            return Ok(walkDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody]Models.DTO.AddWalkRequest addWalkRequest)
        {
            //validate request
            if (!(await ValidateAddWalkAsync(addWalkRequest)))
                {
                return BadRequest(ModelState);
                }
            
            var walk = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId,
            };
            walk=await walkRepository.AddAsync(walk);
            if(walk==null)
            {
                return NotFound();
            }
            var walkDTO=mapper.Map<Models.DTO.Walk>(walk);
             return CreatedAtAction(nameof(GetWalkAsync), new {id=walkDTO.Id}, walkDTO);    

        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync(Guid id, Models.DTO.WalkUpdateRequest walkUpdateRequest)
        {
            //validate request
            if (!(await ValidateUpdateWalkAsync(walkUpdateRequest)))
            {
                return BadRequest(ModelState);
            }
            //DTO  to domain
            var walk = new Models.Domain.Walk()
            {
                Length = walkUpdateRequest.Length,
                Name = walkUpdateRequest.Name,
                RegionId = walkUpdateRequest.RegionId,
                WalkDifficultyId = walkUpdateRequest.WalkDifficultyId,
            };

            //domain to repository
            walk=await walkRepository.UpdateAsync(id, walk);   
            //check if  null
            if(walk==null)
            {
                return NotFound();
            }

            //domain to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
             return Ok(walkDTO);



        }
        [HttpDelete]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            var walk=await walkRepository.DeleteAsync(id);
            var walkDTO =mapper.Map<Models.DTO.Walk>(walk);
            return Ok(walkDTO);

        }

        #region PrivateMethods


        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if(addWalkRequest==null)
            {
                ModelState.AddModelError(nameof(addWalkRequest), "Request body canoot be empty");
                return false;
            }
            if(string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} cannot be null or whitespace or empty");
            }
            if ((addWalkRequest.Length)<=0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)} should be grater than zero");
            }
            var region =await regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region==null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} is invalid");
            }
            var walkdifficulty=await walkDifficultiesRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if(walkdifficulty==null)
            {

                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid");
            }
            if (ModelState.ErrorCount>0)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.WalkUpdateRequest walkUpdateRequest)
        {
            if (walkUpdateRequest == null)
            {
                ModelState.AddModelError(nameof(walkUpdateRequest), "Request body canoot be empty");
                return false;
            }
            if (string.IsNullOrWhiteSpace(walkUpdateRequest.Name))
            {
                ModelState.AddModelError(nameof(walkUpdateRequest.Name), $"{nameof(walkUpdateRequest.Name)} cannot be null or whitespace or empty");
            }
            if ((walkUpdateRequest.Length) <= 0)
            {
                ModelState.AddModelError(nameof(walkUpdateRequest.Length), $"{nameof(walkUpdateRequest.Length)} should not  be less than or equal to zero");
            }
            var region = await regionRepository.GetAsync(walkUpdateRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(walkUpdateRequest.RegionId), $"{nameof(walkUpdateRequest.RegionId)} is invalid");
            }
            var walkdifficulty = await walkDifficultiesRepository.GetAsync(walkUpdateRequest.WalkDifficultyId);
            if (walkdifficulty == null)
            {

                ModelState.AddModelError(nameof(walkUpdateRequest.WalkDifficultyId), $"{nameof(walkUpdateRequest.WalkDifficultyId)} is invalid");
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
