using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using WebAPI.Interfaces;

namespace WebAPI.Controllers
{
    public class PropertyController : BaseController
    {
        private readonly IUnitOfWork _uow;
        public PropertyController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // property/type/
        [HttpGet("type/{sellRent}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPropertyList(int sellRent)
        {
            var properties = await _uow.PropertyRepository.GetPropertiesAsync(sellRent);
            return Ok(properties);
        }
    }
}
