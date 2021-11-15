using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using STOCMA.Data;
using STOCMA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace STOCMA.Auth
{
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(ApplicationDbContext db, UserManager<ApplicationUser> _userManager)
        {
            this.db = db;
            this.userManager = _userManager;
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(ApplicationUser user)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            //var userManager = new UserManager<ApplicationUser>(userStore);
            var applicationUser = await userManager.FindByIdAsync(user.Id);

            applicationUser.UserName = user.UserName;
            applicationUser.Email = user.UserName;

            await userManager.UpdateAsync(applicationUser);

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> UpdatePassword(string userId, string newPassword)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var applicationUser = await userManager.FindByIdAsync(userId);
            await userManager.RemovePasswordAsync(applicationUser);
            await userManager.AddPasswordAsync(applicationUser, newPassword);

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> SetClaim(string userId, string claim, bool enabled)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var applicationUser = await userManager.FindByIdAsync(userId);
            var userClaim = (await userManager.GetClaimsAsync(applicationUser)).FirstOrDefault(x => x.Type == claim);
            var userHasClaim = userClaim != null;
            if (enabled && !userHasClaim)
                await userManager.AddClaimAsync(applicationUser, new Claim(claim, "true"));
            else if (userHasClaim)
                await userManager.RemoveClaimAsync(applicationUser, userClaim);

            userHasClaim = (await userManager.GetClaimsAsync(applicationUser)).FirstOrDefault(x => x.Type == claim) != null;

            await userManager.UpdateSecurityStampAsync(applicationUser);

            return Ok(new { userHasClaim });
        }

        [HttpPost]
        public ActionResult HasClaim(string userId, string claim)
        {
            return Ok(new { userHasClaim = new AuthManager(db, userManager).UserHasClaim(userId, claim) });
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(string username, string password)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            ApplicationUser user = new ApplicationUser();
            user.Id = Guid.NewGuid().ToString();
            user.Email = username;
            user.UserName = username;
            var result = await userManager.CreateAsync(user, password);


            if (result.Succeeded)
                return Ok(new { user });
            else
                return BadRequest();

        }

        [HttpDelete]
        public async Task<ActionResult> RemoveUser(string userId)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var user = await userManager.FindByIdAsync(userId);
            await userManager.DeleteAsync(user);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> GetCurrentUserClaims()
        {
            var userId = userManager.GetUserId(User);
            var currentUser = await userManager.FindByIdAsync(userId);

            var claims = User.Claims.Select(x => x.Value);
            var isAdmin = User.HasClaim(ClaimTypes.Role, "Admin");
            var username = currentUser.UserName;

            return Ok(new { username, isAdmin, claims = claims });
        }
    }
}