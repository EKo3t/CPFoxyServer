using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TemplateAuth.ClientModels;
using TemplateAuth.Models;

namespace TemplateAuth.Controllers
{
    [System.Web.Http.Authorize]
    [System.Web.Http.RoutePrefix("api/Order")]
    public class OrderController : ApiController
    {
        [HttpPost]        
        [Route("Create")]
        [HostAuthentication(DefaultAuthenticationTypes.ApplicationCookie)]
        public async Task<IHttpActionResult> createOrder(OrderCM model)
        {
            var userManager = Request.GetOwinContext().Get<ApplicationUserManager>();
            ApplicationUser user = await userManager.FindByIdAsync(User.Identity.GetUserId());
            var context = Request.GetOwinContext().Get<ApplicationDbContext>();
            if (model == null)
                return StatusCode(HttpStatusCode.BadRequest);
            if (model.OrderTime < DateTime.Now)
                return StatusCode(HttpStatusCode.BadRequest);
            Order order = context.Orders.Create();
            order.orderTime = model.OrderTime;
            order.StartAddress = model.StartAddress;
            order.EndAddress = model.EndAddress;
            order.UserId = user.Id;
            order.User = user;
            Status status = context.Statuses.FirstOrDefault(u => u.Name.Equals("Created"));
            order.Status = status;
            order.StatusId = status.Id;
            try
            {
                context.Orders.Add(order);
                context.Entry(order).State = System.Data.Entity.EntityState.Added;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            return Ok();
        }

        [HttpGet]
        [Route("List")]
        [HostAuthentication(DefaultAuthenticationTypes.ApplicationCookie)]
        public async Task<IHttpActionResult> GetList()
        {
            var userManager = Request.GetOwinContext().Get<ApplicationUserManager>();
            var context = Request.GetOwinContext().Get<ApplicationDbContext>();
            if (context == null || userManager == null)
                return StatusCode(HttpStatusCode.InternalServerError);
            string userId = User.Identity.GetUserId();
            List<Order> orders = new List<Order>();
            foreach (Order order in context.Orders)
            {
                if (order.UserId != null && order.UserId.Equals(userId))
                    orders.Add(order);
            }
            return Ok(orders);
        }
    }
}
