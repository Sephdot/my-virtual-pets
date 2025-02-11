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
using my_virtual_pets_api.Entities;


namespace my_virtual_pets_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly ITokenService _tokenService;
        
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, IConfiguration configuration, ITokenService tokenService)
        {
            _configuration = configuration;
            _userService = userService;
            _tokenService = tokenService;
        }
        
        [HttpGet("/login-google")]
        public async Task LogInGoogle()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "https://localhost:7091/google-callback"
                
            };
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, properties);
        }
        
        
        [HttpGet("/google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var authResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authResult.Succeeded)
            {
                return BadRequest("Authentication failed");
            }
            
            var autho = authResult.Principal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var email = authResult.Principal.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            var fullname = authResult.Principal.Claims.First(x => x.Type == ClaimTypes.GivenName).Value;
            
            var userId = await _userService.CreateNewAuthUser(email, fullname, autho);
            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, "User"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            OAuthToken newToken = new OAuthToken()
                { Auth0Id = userId, Token = new JwtSecurityTokenHandler().WriteToken(token), Email = email };

            var cacheKey = Guid.NewGuid(); 
            _tokenService.StoreToken(cacheKey, newToken);

            return Redirect($"http://localhost:5092/oauth/{cacheKey}");
        }

        [HttpGet("tokenexchange/{cacheKey}")]
        public async Task<IActionResult> TokenExchange(Guid cacheKey)
        {
            OAuthToken returnToken = _tokenService.GetToken(cacheKey);
            if (returnToken == null) return BadRequest("No token found");
            return Ok(returnToken);
        }


        [HttpPost("register")]
        public async Task<IActionResult> NewLocalUser(NewUserDTO newUserDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (await _userService.ExistsByEmail(newUserDto.Email)) return BadRequest("Email already registered");
            if (await _userService.ExistsByUsername(newUserDto.Username)) return BadRequest("Username already taken");

            await _userService.CreateNewLocalUser(newUserDto);

            return Created("/register", "New local user created");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserLoginDTO userLoginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!await _userService.ExistsByUsername(userLoginDto.Username)) return BadRequest("This username does not exist");
            if (!await _userService.DoesPasswordMatch(userLoginDto)) return BadRequest("Password is incorrect");

            Guid userId = await _userService.GetUserIdByUsername(userLoginDto.Username);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.Name, userLoginDto.Username),
                new Claim(ClaimTypes.Role, "User"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:Jwt:SecretKey"]));
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
        
        // [Authorize(AuthenticationSchemes = "loginjwt")]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserDetailsByUserId(Guid userId)
        {
            var userDisplayDTO = await _userService.GetUserDetailsByUserId(userId);

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

        // [Authorize(AuthenticationSchemes = "loginjwt")]
        [HttpPost]
        [Route("AddToFavourites")]
        public async Task<IActionResult> AddPetToFavourites(Favourites favourites)
        {
            try
            {
                bool isSuccess = await _userService.AddToFavourites(favourites.GlobalUserId, favourites.PetId);
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
        
        [Authorize(AuthenticationSchemes = "loginjwt")]
        [HttpGet]
        [Route("{GlobalUserId}/FavouritePetIds")]
        public async Task<IActionResult> GetFavouritePetIds(Guid GlobalUserId)
        {
            try
            {
                var favouritePetIds = await _userService.GetFavouritePetId(GlobalUserId);
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

        // [Authorize(AuthenticationSchemes = "loginjwt")]
        [HttpGet]
        [Route("{GlobalUserId}/FavouritePets")]
        public async Task<IActionResult> GetFavouritePets(Guid GlobalUserId)
        {
            try
            {
                var favouritePets = await _userService.GetFavouritePets(GlobalUserId);
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

        [Authorize(AuthenticationSchemes = "loginjwt")]
        [HttpDelete]
        [Route("{GlobalUserId}/RemoveFromFavourites/{PetId}")]
        public async Task<IActionResult> RemoveFromFavourite(Guid GlobalUserId, Guid PetId)
        {
            try
            {
                bool isSuccess = await _userService.RemoveFromFavourites(GlobalUserId, PetId);
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
        public async Task<IActionResult> CheckUsername(string username)
        {
            Console.WriteLine(username);
            try
            {
                bool exists = await _userService.ExistsByUsername(username);
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
        public async Task<IActionResult> CheckEmail(string email)
        {
            try
            {
                bool exists = await _userService.ExistsByEmail(email);
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

        [Authorize(AuthenticationSchemes = "loginjwt")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO updateduser, string currentPassword)
        {
            try
            {
                await _userService.UpdateUser(updateduser, currentPassword);
                return Ok("User updated successfully.");
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
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
