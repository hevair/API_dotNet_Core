using System.Threading.Tasks;
using Api.Domain.DTOs;
using Api.Domain.Entities;
using Api.Domain.Interface.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }
        [HttpPost]
        public async Task<object> Login( [FromBody] LoginDTO login){
            

            if(!ModelState.IsValid){
                return BadRequest();
            }

            if(login == null){
                return null;
            }

            var result = await _loginService.FindByUser(login);

            if(result == null) return NotFound();

            return Ok(result);
            
        }
    }
}