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
    public class RevendeursController : ODataController
    {
        private readonly ApplicationDbContext db;

        public RevendeursController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET: odata/Revendeurs
        [EnableQuery]
        public IQueryable<Revendeur> GetRevendeurs()
        {
            return db.Revendeurs;
        }

        // GET: odata/Revendeurs(5)
        [EnableQuery]
        public SingleResult<Revendeur> GetRevendeur([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Revendeurs.Where(revendeur => revendeur.Id == key));
        }

        // PUT: odata/Revendeurs(5)
        public async Task<IActionResult> Put([FromODataUri] Guid key, Delta<Revendeur> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Revendeur revendeur = await db.Revendeurs.FindAsync(key);
            if (revendeur == null)
            {
                return NotFound();
            }

            patch.Put(revendeur);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RevendeurExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(revendeur);
        }

        // POST: odata/Revendeurs
        public async Task<IActionResult> Post([FromBody] Revendeur revendeur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Revendeurs.Add(revendeur);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RevendeurExists(revendeur.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(revendeur);
        }

        // PATCH: odata/Revendeurs(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<Revendeur> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Revendeur revendeur = await db.Revendeurs.FindAsync(key);
            if (revendeur == null)
            {
                return NotFound();
            }

            patch.Patch(revendeur);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RevendeurExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(revendeur);
        }

        // DELETE: odata/Revendeurs(5)
        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            Revendeur revendeur = await db.Revendeurs.FindAsync(key);
            if (revendeur == null)
            {
                return NotFound();
            }

            db.Revendeurs.Remove(revendeur);
            await db.SaveChangesAsync();

            return NoContent();
        }

        // GET: odata/Revendeurs(5)/Clients
        [EnableQuery]
        public IQueryable<Client> GetClients([FromODataUri] Guid key)
        {
            return db.Revendeurs.Where(m => m.Id == key).SelectMany(m => m.Clients);
        }


        private bool RevendeurExists(Guid key)
        {
            return db.Revendeurs.Count(e => e.Id == key) > 0;
        }
    }
}
