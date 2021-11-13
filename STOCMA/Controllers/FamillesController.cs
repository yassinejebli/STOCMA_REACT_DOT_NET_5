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
    public class FamillesController : ODataController
    {
        private readonly ApplicationDbContext db;

        public FamillesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET: odata/Familles
        [EnableQuery]
        public IQueryable<Famille> GetFamilles()
        {
            return db.Familles;
        }

        // GET: odata/Familles(5)
        [EnableQuery]
        public SingleResult<Famille> GetFamille([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Familles.Where(famille => famille.Id == key));
        }

        // PUT: odata/Familles(5)
        public async Task<IActionResult> Put([FromODataUri] Guid key, Delta<Famille> patch)
        {



            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Famille famille = await db.Familles.FindAsync(key);

            if (famille == null)
            {
                return NotFound();
            }

            patch.Put(famille);

            try
            {
                await db.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!FamilleExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(famille);
        }

        // POST: odata/Familles
        public async Task<IActionResult> Post([FromBody] Famille famille)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Familles.Add(famille);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FamilleExists(famille.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(famille);
        }

        // PATCH: odata/Familles(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<Famille> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Famille famille = await db.Familles.FindAsync(key);
            if (famille == null)
            {
                return NotFound();
            }

            patch.Patch(famille);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FamilleExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(famille);
        }

        // DELETE: odata/Familles(5)
        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            Famille famille = await db.Familles.FindAsync(key);
            if (famille == null)
            {
                return NotFound();
            }

            db.Familles.Remove(famille);
            await db.SaveChangesAsync();

            return NoContent();
        }

        // GET: odata/Familles(5)/Categories
        [EnableQuery]
        public IQueryable<Categorie> GetCategories([FromODataUri] Guid key)
        {
            return db.Familles.Where(m => m.Id == key).SelectMany(m => m.Categories);
        }

        private bool FamilleExists(Guid key)
        {
            return db.Familles.Count(e => e.Id == key) > 0;
        }
    }
}
