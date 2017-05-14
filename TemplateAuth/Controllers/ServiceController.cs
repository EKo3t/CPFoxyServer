using Microsoft.AspNet.Identity;
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
    [Authorize]
    [RoutePrefix("api/Service")]
    public class ServiceController : ApiController
    {
        [HttpGet]
        [Route("Get")]
        [HostAuthentication(DefaultAuthenticationTypes.ApplicationCookie)]
        public async Task<IHttpActionResult> GetAll()
        {
            var context = Request.GetOwinContext().Get<ApplicationDbContext>();
            if (context == null)
                return StatusCode(HttpStatusCode.InternalServerError);
            var list = context.Services.ToList();            
            return Ok(list);
        }

        [HttpPost]
        [Route("Create")]
        [HostAuthentication(DefaultAuthenticationTypes.ApplicationCookie)]
        public async Task<IHttpActionResult> Create(Service service)
        {
            var context = Request.GetOwinContext().Get<ApplicationDbContext>();
            if (context == null)
                return StatusCode(HttpStatusCode.InternalServerError);
            try
            {
                service.Id = Guid.NewGuid();
                context.Services.Add(service);
                context.Entry(service).State = System.Data.Entity.EntityState.Added;
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }
    }
}
