using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADAdmin.Controllers
{
    //[Authorize(Roles = @"kglsa\PRO ADAdmin FrontEnd")]
    [Authorize(Roles = @"BUILTIN\Administratorzy")]
    public class UsersController : ControllerBase
    {
        Models.Users _users;
        public UsersController(Models.Users users)
        {
            _users = users;
        }

        [HttpGet]
        [Route("Users/List")]
        public string List()
        {

            return _users.GetUsersList(User.Identity.Name);
        }

        [HttpPost]
        [Route("Users/Unlock")]
        public async Task<ActionResult> Unlock(string username)
        {
            
            Boolean result = await _users.UnlockAccount(User.Identity.Name, username);

            if (result)
                return new StatusCodeResult(200);
            else
                return new StatusCodeResult(422);
        }

        [HttpPost]
        [Route("Users/ResetPassword")]
        public async Task<ActionResult> ResetPassword(string username, string password)
        {

            Boolean result = await _users.ResetPassword(User.Identity.Name, username, password);

            if (result)
                return new StatusCodeResult(200);
            else
                return new StatusCodeResult(422);
        }

        [HttpPost]
        [Route("Users/ResetSession")]
        public async Task<ActionResult> ResetSession(string username)
        {

            Boolean result = await _users.ResetSession(User.Identity.Name, username);

            if (result)
                return new StatusCodeResult(200);
            else
                return new StatusCodeResult(422);
        }

        [HttpGet]
        [Route("Users/GetStatus")]
        public async Task<string> GetStatus(string username)
        {
            return await _users.GetStatus(User.Identity.Name, username);
        }
    }
}