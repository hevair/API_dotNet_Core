using System;
using System.Net;
using System.Threading.Tasks;
using Api.Domain.DTOs;
using Api.Domain.Entities;
using Api.Domain.Interface.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Controllers
{
    [Authorize("Bearer")]
    [Route ("api/[controller]")]
    [ApiController]
    public class UsersController: ControllerBase
    {

        private readonly IUserService _userService;
        
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserEntity), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UserDTO>> GetAll(){

            if(!ModelState.IsValid){
                return BadRequest();
            }

            return Ok(await _userService.GetAll());
        }

        [HttpGet]
        [Route("{id}",Name = "Get")]
        public async Task<ActionResult<UserDTO>> Get(Guid id){
            if(!ModelState.IsValid){
                return BadRequest();
            }

            var result = await _userService.Get(id);

            if(result == null){
                return null;
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("user",Name ="Create")]
        [ProducesResponseType(typeof(UserEntity), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UserDTO>> Create( [FromBody] UserDTOCreate user){
            if(!ModelState.IsValid){
                return BadRequest();
            }
   
            var result = await _userService.Post(user);

            
            if(result == null){
                return null;
            }

            return CreatedAtRoute("Create",user);

        }
    }
}