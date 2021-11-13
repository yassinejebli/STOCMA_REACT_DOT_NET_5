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
    public class RdbsController : ODataController
    {
        private readonly ApplicationDbContext db;

        public RdbsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET: odata/Rdbs
        [EnableQuery]
        public IQueryable<Rdb> GetRdbs()
        {
            return db.Rdbs;
        }

        // GET: odata/Rdbs(5)
        [EnableQuery]
        public SingleResult<Rdb> GetRdb([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Rdbs.Where(rdb => rdb.Id == key));
        }

        // PUT: odata/Rdbs(5)
        public async Task<IActionResult> Put([FromODataUri] Guid key, Delta<Rdb> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Rdb rdb = await db.Rdbs.FindAsync(key);
            if (rdb == null)
            {
                return NotFound();
            }

            patch.Put(rdb);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RdbExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(rdb);
        }

        // POST: odata/Rdbs
        public async Task<IActionResult> Post([FromBody] Rdb rdb)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rdbs.Add(rdb);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RdbExists(rdb.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(rdb);
        }

        // PATCH: odata/Rdbs(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<Rdb> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Rdb rdb = await db.Rdbs.FindAsync(key);
            if (rdb == null)
            {
                return NotFound();
            }

            patch.Patch(rdb);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RdbExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(rdb);
        }

        // DELETE: odata/Rdbs(5)
        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            Rdb rdb = await db.Rdbs.FindAsync(key);
            if (rdb == null)
            {
                return NotFound();
            }

            db.Rdbs.Remove(rdb);
            await db.SaveChangesAsync();

            return NoContent();
        }

        // GET: odata/Rdbs(5)/Client
        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Rdbs.Where(m => m.Id == key).Select(m => m.Client));
        }

        // GET: odata/Rdbs(5)/RdbItems
        [EnableQuery]
        public IQueryable<RdbItem> GetRdbItems([FromODataUri] Guid key)
        {
            return db.Rdbs.Where(m => m.Id == key).SelectMany(m => m.RdbItems);
        }

        private bool RdbExists(Guid key)
        {
            return db.Rdbs.Count(e => e.Id == key) > 0;
        }
    }
}
