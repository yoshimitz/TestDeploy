using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LmycWeb.Data;
using LmycWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LmycWeb.Controllers
{
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _services;

        public RolesController(ApplicationDbContext context, IServiceProvider services)
        {
            _context = context;
            _services = services;
        }

        public ActionResult Index()
        {
            var roles = _context.Roles.ToList();
            return View(roles);
        }

        // GET: /Roles/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(IFormCollection collection)
        {
            string name = collection["RoleName"];

            if (!string.IsNullOrEmpty(name))
            {
                try
                {
                    var role = _context.Roles.Where(r => r.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    if (role == null)
                    {
                        _context.Roles.Add(new IdentityRole()
                        {
                            Name = name,
                            NormalizedName = name.ToUpper()
                        });
                        _context.SaveChangesAsync();
                        ViewBag.ResultMessage = "Role created successfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ResultMessage = "Role already exists";

                        return View();
                    }
                }
                catch
                {
                    ViewBag.ResultMessage = "Role already exists";

                    return View();
                }
            }
            else
            {
                ViewBag.ResultMessage = "Please enter a role name";
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string RoleName)
        {
            var thisRole = _context.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            _context.Roles.Remove(thisRole);
            _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //
        // GET: /Roles/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string roleName)
        {
            var thisRole = _context.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            return View(thisRole);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(IdentityRole role, string roleName)
        {
            if (!string.IsNullOrEmpty(role.Name))
            {
                var roleToUpdate = _context.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                var results = _context.Roles.Where(r => r.Name.Equals(role.Name, StringComparison.CurrentCultureIgnoreCase));
                int count = results.ToList().Count;

                //var roleToUpdate = await _context.Roles.SingleOrDefaultAsync(r => r.Name == role.Name);
                if (await TryUpdateModelAsync<IdentityRole>(
                    roleToUpdate,
                    "",
                    r => r.Name, r => r.NormalizedName, r => r.ConcurrencyStamp))
                {
                    try
                    {
                        //_context.Roles.Update(role); 

                        //_context.Entry(role).State = EntityState.Modified;
                        if (count == 0)
                        {
                            await _context.SaveChangesAsync();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.ResultMessage = "Role already exists";
                            return View();
                        }

                    }
                    catch
                    {
                        ViewBag.ResultMessage = "Role already exists";

                        return View();
                    }
                }
                else
                {
                    ViewBag.ResultMessage = "Please enter a role name";
                    return View();
                }
            }
            else
            {
                ViewBag.ResultMessage = "Please enter a role name";
                return View();
            }

        }

        [Authorize(Roles = "Admin")]
        public ActionResult ManageUserRoles()
        {
            // prepopulate roles for the view dropdown
            var userList = _context.Users.OrderBy(us => us.UserName).ToList().Select(uu => new SelectListItem
            {
                Value = uu.UserName.ToString(),
                Text = uu.UserName
            }).ToList();
            var list = _context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem
            {
                Value = rr.Name.ToString(),
                Text = rr.Name
            }).ToList();

            ViewBag.Users = userList;
            ViewBag.Roles = list;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            ApplicationUser user = _context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            var um = _services.GetRequiredService<UserManager<ApplicationUser>>();
            //var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            if (user != null && !String.IsNullOrEmpty(RoleName))
            {
                var idResult = um.AddToRoleAsync(user, RoleName);
                
                if (idResult.Result.Succeeded)
                {
                    ViewBag.AddResultMessageSuccess = "Role successfully added to User";
                }
                else
                {
                    ViewBag.AddResultMessageError = "Role already exists for User";
                }
            }
            else
            {
                ViewBag.AddResultMessageError = "Please select a Username and a Role";
            }

            // prepopulate roles for the view dropdown
            var userList = _context.Users.OrderBy(us => us.UserName).ToList().Select(uu => new SelectListItem
            {
                Value = uu.UserName.ToString(),
                Text = uu.UserName
            }).ToList();
            var list = _context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem
            {
                Value = rr.Name.ToString(),
                Text = rr.Name
            }).ToList();

            ViewBag.Users = userList;
            ViewBag.Roles = list;

            return View("ManageUserRoles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            ApplicationUser user = _context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            var um = _services.GetRequiredService<UserManager<ApplicationUser>>();

            if (String.IsNullOrEmpty(UserName))
            {
                ViewBag.Message = "Please select a User.";
            }
            else if (user != null)
            {
                var rolesList = um.GetRolesAsync(user);

                ViewBag.RolesForThisUser = rolesList.Result;
            }

            // prepopulate roles for the view dropdown
            var userList = _context.Users.OrderBy(us => us.UserName).ToList().Select(uu => new SelectListItem
            {
                Value = uu.UserName.ToString(),
                Text = uu.UserName
            }).ToList();
            var list = _context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem
            {
                Value = rr.Name.ToString(),
                Text = rr.Name
            }).ToList();

            ViewBag.Users = userList;
            ViewBag.Roles = list;

            return View("ManageUserRoles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRoleForUser(string UserName, string RoleName)
        {
            ApplicationUser user = _context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            var um = _services.GetRequiredService<UserManager<ApplicationUser>>();
            bool isIn = await um.IsInRoleAsync(user, RoleName);

            if (String.IsNullOrEmpty(UserName) || String.IsNullOrEmpty(RoleName))
            {
                ViewBag.DeleteResultMessageError = "Please select a User and a Role";
            }
            else if (isIn && !user.UserName.Equals("a"))
            {
                await um.RemoveFromRoleAsync(user, RoleName);
                ViewBag.DeleteResultMessageSuccess = "Role removed from this user successfully";
            }
            else if (isIn && user.UserName.Equals("a") && !RoleName.Equals("Admin"))
            {
                await um.RemoveFromRoleAsync(user, RoleName);
                ViewBag.DeleteResultMessageSuccess = "Role removed from this user successfully";
            }
            else if (user.UserName.Equals("a") && RoleName.Equals("Admin"))
            {
                ViewBag.DeleteResultMessageError = "Cannot delete this Role from this User";
            }
            else
            {
                ViewBag.DeleteResultMessageError = "This User doesn't belong to selected Role";
            }
            // prepopulate roles for the view dropdown
            var userList = _context.Users.OrderBy(us => us.UserName).ToList().Select(uu => new SelectListItem
            {
                Value = uu.UserName.ToString(),
                Text = uu.UserName
            }).ToList();
            var list = _context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem
            {
                Value = rr.Name.ToString(),
                Text = rr.Name
            }).ToList();

            ViewBag.Users = userList;
            ViewBag.Roles = list;

            return View("ManageUserRoles");
        }
    }
}