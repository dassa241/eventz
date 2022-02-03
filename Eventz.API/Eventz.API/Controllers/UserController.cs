using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eventz.BLL.ViewModels;
using Eventz.DAL.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Eventz.API.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController : ControllerBase
    {

        private readonly IUser _user;
        private IHttpContextAccessor _accessor;
        private readonly IConfiguration _config;


        public UserController(IUser user, IHttpContextAccessor accessor, IConfiguration config)
        {
            _user = user;
            _accessor = accessor;
            _config = config;

        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            var userObj = _user.GetUser(user.Username, user.Password);
            if (userObj!=null)
            {
                var Token = _user.GenerateJSONWebToken(userObj);
                return Ok(new { Token, userObj.UserIdx, userObj.FirstName, userObj.LastName });
            }
            return BadRequest(new { Msg = "Invalid username or password" });
         
        }
    }
}
