using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TemplateAuth.ClientModels;
using TemplateAuth.Models;
using TemplateAuth.Models.Vehicle;

namespace TemplateAuth.Controllers
{
    [RoutePrefix("api/Driver")]
    public class DriverController : ApiController
    {
        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IHttpActionResult> CreateDriver(Dictionary<string, string> param)
        {
            try
            {
                string email = param["email"];
                int id = JsonConvert.DeserializeObject<int>(param["car"]);
                if (email == null)
                    return StatusCode(HttpStatusCode.BadRequest);
                var context = Request.GetOwinContext().Get<ApplicationDbContext>();
                var user = context.Users.FirstOrDefault(u => u.Email.Equals(email));
                if (user == null)
                    return StatusCode(HttpStatusCode.BadRequest);
                Driver driver = context.Drivers.Create();
                var car = context.Cars.FirstOrDefault(u => u.Id.Equals(id));
                driver.Car = car;
                context.Drivers.Add(driver);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("List")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IHttpActionResult> ReturnList()
        {
            try
            {
                var context = Request.GetOwinContext().Get<ApplicationDbContext>();
                var drivers = context.Drivers.ToList();
                foreach (Driver driver in drivers)
                {
                    DriverCM driverCM = new DriverCM();
                    var userToInfo = context.UserToInfoes.FirstOrDefault(u => u.UserId.Equals(driver.UserId));
                    var userInfo = userToInfo == null ? null : userToInfo.UserInfo ;
                    driverCM.FirstName = userInfo == null ? null : userInfo.FirstName;
                    driverCM.LastName = userInfo == null ? null : userInfo.LastName;
                    driverCM.MiddleName = userInfo == null ? null : userInfo.MiddleName;
                    var car = context.Cars.FirstOrDefault();
                    driverCM.CarMark = car.CarModel.CarMark.Mark;
                    driverCM.CarModel = car.CarModel.Name;
                    driverCM.Color = car.Color;
                }
                return Ok(drivers);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
        }
    }
}
