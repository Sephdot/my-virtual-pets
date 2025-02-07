using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library.DTO;
using System.Security.Claims;

namespace my_virtual_pets_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IConfiguration _configuration;
        
        public UserController(IUserService userService, IConfiguration configuration)
        {
            _configuration = configuration;
            _userService = userService;
        }


        [HttpPost("register")]
        public IActionResult NewLocalUser(NewUserDTO newUserDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (_userService.ExistsByEmail(newUserDto.Email)) return BadRequest("Email already registered");
            if (_userService.ExistsByUsername(newUserDto.Username)) return BadRequest("Username already taken");

            _userService.CreateNewLocalUser(newUserDto);

            return Created("/register", "New local user created");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserLoginDTO userLoginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!_userService.ExistsByUsername(userLoginDto.Username)) return BadRequest("This username does not exist");
            if (!_userService.DoesPasswordMatch(userLoginDto)) return BadRequest("Password is incorrect");

            
            Console.WriteLine($"User {userLoginDto.Username} logged in");
            Guid userId = _userService.GetUserIdByUsername(userLoginDto.Username);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.Name, userLoginDto.Username),
                new Claim(ClaimTypes.Role, "User"),
            };
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), 
                signingCredentials: creds
            );

            return Ok(new
            {

                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        
        [Authorize]
        [HttpGet("auth")]
        public IActionResult AuthCheck()
        {
            var firstClaim = User.Claims.ElementAtOrDefault(0)?.Value;
            return Ok(new CurrentUserDTO() { Id = firstClaim,  Username = User.Identity.Name } );
        }

        
        [HttpGet("{userId}")]
        public IActionResult GetUserDetailsByUserId(Guid userId)
        {
            var userDisplayDTO = _userService.GetUserDetailsByUserId(userId);

            try
            {
                if (userDisplayDTO == null)
                {
                    return NotFound($"User with ID {userId} not found.");
                }
                return Ok(userDisplayDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching top pets: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("{GlobalUserId}/AddToFavourites{PetId}")]

        public IActionResult AddPetToFavourites(Guid GlobalUserId, Guid PetId)
        {
            try
            {
                bool isSuccess = _userService.AddToFavourites(GlobalUserId, PetId);
                if (isSuccess) return NoContent();
                else return Conflict("Pet is already favourited");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{GlobalUserId}/FavouritePetIds")]
        public IActionResult GetFavouritePetIds(Guid GlobalUserId)
        {
            try
            {
                var favouritePetIds = _userService.GetFavouritePetId(GlobalUserId);
                return Ok(favouritePetIds);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("User not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{GlobalUserId}/FavouritePets")]
        public IActionResult GetFavouritePets(Guid GlobalUserId)
        {
            try
            {
                var favouritePets = _userService.GetFavouritePets(GlobalUserId);
                return Ok(favouritePets);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("User not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{GlobalUserId}/RemoveFromFavourites{PetId}")]
        public IActionResult RemoveFromFavourite(Guid GlobalUserId, Guid PetId)
        {
            try
            {
                bool isSuccess = _userService.RemoveFromFavourites(GlobalUserId, PetId);
                if (isSuccess) return NoContent();
                else return Conflict("Pet is already unfavourited");

            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        [HttpPut("update")]
        public IActionResult UpdateUser([FromBody] UpdateUserDTO updateduser)
        {
            try
            {
                _userService.UpdateUser(updateduser);
                return Ok("User updated successfully.");
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (InvalidOperationException invOpEx)
            {
                return BadRequest(invOpEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }
    }
}
