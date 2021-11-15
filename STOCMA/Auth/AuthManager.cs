using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using STOCMA.Data;
using STOCMA.Models;

namespace STOCMA.Auth
{
    public class AuthManager
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public AuthManager(ApplicationDbContext db, UserManager<ApplicationUser> _userManager)
        {
            this.db = db;
            this.userManager = _userManager;
        }

        public async Task<bool> UserHasClaim(string userId, string claim)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var applicationUser = await userManager.FindByIdAsync(userId);
            var userClaim = (await userManager.GetClaimsAsync(applicationUser)).FirstOrDefault(x => x.Type == claim);
            var userHasClaim = userClaim != null;

            return userHasClaim;
        }
    }
}