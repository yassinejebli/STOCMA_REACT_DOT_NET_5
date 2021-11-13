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
    //[Authorize]
    public class DepensesController : ODataController
    {
        private readonly ApplicationDbContext db;

        public DepensesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<Depense> GetDepenses()
        {
            return (IQueryable<Depense>)this.db.Depenses.OrderByDescending(x => x.Date);
        }

        [EnableQuery]
        public SingleResult<Depense> GetDepense([FromODataUri] Guid key)
        {
            return SingleResult.Create<Depense>(this.db.Depenses.Where<Depense>((Expression<Func<Depense, bool>>)(depense => depense.Id == key)));
        }

        [EnableQuery]
        public async Task<IActionResult> Put([FromODataUri] Guid key, Depense newDepense)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            Depense depense = await this.db.Depenses.FindAsync((object)key);
            if (depense == null)
                return (IActionResult)this.NotFound();

            depense.Date = newDepense.Date;
            depense.Titre = newDepense.Titre;
            //-----------------------------------------------Updating document items
            db.DepenseItems.RemoveRange(depense.DepenseItems);
            db.DepenseItems.AddRange(newDepense.DepenseItems);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.DepenseExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            var depenseWithItems = db.Depenses.Where(x => x.Id == depense.Id);
            return Ok(depenseWithItems);
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] Depense depense)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            this.db.Depenses.Add(depense);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.DepenseExists(depense.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            var depenseWithItems = db.Depenses.Where(x => x.Id == depense.Id);
            return Ok(depenseWithItems);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<Depense> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            Depense depense = await this.db.Depenses.FindAsync((object)key);
            if (depense == null)
                return (IActionResult)this.NotFound();
            patch.Patch(depense);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.DepenseExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<Depense>(depense);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            Depense async = await this.db.Depenses.FindAsync((object)key);
            if (async == null)
                return (IActionResult)this.NotFound();

            db.DepenseItems.RemoveRange(async.DepenseItems);
            this.db.Depenses.Remove(async);
            int num = await this.db.SaveChangesAsync();
            return (IActionResult)this.NoContent();
        }

        [EnableQuery]
        public IQueryable<DepenseItem> GetDepenseITems([FromODataUri] Guid key)
        {
            return this.db.Depenses.Where(m => m.Id == key).SelectMany(m => m.DepenseItems);
        }

        private bool DepenseExists(Guid key)
        {
            return this.db.Depenses.Count<Depense>((Expression<Func<Depense, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
