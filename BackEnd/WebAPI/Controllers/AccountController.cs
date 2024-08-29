using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;
using WebAPI.Interfaces;

namespace WebAPI.Controllers
{

    public class AccountController : BaseController
    {
        private readonly IUnitOfWork _uow;
        public AccountController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // api/account/login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginReqDto loginReq)
        {
            var user = await _uow.UserRepository.Authenticate(loginReq.Username, loginReq.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var loginRes = new LoginResDto();
            loginRes.UserName = user.Username;
            loginRes.Token = "Token to be generated.";

            return Ok(loginRes);
        }
    }
}
