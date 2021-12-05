using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using STOCMA.Data;
using STOCMA.Models;

namespace STOCMA.Controllers
{
    [Authorize]
    public class ApplicationUsersController : ODataController
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ApplicationUsersController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        //var usersWithClaims = userManager.Users.ToList().Select(async x => new { x, claims = await userManager.GetClaimsAsync(x) });
        [EnableQuery]
        public IQueryable<CustomApplicationUser> GetApplicationUsers()
        {
            var usersWithClaims = userManager.Users.ToList().Select(x => new
           CustomApplicationUser
            {
                Id = x.Id,
                UserName = x.UserName,
                Claims = userManager.GetClaimsAsync(x).Result.ToList()
            }).AsQueryable();

            return usersWithClaims;
        }

        [EnableQuery]
        public SingleResult<ApplicationUser> GetApplicationUser([FromODataUri] string key)
        {
            return SingleResult.Create<ApplicationUser>(this.userManager.Users.Where<ApplicationUser>((Expression<Func<ApplicationUser, bool>>)(applicationUser => applicationUser.Id == key)));
        }

        public async Task<IActionResult> Put([FromODataUri] string key, string UserName)
        {
            //var userStore = new UserStore<ApplicationUser>(userManager.Users);
            //var userManager = new UserManager<ApplicationUser>(userStore);
            //var applicationUser = userManager.FindById(key);
            var applicationUser = await userManager.FindByIdAsync(key);

            if (applicationUser == null)
                return NotFound();

            applicationUser.UserName = UserName;


            try
            {
                await userManager.UpdateAsync(applicationUser);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.ApplicationUserExists(new Guid(key)))
                    return NotFound();
                throw;
            }
            return (IActionResult)this.Updated<ApplicationUser>(applicationUser);
        }

        public async Task<IActionResult> Post([FromBody] ApplicationUser applicationUser)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            try
            {
                await this.userManager.CreateAsync(applicationUser);
            }
            catch (DbUpdateException ex)
            {
                if (this.ApplicationUserExists(new Guid(applicationUser.Id)))
                    return (IActionResult)this.Conflict();
                throw;
            }
            return (IActionResult)this.Created<ApplicationUser>(applicationUser);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<ApplicationUser> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            ApplicationUser applicationUser = await this.userManager.FindByIdAsync(key.ToString());
            if (applicationUser == null)
                return (IActionResult)this.NotFound();
            patch.Patch(applicationUser);
            try
            {
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.ApplicationUserExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<ApplicationUser>(applicationUser);
        }

        public async Task<IActionResult> Delete([FromODataUri] string key)
        {
            ApplicationUser async = await this.userManager.FindByIdAsync(key);
            if (async == null)
                return (IActionResult)this.NotFound();
            await this.userManager.DeleteAsync(async);
            return NoContent();
        }

        private bool ApplicationUserExists(Guid key)
        {
            return this.userManager.Users.Count<ApplicationUser>((Expression<Func<ApplicationUser, bool>>)(e => e.Id == key.ToString())) > 0;
        }
    }

    public class CustomApplicationUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public IList<Claim> Claims { get; set; } = new List<Claim>();
    }

}