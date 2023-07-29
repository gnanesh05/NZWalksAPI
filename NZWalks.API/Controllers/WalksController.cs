using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository repository;
        private readonly IMapper mapper;
        public WalksController(IWalkRepository repository ,IMapper mapper) 
        {
            this.repository = repository;
            this.mapper = mapper;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WalkCreateDTO walkCreateDTO)
        {
            if(ModelState.IsValid)
            {
                var WalkDomainModel = mapper.Map<Walk>(walkCreateDTO);
                await repository.CreateAsync(WalkDomainModel);
                return Ok(mapper.Map<WalkDTO>(WalkDomainModel));
            }
            else
            {
                return BadRequest(ModelState);
            }
            
        }

        // /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=True&pageNumber&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber=1, [FromQuery] int pageSize=10)
        {
            var walkDomainModel = await repository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);
            return Ok(mapper.Map<List<WalkDTO>>(walkDomainModel));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await repository.GetByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDTO>(walkDomainModel));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] WalkUpdateDTO walkUpdateDTO)
        {
            if(ModelState.IsValid)
            {
                var walkDomainModel = mapper.Map<Walk>(walkUpdateDTO);
                walkDomainModel = await repository.UpdateAsync(id, walkDomainModel);
                if (walkDomainModel == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<WalkDTO>(walkDomainModel));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
           var deletedWalkDomainModel  = await repository.DeleteAsync(id);
           if(deletedWalkDomainModel == null)
            {
                return NotFound();
            }
           return Ok(mapper.Map<WalkDTO>(deletedWalkDomainModel));

        }
    }
}
