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
    public class SitesController : ODataController
    {
        private readonly ApplicationDbContext db;

        public SitesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET: odata/Sites
        [EnableQuery]
        public IQueryable<Site> GetSites()
        {
            return db.Sites;
        }

        // GET: odata/Sites(5)
        [EnableQuery]
        public SingleResult<Site> GetSite([FromODataUri] int key)
        {
            return SingleResult.Create(db.Sites.Where(site => site.Id == key));
        }

        // PUT: odata/Sites(5)
        public async Task<IActionResult> Put([FromODataUri] int key, Delta<Site> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Site site = await db.Sites.FindAsync(key);
            if (site == null)
            {
                return NotFound();
            }

            patch.Put(site);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiteExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(site);
        }

        // POST: odata/Sites
        public async Task<IActionResult> Post([FromBody] Site site)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            site.ArticleSites = new List<ArticleSite>();
            db.Sites.Add(site);
            foreach (var article in db.Articles)
            {
                site.ArticleSites.Add(new ArticleSite
                {
                    Article = article,
                    QteStock = 0,
                    Site = site,
                    Disabled = true,
                });
            }
            await db.SaveChangesAsync();

            return Created(site);
        }

        // PATCH: odata/Sites(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IActionResult> Patch([FromODataUri] int key, Delta<Site> patch)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Site site = await db.Sites.FindAsync(key);
            if (site == null)
            {
                return NotFound();
            }

            patch.Patch(site);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiteExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(site);
        }

        // DELETE: odata/Sites(5)
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            Site site = await db.Sites.FindAsync(key);
            if (site == null)
            {
                return NotFound();
            }

            //db.Sites.Remove(site);
            site.Disabled = true;
            await db.SaveChangesAsync();

            return NoContent();
        }

        // GET: odata/Sites(5)/ArticleSites
        [EnableQuery]
        public IQueryable<ArticleSite> GetArticleSites([FromODataUri] int key)
        {
            return db.Sites.Where(m => m.Id == key).SelectMany(m => m.ArticleSites);
        }

        private bool SiteExists(int key)
        {
            return db.Sites.Count(e => e.Id == key) > 0;
        }
    }
}
