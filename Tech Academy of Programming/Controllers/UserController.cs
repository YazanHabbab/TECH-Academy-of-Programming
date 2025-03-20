using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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

        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="user">Username should be 3 characters long at least and not exist, Email should be valid and not exist, Password should be 4 characters long at least</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/user/register
        ///
        ///     {
        ///        "firstName": "John",
        ///        "lastName": "Doe",
        ///        "Email": "John_Doe@gmail.com",
        ///        "Password": "john123",
        ///     }
        /// </remarks>
        /// <response code="200">If the user registeration was successful</response>
        /// <response code="400">If the username or email or password is not valid or the register failed</response>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> register([FromBody] UserRegisterModel user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userRepository.register(user);

            if (!result.Succeeded)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        /// <summary>
        /// Logs user in using his credentials and return new json web token for user authentication
        /// </summary>
        /// <param name="user"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/user/login
        ///
        ///     {
        ///        "UserNameOrEmail": "John_Doe",
        ///        or "UserNameOrEmail": "John_Doe@gmail.com",
        ///        "Password": "john321"
        ///     }
        /// </remarks>
        /// <response code="200">If the login was successful</response>
        /// <response code="400">If the username or email or password is not valid</response>
        /// <response code="401">If the login failed</response>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(AuthModel))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(401, Type = typeof(string))]
        public async Task<IActionResult> login([FromBody] UserLoginModel user)
        {
            if (!ModelState.IsValid)
                return BadRequest("Please check your username and password.");

            var result = await _userRepository.login(user);

            if (!result.IsAuthenticated)
                return Unauthorized(result);

            return Ok(result);
        }

        /// <summary>
        /// Returns the user profile with specific Id
        /// </summary>
        /// <param name="ID"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/user/getProfile?Id=ebf825e3-d70e-48da-abd4-9570dfc08a66
        ///
        /// </remarks>
        /// <response code="200">If the request is successful</response>
        /// <response code="400">If the ModelState is not valid or the operation failed</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="404">If the user is not found</response>
        [Authorize]
        [HttpGet("{Id}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> getProfile(string ID)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userRepository.getProfile(ID);

            if (user is null)
            {
                return NotFound("The user is not found.");
            }

            return Ok(user);
        }

        /// <summary>
        /// Updates the user profile with specific Id
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="user">New Username and new Email should not exist new password should be 4 characters long at least</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/user/updateUser?Id=ebf825e3-d70e-48da-abd4-9570dfc08a66
        ///
        ///     {
        ///        "firstName": "John",
        ///        "lastName": "Doe",
        ///        "Email": "John_Doe@gmail.com",
        ///        "oldPassword": "john123",
        ///        "newPassword": "doe321"
        ///     }
        /// </remarks>
        /// <response code="200">If the request is successful</response>
        /// <response code="400">If the ModelState is not valid or the operation failed</response>
        /// <response code="401">If the user is not authenticated</response>
        [Authorize]
        [HttpPut("{Id}")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> updateUser(string ID, [FromBody] UpdatedUserModel user)
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

        /// <summary>
        /// Deletes user using user Id
        /// </summary>
        /// <param name="ID"></param>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /api/user/DeleteUser?Id=ebf825e3-d70e-48da-abd4-9570dfc08a66
        /// </remarks>
        /// <response code="200">If the delete is successful</response>
        /// <response code="400">If the user did not delete</response>
        /// <response code="401">If the user is not authenticated</response>
        [Authorize]
        [HttpDelete("{Id}")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> deleteUser(string ID)
        {
            var result = await _userRepository.delete(ID);

            if (!result.IsDeleted)
            {
                return BadRequest(result.Message);
            }

            return Ok("User with Id : \"" + ID + "\" is deleted");
        }


        /// <summary>
        /// Gets all the users (only for admin use)
        /// </summary>
        /// <param name="size">Default is 5</param>
        /// <param name="page">Default is 1</param>
        /// <returns>A list of users with pagination(size is the number of users to be returned by page)</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/user/getUsers?size=10&amp;page=1 
        /// </remarks>
        /// <response code="200">If the request is successful</response>
        /// <response code="400">If the ModelState is not valid</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized</response>
        /// <response code="404">If no users were found</response>
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<User>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> getUsers(int size, int page)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var users = await _userRepository.getUsers(size, page);

            if (!users.Any())
            {
                return NotFound("No users were found.");
            }

            return Ok(users);
        }
    }
}