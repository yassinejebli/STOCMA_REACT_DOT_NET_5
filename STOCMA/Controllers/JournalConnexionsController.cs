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
    public class JournalConnexionsController : ODataController
    {
        private readonly ApplicationDbContext db;

        public JournalConnexionsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery]
        public IQueryable<JournalConnexion> GetJournalConnexions()
        {
            return (IQueryable<JournalConnexion>)this.db.JournalConnexions;
        }

        [EnableQuery]
        public SingleResult<JournalConnexion> GetJournalConnexion([FromODataUri] Guid key)
        {
            return SingleResult.Create<JournalConnexion>(this.db.JournalConnexions.Where<JournalConnexion>((Expression<Func<JournalConnexion, bool>>)(journalConnexion => journalConnexion.Id == key)));
        }

        public async Task<IActionResult> Put([FromODataUri] Guid key, Delta<JournalConnexion> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            JournalConnexion journalConnexion = await this.db.JournalConnexions.FindAsync((object)key);
            if (journalConnexion == null)
                return (IActionResult)this.NotFound();
            patch.Put(journalConnexion);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.JournalConnexionExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<JournalConnexion>(journalConnexion);
        }

        public async Task<IActionResult> Post([FromBody] JournalConnexion journalConnexion)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            this.db.JournalConnexions.Add(journalConnexion);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.JournalConnexionExists(journalConnexion.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            return (IActionResult)this.Created<JournalConnexion>(journalConnexion);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<JournalConnexion> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            JournalConnexion journalConnexion = await this.db.JournalConnexions.FindAsync((object)key);
            if (journalConnexion == null)
                return (IActionResult)this.NotFound();
            patch.Patch(journalConnexion);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.JournalConnexionExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<JournalConnexion>(journalConnexion);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            JournalConnexion async = await this.db.JournalConnexions.FindAsync((object)key);
            if (async == null)
                return (IActionResult)this.NotFound();
            this.db.JournalConnexions.Remove(async);
            int num = await this.db.SaveChangesAsync();
            return (IActionResult)this.NoContent();
        }

        private bool JournalConnexionExists(Guid key)
        {
            return this.db.JournalConnexions.Count<JournalConnexion>((Expression<Func<JournalConnexion, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
