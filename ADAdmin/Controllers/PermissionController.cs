using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace ADAdmin.Controllers
{
    //[Authorize(Roles = @"kglsa\PRO ADAdmin FrontEnd")]
    [Authorize(Roles = @"BUILTIN\Administratorzy")]
    public class PermissionController : ControllerBase
    {
        [HttpGet]
        [Route("Permission/Check")]
        public ActionResult Check()
        {
            return new StatusCodeResult(200);
        }

        [HttpGet]
        [Route("Permission/Identity")]
        public string Identity()
        {
            return JsonConvert.SerializeObject(User.Identity.Name);
        }
    }
}