using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TemplateAuth.Models;

namespace TemplateAuth.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    [RoutePrefix("api/Status")]
    public class StatusController : ApiController
    {
        [HttpGet]
        [Route("GetAll")]
        public async Task<IHttpActionResult> GetAll()
        {
            var context = Request.GetOwinContext().Get<ApplicationDbContext>();
            if (context == null)
                return StatusCode(HttpStatusCode.InternalServerError);
            return Ok(context.Statuses);
        }
    }
}
