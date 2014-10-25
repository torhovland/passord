using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Mvc;

namespace ClaimsTodoApp.Controllers
{
    [System.Web.Http.Authorize]
    public class ClaimsController : Controller
    {
        public ActionResult Index()
        {
            var identity = User.Identity as ClaimsIdentity;
            return View(identity.Claims);
        }
    }
}
