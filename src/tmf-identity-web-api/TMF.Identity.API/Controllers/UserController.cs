using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMF.Identity.API.Dto;
using TMF.Identity.Core.Interfaces;
using TMF.Identity.Core.Model;

namespace TMF.Identity.API.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;

        public UserController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        /// <summary>
        /// Get all registered users
        /// </summary>
        /// <returns>
        /// List with all registered users
        /// </returns> 
        /// <response code="200">List with registered users</response>
        /// <response code="401">Access denied</response>
        /// <response code="404">No users found</response>
        /// <response code="500">Something went wrong</response>
        [HttpGet("/all", Name = "Get all registered users")]
        [ProducesResponseType(typeof(IReadOnlyList<UserEntity>), 200)]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var allUsers = await _userManagementService.GetAllUsersAsync();
            return Ok(allUsers);
        }


        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <returns>
        /// User specified by ID
        /// </returns> 
        /// <response code="200">User found by ID</response>
        /// <response code="401">Access denied</response>
        /// <response code="404">No user found with specified ID</response>
        /// <response code="500">Something went wrong</response>
        [HttpGet("/{userId}", Name = "Get user by ID")]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> GetUserByIdAsync(string userId)
        {
            var user = await _userManagementService.GetUserAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <response code="201">Newly created user</response>
        /// <response code="401">Access denied</response>
        /// <response code="400">Model is not valid</response>
        /// <response code="500">Something went wrong</response>
        [HttpPost("", Name = "Create new user")]
        [ProducesResponseType(typeof(object), 201)]
        public async Task<IActionResult> CreateNewUserAsync([FromBody] CreateUserDto newUser)
        {
            var userEntity = new UserEntity
            {
                Email = newUser.Email,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName
            };
            var createdUser = await _userManagementService.CreateUserAsync(userEntity);

            return Ok(createdUser);
        }

        /// <summary>
        /// Delete user by ID
        /// </summary>
        /// <response code="401">Access denied</response>
        /// <response code="400">Model is not valid</response>
        /// <response code="500">Something went wrong</response>
        [HttpDelete("/{userId}", Name = "Delete user by ID")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] string userId)
        {
            await _userManagementService.DeleteUserAsync(userId);
            return Ok();
        }
    }
}
