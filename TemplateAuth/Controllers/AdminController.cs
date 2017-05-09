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
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace TemplateAuth.Controllers
{
    [System.Web.Http.Authorize(Roles = "Admin")]
    [System.Web.Http.RoutePrefix("api/Admin")]
    public class AdminController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("Users")]
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
                    var userInfo = context.UserInfoes.FirstOrDefault(u => u.UserId.Equals(user.Id));
                    userModel.Email = user.Email;
                    userModel.FirstName = userInfo == null ? "" : userInfo.FirstName;
                    userModel.LastName = userInfo == null ? "" : userInfo.LastName;
                    userModel.MiddleName = userInfo == null ? "" : userInfo.MiddleName;
                    userTableModelList.Add(userModel);
                }
            }
            return JsonConvert.SerializeObjectAsync(userTableModelList);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Delete")]
        [HostAuthentication(DefaultAuthenticationTypes.ApplicationCookie)]
        public async Task<IHttpActionResult> DeleteConfirmed(Dictionary<string, string> email)
        {
            var context = Request.GetOwinContext().Get<ApplicationDbContext>();
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = Request.GetOwinContext().Get<ApplicationRoleManager>();
            if (email == null || email["email"] == null)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = null;
            foreach (ApplicationUser loopUser in context.Users.ToList()) 
            {
                if (loopUser.Email.Equals(email["email"]))
                {
                    user = loopUser;
                    break;
                }
            }
            var rolesForUser = await userManager.GetRolesAsync(user.Id);
            if (rolesForUser.Count() > 0)
            {
                foreach (var item in rolesForUser)
                {
                    var result = await userManager.RemoveFromRolesAsync(user.Id);
                }
            }
            try
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(HttpStatusCode.BadRequest);
            }
            return Ok();
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Create")]
        [HostAuthentication(DefaultAuthenticationTypes.ApplicationCookie)]
        public async Task<IHttpActionResult> CreateUser(UserCreateModel model)
        {
            var context = Request.GetOwinContext().Get<ApplicationDbContext>();
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = Request.GetOwinContext().Get<ApplicationRoleManager>();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                UserInfo userInfo = new UserInfo();
                userInfo.FirstName = model.FirstName;
                userInfo.LastName = model.LastName;
                userInfo.MiddleName = model.MiddleName;
                userInfo.BirthDate = model.BirthDate;
                userInfo.User = user;
                userInfo.UserId = user.Id;
                try
                {
                    context.UserInfoes.Add(userInfo);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return StatusCode(HttpStatusCode.BadRequest);
                }
                return Ok();
            }

            return StatusCode(HttpStatusCode.BadRequest);
        }
    }
}
