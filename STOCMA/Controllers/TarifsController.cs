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
    public class TarifsController : ODataController
    {
        private readonly ApplicationDbContext db;

        public TarifsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<Tarif> GetTarifs()
        {
            return db.Tarifs.OrderByDescending(x => x.Date); ;
        }

        [EnableQuery]
        public SingleResult<Tarif> GetTarif([FromODataUri] Guid key)
        {
            return SingleResult.Create<Tarif>(this.db.Tarifs.Where<Tarif>((Expression<Func<Tarif, bool>>)(tarif => tarif.Id == key)));
        }

        [EnableQuery]
        public async Task<IActionResult> Put([FromODataUri] Guid key, Tarif newTarif)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            Tarif tarif = await this.db.Tarifs.FindAsync((object)key);
            if (tarif == null)
                return (IActionResult)this.NotFound();

            db.TarifItems.RemoveRange(tarif.TarifItems);
            db.TarifItems.AddRange(newTarif.TarifItems);
            tarif.Date = newTarif.Date;
            tarif.Ref = newTarif.Ref;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.TarifExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            var tarifWithItems = db.Tarifs.Where(x => x.Id == tarif.Id);
            return Ok(tarifWithItems);
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] Tarif tarif)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            this.db.Tarifs.Add(tarif);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.TarifExists(tarif.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            var tarifWithItems = db.Tarifs.Where(x => x.Id == tarif.Id);
            return Ok(tarifWithItems);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<Tarif> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            Tarif tarif = await this.db.Tarifs.FindAsync((object)key);
            if (tarif == null)
                return (IActionResult)this.NotFound();
            patch.Patch(tarif);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.TarifExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<Tarif>(tarif);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            Tarif async = await this.db.Tarifs.FindAsync((object)key);
            if (async == null)
                return (IActionResult)this.NotFound();
            db.TarifItems.RemoveRange(async.TarifItems);
            db.Tarifs.Remove(async);
            await db.SaveChangesAsync();
            return (IActionResult)this.NoContent();
        }

        [EnableQuery]
        public IQueryable<TarifItem> GetTarifItems([FromODataUri] Guid key)
        {
            return this.db.Tarifs.Where<Tarif>((Expression<Func<Tarif, bool>>)(m => m.Id == key)).SelectMany<Tarif, TarifItem>((Expression<Func<Tarif, IEnumerable<TarifItem>>>)(m => m.TarifItems));
        }

        private bool TarifExists(Guid key)
        {
            return this.db.Tarifs.Count<Tarif>((Expression<Func<Tarif, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
