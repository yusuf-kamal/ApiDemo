using ApiDemo.Dtos;
using ApiDemo.Response_Module;
using AutoMapper;
using Core.Entities.Identity;
using Core.Entities.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ApiDemo.Controllers
{
   
    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager
            ,SignInManager<AppUser>  signInManager ,
            ITokenService tokenService,IMapper mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _signInManager = signInManager;
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            //var Email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
                return NotFound(new ApiResponse(404));

            return new UserDto
            {
                Email = Email,
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user)
        };

        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login( LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) 
                return Unauthorized(new ApiResponse(401));
            var result= await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password,false);
            if(!result.Succeeded)
                return Unauthorized(new ApiResponse(401));

            return new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user)
            };
        }


        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register( ReqisterDto reqisterDto)
        {
            if (CheckEmailAsync(reqisterDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValdiationErrorResponse
                {
                    Errors = new[]
                    {
                        "Email Address is used"
                    }
                });

            }
            var user = new AppUser
            {
                DisplayName = reqisterDto.DisplayName,
                Email = reqisterDto.Email,
                UserName = reqisterDto.Email
            };
            var result = await _userManager.CreateAsync(user,reqisterDto.Password);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));
                     return new UserDto
                     {
                         Email = user.Email,
                         DisplayName = user.DisplayName,
                         Token = _tokenService.CreateToken(user)
                     };
        }
        [HttpGet("emailexits")]
        public async Task<ActionResult<bool>> CheckEmailAsync([FromQuery] string email)
        {
          return  await _userManager.FindByEmailAsync(email) != null;
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.Users.Include(x => x.address)
                .SingleOrDefaultAsync(x => x.Email == Email);
            return Ok(_mapper.Map<AddressDto>(user.address));
        }

        [Authorize]
        [HttpPost("updateAddress")]
        public async Task<ActionResult<AddressDto>>UpdateUserAddress(AddressDto addressDto)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.Users.Include(x => x.address)
                .SingleOrDefaultAsync(x => x.Email == Email);
            user.address=_mapper.Map<Address>(addressDto);
            var result=await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return Ok(_mapper.Map<AddressDto>(user.address));
            return BadRequest(new ApiResponse( 400, "problem in update"));
        }
    }


}
