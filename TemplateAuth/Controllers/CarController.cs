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
using TemplateAuth.Models.Vehicle;

namespace TemplateAuth.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    [RoutePrefix("api/Car")]
    public class CarController : ApiController
    {
        [HttpGet]
        [Route("AllMarks")]
        public async Task<IHttpActionResult> getAllMarks()
        {
            var context = Request.GetOwinContext().Get<ApplicationDbContext>();
            if (context == null)
                return StatusCode(HttpStatusCode.InternalServerError);
            var list = context.CarMarks.ToList();
            return Ok(list);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IHttpActionResult> getAllCars()
        {
            var context = Request.GetOwinContext().Get<ApplicationDbContext>();
            if (context == null)
                return StatusCode(HttpStatusCode.InternalServerError);
            try
            {
                var list = new List<CarCM>();
                foreach (Car item in context.Cars)
                {
                    CarCM car = new CarCM();
                    car.CarMark = item.CarModel.CarMark.Mark;
                    car.CarModel = item.CarModel.Name;
                    car.CarColor = item.Color;
                    car.Id = item.Id;
                    list.Add(car);
                }                
                return Ok(list);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("GetByModel")]
        public async Task<IHttpActionResult> getMarkModels(Dictionary<string, string> param)
        {
            var markId = param["mark"];
            if (markId == null || markId.Length == 0)
                return StatusCode(HttpStatusCode.BadRequest);
            var context = Request.GetOwinContext().Get<ApplicationDbContext>();
            if (context == null)
                return StatusCode(HttpStatusCode.InternalServerError);
            try
            {
                var modelList = context.CarModels.Where(u => u.CarMark.Id.Equals(markId)).ToList();
                List<string> result = modelList.Select(u => u.Name).ToList();
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create(CarCM model)
        {
            var context = Request.GetOwinContext().Get<ApplicationDbContext>();
            if (context == null)
                return StatusCode(HttpStatusCode.InternalServerError);
            try
            {
                var carMark = context.CarMarks.FirstOrDefault(u => u.Mark.Equals(model.CarMark));
                if (carMark == null)
                {
                    carMark = context.CarMarks.Create();
                    carMark.Mark = model.CarMark;
                    context.Entry(carMark).State = System.Data.Entity.EntityState.Added;
                }
                var carModel = context.CarModels
                    .FirstOrDefault(u => u.CarMark.Mark.Equals(model.CarMark) 
                    && model.CarModel.Equals(model.CarModel));
                if (carModel == null)
                {
                    carModel = context.CarModels.Create();
                    carModel.CarMark = carMark;
                    carModel.Name = model.CarModel;
                    context.Entry(carModel).State = System.Data.Entity.EntityState.Added;
                }
                var car = context.Cars.Create();
                car.Color = model.CarColor;
                car.CarModel = carModel;
                car.Id = Guid.NewGuid();
                context.Entry(car).State = System.Data.Entity.EntityState.Added;
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
