using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("Walks")]
    public class WalksController :Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
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
    }
}
