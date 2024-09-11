using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using WebAPI.Dtos;
using WebAPI.Interfaces;

namespace WebAPI.Controllers
{
    public class PropertyController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public PropertyController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // property/list/
        [HttpGet("list/{sellRent}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPropertyList(int sellRent)
        {
            var properties = await _uow.PropertyRepository.GetPropertiesAsync(sellRent);
            var propertyListDto = _mapper.Map<IEnumerable<PropertyListDto>>(properties);
            return Ok(propertyListDto);
        }
    }
}
