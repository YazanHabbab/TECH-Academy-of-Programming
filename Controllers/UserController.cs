using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TECH_Academy_of_Programming.Constant;
using TECH_Academy_of_Programming.Interfaces;
using TECH_Academy_of_Programming.Models;
using TECH_Academy_of_Programming.Repositories;

namespace TECH_Academy_of_Programming.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUsersRepo _userRepository;
        public UserController(IUsersRepo userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(string))]
        public async Task<IActionResult> register([FromForm] UserRegisterModel user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userRepository.register(user);

            if(!result.Succeeded)
            return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(AuthModel))]
        public async Task<IActionResult> login([FromForm] UserLoginModel user)
        {
            if (!ModelState.IsValid)
                return BadRequest("Please check your username and password.");

            var result = await _userRepository.login(user);

            if (!result.IsAuthenticated)
                return Unauthorized(result);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("id")]
        [ProducesResponseType(200, Type = typeof(User))]
        public async Task<IActionResult> getProfile(string ID)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userRepository.getProfile(ID);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Authorize]
        [HttpPut("id")]
        [ProducesResponseType(200, Type = typeof(UpdateResult))]
        public async Task<IActionResult> UpdateUser(string ID, [FromForm] UpdatedUserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userRepository.updateProfile(ID, user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Message);
            }

            return Ok("User Updated: " + result.IsUpdated);
        }

        [Authorize]
        [HttpDelete("id")]
        [ProducesResponseType(200, Type = typeof(DeleteResult))]
        public async Task<IActionResult> DeleteUser(string ID)
        {
            var result = await _userRepository.delete(ID);

            if (!result.IsDeleted)
            {
                return BadRequest(result.Message);
            }

            return Ok("User with Id : \"" + ID + "\" is deleted");
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<User>))]
        public async Task<IActionResult> getUsers(int page)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var users = await _userRepository.getUsers(page);

            if (!users.Any())
            {
                return NotFound();
            }

            return Ok(users);
        }
    }
}