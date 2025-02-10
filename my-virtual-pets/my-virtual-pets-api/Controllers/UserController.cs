using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using my_virtual_pets_api.Services.Interfaces;
using my_virtual_pets_class_library.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.Data.SqlClient;

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
        
        [HttpGet("/login-google")]
        public async Task LogInGoogle()
        {
            Console.WriteLine("THIS IS THE LOGIN GOOGLE");
            var properties = new AuthenticationProperties
            {
                RedirectUri = "https://localhost:7091/google-callback"
                
            };
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, properties);
        }
        
        
        [HttpGet("/google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            Console.WriteLine("THIS IS THE SIGNED-IN GOOGLE");
            var authResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authResult.Succeeded)
            {
                Console.WriteLine("Authentication failed");
                Console.WriteLine(authResult.Failure?.Message);
                return BadRequest("Authentication failed");
            }

            Guid userId = new Guid(); 

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.Name, "THIS IS A TEST"),
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
            
            // return Ok(new
            // {
            //
            //     token = new JwtSecurityTokenHandler().WriteToken(token),
            //     userid = userId
            // });
            //
            
            // create a new auth user if one does not exist? 
            
            // return a jwt token to the frontend - and log them in there 
            
            
            // If authentication succeeds, sign in the user
            // var claimsPrincipal = authResult.Principal;
            // await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
            
            // with tokens, userid, email?? 
            
            return Redirect("http://localhost:5092/test2");
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

                token = new JwtSecurityTokenHandler().WriteToken(token),
                userid = userId
            });
        }
        
        [Authorize("loginjwt")]
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
                return StatusCode(500, $"An error occurred while fetching user details: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("AddToFavourites")]
        public IActionResult AddPetToFavourites(Favourites favourites)
        {
            try
            {
                bool isSuccess = _userService.AddToFavourites(favourites.GlobalUserId, favourites.PetId);
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
        
        [Authorize]
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

        [Authorize]
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

        [Authorize]
        [HttpDelete]
        [Route("{GlobalUserId}/RemoveFromFavourites/{PetId}")]
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

        [HttpGet]
        [Route("CheckUsername/{username}")]
        public IActionResult CheckUsername(string username)
        {
            Console.WriteLine(username);
            try
            {
                bool exists = _userService.ExistsByUsername(username);
                Console.WriteLine(exists);
                return (Ok( new BoolReturn(){ IsTrue = exists} ));
            }
            catch
            {
                return StatusCode(500, "An error occurred while checking the username");
            }
        }

        [HttpGet]
        [Route("CheckEmail/{email}")]
        public IActionResult CheckEmail(string email)
        {
            try
            {
                bool exists = _userService.ExistsByEmail(email);
                return (Ok( new BoolReturn(){ IsTrue = exists} ));
            }
            catch (FormatException ex)
            {
                return BadRequest("Invalid email format.");
            }
            catch
            {
                return StatusCode(500, "An error occurred while checking the email");
            }
        }

        [Authorize]
        [HttpPut("update")]
        public IActionResult UpdateUser([FromBody] UpdateUserDTO updateduser, string currentPassword)
        {
            try
            {
                _userService.UpdateUser(updateduser, currentPassword);
                return Ok("User updated successfully.");
            }
            catch (KeyNotFoundException keyNFEx)
            {
                return NotFound(keyNFEx.Message);
            }
            catch (InvalidOperationException invalidOpEx)
            {
                return BadRequest(invalidOpEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }
    }
}
