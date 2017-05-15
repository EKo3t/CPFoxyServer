using Microsoft.AspNet.Identity;
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
        [HostAuthentication(DefaultAuthenticationTypes.ApplicationCookie)]
        public async Task<IHttpActionResult> CreateDriver(Dictionary<string, string> param)
        {
            try
            {
                string email = param["email"];
                Guid id = Guid.Parse(param["car"]);
                if (email == null)
                    return StatusCode(HttpStatusCode.BadRequest);
                var context = Request.GetOwinContext().Get<ApplicationDbContext>();
                var user = context.Users.FirstOrDefault(u => u.Email.Equals(email));
                if (user == null)
                    return StatusCode(HttpStatusCode.BadRequest);
                Driver driver = context.Drivers.Create();
                var car = context.Cars.FirstOrDefault(u => u.Id.Equals(id));
                driver.Car = car;
                driver.Id = Guid.NewGuid();
                driver.User = user;
                context.Drivers.Add(driver);
                context.Entry(driver).State = System.Data.Entity.EntityState.Added;
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
        [HostAuthentication(DefaultAuthenticationTypes.ApplicationCookie)]
        public async Task<IHttpActionResult> ReturnList()
        {
            try
            {
                var context = Request.GetOwinContext().Get<ApplicationDbContext>();
                var drivers = context.Drivers.ToList();
                var result = new List<DriverCM>();
                foreach (Driver driver in drivers)
                {
                    DriverCM driverCM = new DriverCM();
                    var userToInfo = context.UserToInfoes.FirstOrDefault(u => u.UserId.Equals(driver.UserId));
                    var userInfo = userToInfo == null ? null : userToInfo.UserInfo ;
                    driverCM.UserDetails = new UserInfoCM(userInfo);
                    var car = context.Cars.FirstOrDefault();
                    CarCM carCM = new CarCM();
                    carCM.Id = car.Id;
                    carCM.CarMark = car.CarModel.CarMark.Mark;
                    carCM.CarModel = car.CarModel.Name;
                    carCM.CarColor = car.Color;
                    driverCM.Car = carCM;
                    result.Add(driverCM);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [Route("Busy")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IHttpActionResult> HireFirst(Dictionary<string, string> orderIdDict)
        {
            try
            {
                Guid orderId = Guid.Parse(orderIdDict["orderId"]);
                var context = Request.GetOwinContext().Get<ApplicationDbContext>();
                var freeDriver = context.Drivers.FirstOrDefault(u => u.Order == null);
                if (freeDriver == null)
                {
                    HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    responseMsg.ReasonPhrase = "Нет свободных водителей";
                    return ResponseMessage(responseMsg);
                }
                var order = context.Orders.FirstOrDefault(u => u.Id.Equals(orderId));
                freeDriver.Order = order;
                DriverCM driverCM = new DriverCM();
                var userToInfo = context.UserToInfoes.FirstOrDefault(u => u.UserId.Equals(freeDriver.UserId));
                var userInfo = userToInfo == null ? null : userToInfo.UserInfo;
                driverCM.UserDetails = new UserInfoCM(userInfo);
                driverCM.Car = new CarCM();
                driverCM.Car.CarMark = freeDriver.Car.CarModel.CarMark.Mark;
                driverCM.Car.CarModel = freeDriver.Car.CarModel.Name;
                driverCM.Car.CarColor = freeDriver.Car.Color;
                context.Entry(freeDriver).State = System.Data.Entity.EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(driverCM);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }
    }
}
