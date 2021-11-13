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
    public class CategoriesController : ODataController
    {
        private readonly ApplicationDbContext db;

        public CategoriesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET: odata/Categories
        [EnableQuery]
        public IQueryable<Categorie> GetCategories()
        {
            return db.Categories;
        }

        // GET: odata/Categories(5)
        [EnableQuery]
        public SingleResult<Categorie> GetCategorie([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Categories.Where(categorie => categorie.Id == key));
        }

        // PUT: odata/Categories(5)
        public async Task<IActionResult> Put([FromODataUri] Guid key, Delta<Categorie> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Categorie categorie = await db.Categories.FindAsync(key);
            if (categorie == null)
            {
                return NotFound();
            }

            patch.Put(categorie);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategorieExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(categorie);
        }

        // POST: odata/Categories
        public async Task<IActionResult> Post([FromBody] Categorie categorie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(categorie);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CategorieExists(categorie.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(categorie);
        }

        // PATCH: odata/Categories(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<Categorie> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Categorie categorie = await db.Categories.FindAsync(key);
            if (categorie == null)
            {
                return NotFound();
            }

            patch.Patch(categorie);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategorieExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(categorie);
        }

        // DELETE: odata/Categories(5)
        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            Categorie categorie = await db.Categories.FindAsync(key);
            if (categorie == null)
            {
                return NotFound();
            }

            db.Categories.Remove(categorie);
            await db.SaveChangesAsync();

            return NoContent();
        }

        // GET: odata/Categories(5)/Articles
        [EnableQuery]
        public IQueryable<Article> GetArticles([FromODataUri] Guid key)
        {
            return db.Categories.Where(m => m.Id == key).SelectMany(m => m.Articles);
        }

        // GET: odata/Categories(5)/Famille
        [EnableQuery]
        public SingleResult<Famille> GetFamille([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Categories.Where(m => m.Id == key).Select(m => m.Famille));
        }

        private bool CategorieExists(Guid key)
        {
            return db.Categories.Count(e => e.Id == key) > 0;
        }
    }
}
