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
    public class RdbItemsController : ODataController
    {
        private readonly ApplicationDbContext db;

        public RdbItemsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET: odata/RdbItems
        [EnableQuery]
        public IQueryable<RdbItem> GetRdbItems()
        {
            return db.RdbItems;
        }

        // GET: odata/RdbItems(5)
        [EnableQuery]
        public SingleResult<RdbItem> GetRdbItem([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RdbItems.Where(rdbItem => rdbItem.Id == key));
        }

        // PUT: odata/RdbItems(5)
        public async Task<IActionResult> Put([FromODataUri] Guid key, Delta<RdbItem> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            RdbItem rdbItem = await db.RdbItems.FindAsync(key);
            if (rdbItem == null)
            {
                return NotFound();
            }

            patch.Put(rdbItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RdbItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(rdbItem);
        }

        // POST: odata/RdbItems
        public async Task<IActionResult> Post([FromBody] RdbItem rdbItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RdbItems.Add(rdbItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RdbItemExists(rdbItem.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(rdbItem);
        }

        // PATCH: odata/RdbItems(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<RdbItem> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            RdbItem rdbItem = await db.RdbItems.FindAsync(key);
            if (rdbItem == null)
            {
                return NotFound();
            }

            patch.Patch(rdbItem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RdbItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(rdbItem);
        }

        // DELETE: odata/RdbItems(5)
        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            RdbItem rdbItem = await db.RdbItems.FindAsync(key);
            if (rdbItem == null)
            {
                return NotFound();
            }

            db.RdbItems.Remove(rdbItem);
            await db.SaveChangesAsync();

            return NoContent();
        }

        // GET: odata/RdbItems(5)/Article
        [EnableQuery]
        public SingleResult<Article> GetArticle([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RdbItems.Where(m => m.Id == key).Select(m => m.Article));
        }

        // GET: odata/RdbItems(5)/Rdb
        [EnableQuery]
        public SingleResult<Rdb> GetRdb([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RdbItems.Where(m => m.Id == key).Select(m => m.Rdb));
        }

        private bool RdbItemExists(Guid key)
        {
            return db.RdbItems.Count(e => e.Id == key) > 0;
        }
    }
}
