using System;
using System.Linq;
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
    public class ArticleSitesController : ODataController
    {
        private readonly ApplicationDbContext db;

        public ArticleSitesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET: odata/ArticleSites
        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<ArticleSite> GetArticleSites()
        {
            return db.ArticleSites.Include(x => x.Article).Include(x => x.Site).OrderByDescending(x => x.Counter);
        }

        // GET: odata/ArticleSites(5)
        [EnableQuery]
        public SingleResult<ArticleSite> GetArticleSite([FromODataUri] int key)
        {
            return SingleResult.Create(db.ArticleSites.Where(articleSite => articleSite.IdSite == key));
        }

        // PUT: odata/ArticleSites(5)
        public async Task<IActionResult> Put([FromODataUri] int key, Delta<ArticleSite> patch)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ArticleSite articleSite = await db.ArticleSites.FindAsync(key);
            if (articleSite == null)
            {
                return NotFound();
            }

            patch.Put(articleSite);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleSiteExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(articleSite);
        }

        // POST: odata/ArticleSites
        public async Task<IActionResult> Post([FromBody] ArticleSite articleSite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ArticleSites.Add(articleSite);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ArticleSiteExists(articleSite.IdSite))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(articleSite);
        }

        // PATCH: odata/ArticleSites(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IActionResult> Patch([FromODataUri] int key, Delta<ArticleSite> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ArticleSite articleSite = await db.ArticleSites.FindAsync(key);
            if (articleSite == null)
            {
                return NotFound();
            }

            patch.Patch(articleSite);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleSiteExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(articleSite);
        }

        public async Task<IActionResult> Delete([FromODataUri] int keyIdSite, [FromODataUri] Guid keyIdArticle)
        {
            ArticleSite articleSite = await db.ArticleSites.FirstOrDefaultAsync(x => x.IdSite == keyIdSite && x.IdArticle == keyIdArticle);
            if (articleSite == null)
            {
                return NotFound();
            }

            //db.ArticleSites.Remove(articleSite);
            articleSite.Disabled = true;
            await db.SaveChangesAsync();

            return NoContent();
        }

        // GET: odata/ArticleSites(5)/Article
        [EnableQuery]
        public SingleResult<Article> GetArticle([FromODataUri] int key)
        {
            return SingleResult.Create(db.ArticleSites.Where(m => m.IdSite == key).Select(m => m.Article));
        }

        // GET: odata/ArticleSites(5)/Site
        [EnableQuery]
        public SingleResult<Site> GetSite([FromODataUri] int key)
        {
            return SingleResult.Create(db.ArticleSites.Where(m => m.IdSite == key).Select(m => m.Site));
        }

        private bool ArticleSiteExists(int key)
        {
            return db.ArticleSites.Count(e => e.IdSite == key) > 0;
        }
    }
}
