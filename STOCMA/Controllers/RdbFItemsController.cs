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
    public class RdbFItemsController : ODataController
    {
        private readonly ApplicationDbContext db;

        public RdbFItemsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET: odata/RdbFItems
        [EnableQuery]
        public IQueryable<RdbFItem> GetRdbFItems()
        {
            return db.RdbFItems;
        }

        // GET: odata/RdbFItems(5)
        [EnableQuery]
        public SingleResult<RdbFItem> GetRdbFItem([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RdbFItems.Where(rdbFItem => rdbFItem.Id == key));
        }

        // PUT: odata/RdbFItems(5)
        public async Task<IActionResult> Put([FromODataUri] Guid key, Delta<RdbFItem> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            RdbFItem rdbFItem = await db.RdbFItems.FindAsync(key);
            if (rdbFItem == null)
            {
                return NotFound();
            }

            patch.Put(rdbFItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RdbFItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(rdbFItem);
        }

        // POST: odata/RdbFItems
        public async Task<IActionResult> Post([FromBody] RdbFItem rdbFItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RdbFItems.Add(rdbFItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RdbFItemExists(rdbFItem.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(rdbFItem);
        }

        // PATCH: odata/RdbFItems(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<RdbFItem> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            RdbFItem rdbFItem = await db.RdbFItems.FindAsync(key);
            if (rdbFItem == null)
            {
                return NotFound();
            }

            patch.Patch(rdbFItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RdbFItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(rdbFItem);
        }

        // DELETE: odata/RdbFItems(5)
        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            RdbFItem rdbFItem = await db.RdbFItems.FindAsync(key);
            if (rdbFItem == null)
            {
                return NotFound();
            }

            db.RdbFItems.Remove(rdbFItem);
            await db.SaveChangesAsync();

            return NoContent();
        }

        // GET: odata/RdbFItems(5)/Article
        [EnableQuery]
        public SingleResult<Article> GetArticle([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RdbFItems.Where(m => m.Id == key).Select(m => m.Article));
        }

        // GET: odata/RdbFItems(5)/RdbF
        [EnableQuery]
        public SingleResult<RdbF> GetRdbF([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RdbFItems.Where(m => m.Id == key).Select(m => m.RdbF));
        }

        private bool RdbFItemExists(Guid key)
        {
            return db.RdbFItems.Count(e => e.Id == key) > 0;
        }
    }
}
