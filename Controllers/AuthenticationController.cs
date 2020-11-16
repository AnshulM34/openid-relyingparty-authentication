using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace openid.relyingparty.authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [Authorize(AuthenticationSchemes = "unknown-authentication-scheme")]
        public async Task<ActionResult> Get()
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                return new StatusCodeResult(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
        }


        [HttpGet]
        [Route("/SignOut")]
        public async Task<ActionResult> ClearSession()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignOutAsync("unknown-authentication-scheme");
                return new StatusCodeResult(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("/Authenticated")]
        public ActionResult IsAuthenticated()
        {
            try
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                    return new StatusCodeResult(StatusCodes.Status200OK); ;
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
