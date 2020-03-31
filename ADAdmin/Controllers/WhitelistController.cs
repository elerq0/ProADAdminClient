using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADAdmin.Controllers
{
    [Authorize(Roles = @"kglsa\PRO ADAdmin FrontEnd")]
    //[Authorize(Roles = @"BUILTIN\Administratorzy")]
    public class WhitelistController : ControllerBase
    {
        Models.Whitelist _whitelist;
        public WhitelistController(Models.Whitelist whitelist)
        {
            _whitelist = whitelist;
        }

        [HttpGet]
        [Route("Whitelist/DomainList")]
        public string DomainList()
        {
            return _whitelist.GetDomainList(User.Identity.Name);
        }

        [HttpGet]
        [Route("Whitelist/AddressList")]
        public string AddressList()
        {
            return _whitelist.GetAddressList(User.Identity.Name);
        }

        [HttpPost]
        [Route("Whitelist/DomainAdd")]
        public async Task<ActionResult> DomainAdd(string path)
        {

            Boolean result = await _whitelist.DomainAdd(User.Identity.Name, path);

            if (result)
                return new StatusCodeResult(200);
            else
                return new StatusCodeResult(422);
        }

        [HttpPost]
        [Route("Whitelist/AddressAdd")]
        public async Task<ActionResult> AddressAdd(string path)
        {

            Boolean result = await _whitelist.AddressAdd(User.Identity.Name, path);

            if (result)
                return new StatusCodeResult(200);
            else
                return new StatusCodeResult(422);
        }

        [HttpPost]
        [Route("Whitelist/DomainRemove")]
        public async Task<ActionResult> DomainRemove(string path)
        {

            Boolean result = await _whitelist.DomainRemove(User.Identity.Name, path);

            if (result)
                return new StatusCodeResult(200);
            else
                return new StatusCodeResult(422);
        }

        [HttpPost]
        [Route("Whitelist/AddressRemove")]
        public async Task<ActionResult> AddressRemove(string path)
        {

            Boolean result = await _whitelist.AddressRemove(User.Identity.Name, path);

            if (result)
                return new StatusCodeResult(200);
            else
                return new StatusCodeResult(422);
        }
    }
}