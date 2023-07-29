using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        { 
             // data from database - domain models
             var regions = await regionRepository.GetAllAsync() ;

            // data map to dto
            //var regionDTO = new List<RegionDTO>();
            //foreach (var region in regions)
            //{
            //    regionDTO.Add(new RegionDTO()
            //    {
            //        Id = region.Id,
            //        Name = region.Name,
            //        Code = region.Code,
            //        RegionImageUrl = region.RegionImageUrl,
            //    });
            //}

            // mapping RegionDomain to RegionDTO
             return Ok(mapper.Map<List<RegionDTO>>(regions));

        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region = await regionRepository.GetByIdAsync(id);
            if(region == null) { 
                return NotFound();
            }

            //var regionsDTO = new RegionDTO() 
            //{
            //    Id = region.Id,
            //    Name = region.Name,
            //    Code = region.Code,
            //    RegionImageUrl = region.RegionImageUrl,
            //};
           
            //Map RegionDomain to RegionDTO
            return Ok(mapper.Map<RegionDTO>(region));
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody]RegionCreateDTO region)
        {
                //var regionDomainModel = new Region()
                //{
                //    Code = region.Code,
                //    RegionImageUrl = region.RegionImageUrl,
                //    Name = region.Name,
                //};

                var regionDomainModel = mapper.Map<Region>(region);

                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                //var regionDTO = new RegionDTO()
                //{
                //    Id = regionDomainModel.Id,
                //    Name = regionDomainModel.Name,
                //    Code = regionDomainModel.Code,
                //    RegionImageUrl = regionDomainModel.RegionImageUrl,
                //};

                var regionDTO = mapper.Map<RegionDTO>(region);
                return CreatedAtAction(nameof(GetById), new { id = regionDTO.Id }, regionDTO);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] RegionUpdateDTO Updateregion)
        {
                //var regionDomainModel = new Region()
                //{
                //    Id = id,
                //    Code = Updateregion.Code,
                //    Name = Updateregion.Name,
                //    RegionImageUrl = Updateregion.RegionImageUrl,
                //};
                var regionDomainModel = mapper.Map<Region>(Updateregion);
                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                regionDomainModel.Name = Updateregion.Name;
                regionDomainModel.Code = Updateregion.Code;
                regionDomainModel.RegionImageUrl = Updateregion.RegionImageUrl;
                await dbContext.SaveChangesAsync();

                //var regionDTO = new RegionDTO()
                //{
                //    Id = regionDomainModel.Id,
                //    Name = regionDomainModel.Name,
                //    Code = regionDomainModel.Code,
                //    RegionImageUrl = regionDomainModel.RegionImageUrl,
                //};
                return Ok(mapper.Map<RegionDTO>(regionDomainModel));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionModel = await regionRepository.DeleteAsync(id);
            if(regionModel == null)
            {
                return NotFound();
            }
            //var regionDTO = new RegionDTO()
            //{
            //    Id = regionModel.Id,
            //    Code = regionModel.Code,
            //    Name = regionModel.Name,
            //    RegionImageUrl = regionModel.RegionImageUrl,
            //};

            return Ok(mapper.Map<RegionDTO>(regionModel));
        }
    }
}
