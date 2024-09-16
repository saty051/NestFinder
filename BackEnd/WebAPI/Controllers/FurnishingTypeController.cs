using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;
using WebAPI.Interfaces;

namespace WebAPI.Controllers
{
    public class FurnishingTypeController: BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public FurnishingTypeController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // GET api/FurnishingType/list
        [HttpGet("list")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFurnishingTypes()
        {
            var furnishingTypes = await _uow.FurnishingTypeRepository.GetFurnishingTypeAsync();
            var furnshingTypesDto = _mapper.Map<IEnumerable<KeyValuePairDto>>(furnishingTypes);
            return Ok(furnshingTypesDto);
        }
    }
}
