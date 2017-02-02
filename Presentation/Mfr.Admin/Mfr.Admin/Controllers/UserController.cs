using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Kendo.Mvc.UI;
using Mfr.Admin.Models;
using Mfr.Admin.Models.Country;
using Mfr.Admin.Models.User;
using Mfr.Core.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PagedList;

namespace Mfr.Admin.Controllers
{
    public class UserController : Controller
    {

        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _role;
     

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? 
                    HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        public ApplicationRoleManager UserRole
        {
            get
            {
                return _role ??
                    HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _role = value;
            }
        }

        public ActionResult EditCredentials(int id)
        {
            var user = UserManager.FindById(id);
            var userViewModel = new UserViewModel()//Load User Credentials
            {
                UserName = user.UserName,
                FName = user.FirstName,
                LName = user.LastName,
                BirthDate = user.BirthDate,
                Phonenumber = user.PhoneNumber,
            };
            var allroles = UserRole.Roles;
            var roles = user.Roles;
            var userRoleViewModels = new List<UserRoleViewModel>();
            foreach (var item in allroles)// Load  All Roles
            { 
                var newViewModel=new UserRoleViewModel
                {
                    Show = item.Show,
                    RoleId = item.Id,
                    BaseCoding = item.BaseCoding,
                    IsActive = item.IsActive,
                    RoleName = item.Name,
                    UserId=id
                };
                  
                    foreach (var role in roles.Where(role => role.RoleId==item.Id))
                    {  //if new Role Exist In userRole table, then apply is checked
                        newViewModel.ApplyPrem = true;
                        newViewModel.UserId = role.UserId;
                    
                    }
                    userRoleViewModels.Add(newViewModel);
            }
            var userMainViewModel = new UserMainViewModel()
            {
                // user main view model,  include two model: one "UserViewModel" and a list of "UserRoleViewModel"
                UserViewModel = userViewModel,
                UserRoleViewModel = userRoleViewModels,
                UserPasswordViewModel=new UserPasswordViewModel {UserId=id}
            };

            return View(userMainViewModel);
        }
        [HttpPost]
        public async Task<ActionResult> _UserPermissions(IList<UserRoleViewModel> model)
        {
                if (model.Count(item => item.ApplyPrem) != 0)
                {
                    var store = new UserStore<ApplicationUser, ApplicationRole, int
                        , ApplicationUserLogin, ApplicationUserRole,
                        ApplicationUserClaim>(new ApplicationDbContext());

                    var manager = new UserManager<ApplicationUser,
                        int>(store);
                    var rolesForUser = await UserManager.GetRolesAsync(model.ElementAt(0).UserId);

                    foreach (var item in rolesForUser)
                    {
                        //delete All Roles For The Selected User
                        await UserManager.RemoveFromRoleAsync(model.ElementAt(0).UserId, item);

                    }
                    foreach (var item in model.Where(item => item.ApplyPrem))
                    {
                        //If ApplyPerm Is Checked, then Add it to User Roles
                        manager.AddToRole(item.UserId, item.RoleName);
                    }
                return RedirectToAction("list");
            }
            ModelState.AddModelError("Perm", "At least one Premission should be slected");
            return View(model);


        }

        [HttpPost]
        public async Task<ActionResult> EditCredentials(UserViewModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.Id);
                user.BirthDate = model.BirthDate;
                user.FirstName = model.FName;
                user.LastName = model.LName;
                user.PhoneNumber = model.Phonenumber;
                user.UserName = model.UserName;

                if (file != null)
                {
                    try
                    {
                        string fileName;
                        if (user.AvatarUrl != null)
                        {
                            //Delete Last Pic If It Exist
                            System.IO.File
                                .Delete(Path.Combine(Server.MapPath("~/App_Themes/Images"), 
                                user.AvatarUrl));
                            fileName = user.AvatarUrl;
                        }
                        else
                            fileName = Guid.NewGuid().ToString()+".jpg";

                        var path = Path.Combine(Server.MapPath("~/App_Themes/Images"),
                            fileName);
                        file.SaveAs(path);
                        user.AvatarUrl = fileName;
                    }
                    catch
                    {

                        ModelState.AddModelError("Picture", 
                            "There was an error uploading the image");
                    }
                }
                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("list");
        }

        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserList(DataSourceRequest command)
        {
            var users = UserManager.Users;
            var usersList = users as IList<ApplicationUser> ?? users.ToList();
            var userModel = new PagedList<ApplicationUser>(usersList.ToList(),
                command.Page, command.PageSize);

            var usersViewModels = userModel.Select(item => new UserViewModel
            {
                Id = item.Id,
                FName = item.FirstName,
                LName = item.LastName,
                IsActive = item.IsActive,
                AvatarUrl = item.AvatarUrl,
                BirthDate = item.BirthDate,
                UserName = item.UserName
            }).Where(usersViewModel => usersViewModel.Id !=
                        User.Identity.GetUserId<int>()).ToList();//the User Cannot Edit Himself!

            var dataSourceResult = new DataSourceResult()
            {
                Data = usersViewModels,
                Total = userModel.TotalItemCount
            };

            return Json(dataSourceResult);
        }

        public ActionResult _PasswordEdit(UserPasswordViewModel model)
        {
            if (model == null)
                throw new ArgumentException("bad request");

                UserManager.RemovePassword(model.UserId);
                var result = UserManager.AddPassword(model.UserId, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("list");
            }

            AddErrors(result);
            return RedirectToAction("EditCredentials", new {@id=model.UserId});
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("PasswordChange", error);
            }
        }


    }
}