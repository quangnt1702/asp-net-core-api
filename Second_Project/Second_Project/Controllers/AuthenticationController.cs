using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Second_Project.RequestModels;
using Second_Project.Services;

namespace Second_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenService _authenticationService;

        public AuthenticationController(IAuthenService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public ActionResult Login(LoginUserRequestModel loginUserRequestModel)
        {
            return Ok(_authenticationService.Authenticate(loginUserRequestModel));
        }

        [HttpGet("getToken")]
        public ActionResult GetToken()
        {
            var accessToken = HttpContext.Items["UserName"];
            if (accessToken != null) return Ok(accessToken.ToString());
            return NotFound();
        }

        [HttpGet("encodeToken")]
        public ActionResult EncodeToken()
        {
            var accessToken = HttpContext.Items["accessToken"];
            if (accessToken != null)
            {
                return Ok(_authenticationService.EncodeToken(accessToken.ToString()));
            }

            return NotFound();
        }
    }
}