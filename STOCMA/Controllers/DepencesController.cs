using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
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
    public class DepencesController : ODataController
    {
        private readonly ApplicationDbContext db;

        public DepencesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery]
        public IQueryable<Depence> GetDepences()
        {
            return (IQueryable<Depence>)this.db.Depences;
        }

        [EnableQuery]
        public SingleResult<Depence> GetDepence([FromODataUri] Guid key)
        {
            return SingleResult.Create<Depence>(this.db.Depences.Where<Depence>((Expression<Func<Depence, bool>>)(depence => depence.Id == key)));
        }

        public async Task<IActionResult> Put([FromODataUri] Guid key, Delta<Depence> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            Depence depence = await this.db.Depences.FindAsync((object)key);
            if (depence == null)
                return (IActionResult)this.NotFound();
            patch.Put(depence);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.DepenceExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<Depence>(depence);
        }

        public async Task<IActionResult> Post([FromBody] Depence depence)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            this.db.Depences.Add(depence);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.DepenceExists(depence.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            return (IActionResult)this.Created<Depence>(depence);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<Depence> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            Depence depence = await this.db.Depences.FindAsync((object)key);
            if (depence == null)
                return (IActionResult)this.NotFound();
            patch.Patch(depence);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.DepenceExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<Depence>(depence);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            Depence async = await this.db.Depences.FindAsync((object)key);
            if (async == null)
                return (IActionResult)this.NotFound();
            this.db.Depences.Remove(async);
            int num = await this.db.SaveChangesAsync();
            return (IActionResult)this.NoContent();
        }

        [EnableQuery]
        public SingleResult<TypeDepence> GetTypeDepence([FromODataUri] Guid key)
        {
            return SingleResult.Create<TypeDepence>(this.db.Depences.Where<Depence>((Expression<Func<Depence, bool>>)(m => m.Id == key)).Select<Depence, TypeDepence>((Expression<Func<Depence, TypeDepence>>)(m => m.TypeDepence)));
        }

        private bool DepenceExists(Guid key)
        {
            return this.db.Depences.Count<Depence>((Expression<Func<Depence, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
