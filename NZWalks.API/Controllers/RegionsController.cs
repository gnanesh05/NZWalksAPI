using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        { 
             // data from database - domain models
             var regions = await dbContext.Regions.ToListAsync() ;

            // data map to dto
            var regionDTO = new List<RegionDTO>();
            foreach (var region in regions)
            {
                regionDTO.Add(new RegionDTO()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl,
                });
            }
             return Ok(regionDTO);

        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region = await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
            if(region == null) { 
                return NotFound();
            }

            var regionsDTO = new RegionDTO() 
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl,
            };
           
            return Ok(regionsDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]RegionCreateDTO region)
        {
            var regionDomainModel = new Region()
            {
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl,
                Name = region.Name,
            };

            await dbContext.Regions.AddAsync(regionDomainModel);
            await dbContext.SaveChangesAsync();

            var regionDTO = new RegionDTO()
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            return CreatedAtAction(nameof(GetById), new {id = regionDTO.Id}, regionDTO);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Create([FromRoute] Guid id, [FromBody] RegionUpdateDTO Updateregion)
        {
            var region =  await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(region == null)
            {
                return NotFound();
            }

            region.Name = Updateregion.Name;
            region.Code = Updateregion.Code;
            region.RegionImageUrl = Updateregion.RegionImageUrl;
            await dbContext.SaveChangesAsync();

            var regionDTO = new RegionDTO()
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl,
            };
            return Ok(regionDTO);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var region = await dbContext.Regions.FirstOrDefaultAsync(y => y.Id == id);
            if(region == null)
            {
                return NotFound();
            }
            dbContext.Regions.Remove(region); 
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
