using LmycWeb.Helper;
using LmycWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LmycWebsite.Helper
{
    public class Helper
    {
        public static string ConvertIdToName(string id)
        {
            IServiceProvider _services = ServiceProviderProvider.Services;
            var um = _services.GetRequiredService<UserManager<ApplicationUser>>();
            var user = um.FindByIdAsync(id).Result;
            var username = user.UserName;

            return username;
        }
    }
}