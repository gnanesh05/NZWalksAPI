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
            var WalkDomainModel = mapper.Map<Walk>(walkCreateDTO);
            await repository.CreateAsync(WalkDomainModel);
            return Ok(mapper.Map<WalkDTO>(WalkDomainModel));
        }
    }
}
