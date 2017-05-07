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

namespace TemplateAuth.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/Admin")]
    public class AdminController : ApiController
    {
        [HttpGet]
        [Route("Users")]
        [HostAuthentication(DefaultAuthenticationTypes.ApplicationCookie)]
        public Task<string> getUsers()
        {
            List<UserTableModel> userTableModelList = new List<UserTableModel>();
            using (ApplicationDbContext context = Request.GetOwinContext().Get<ApplicationDbContext>())
            {
                List<ApplicationUser> users = context.Users.ToList();

                foreach (ApplicationUser user in users)
                {
                    UserTableModel userModel = new UserTableModel();
                    userModel.Email = user.Email;
                    userModel.FirstName = "a";
                    userModel.LastName = "b";
                    userModel.MiddleName = "c";
                    userTableModelList.Add(userModel);
                }

            }
            return JsonConvert.SerializeObjectAsync(userTableModelList);
        }
    }
}
