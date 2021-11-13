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
    public class RdbFsController : ODataController
    {
        private readonly ApplicationDbContext db;

        public RdbFsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET: odata/RdbFs
        [EnableQuery]
        public IQueryable<RdbF> GetRdbFs()
        {
            return db.RdbFs;
        }

        // GET: odata/RdbFs(5)
        [EnableQuery]
        public SingleResult<RdbF> GetRdbF([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RdbFs.Where(rdbF => rdbF.Id == key));
        }

        // PUT: odata/RdbFs(5)
        public async Task<IActionResult> Put([FromODataUri] Guid key, Delta<RdbF> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            RdbF rdbF = await db.RdbFs.FindAsync(key);
            if (rdbF == null)
            {
                return NotFound();
            }

            patch.Put(rdbF);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RdbFExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(rdbF);
        }

        // POST: odata/RdbFs
        public async Task<IActionResult> Post([FromBody] RdbF rdbF)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RdbFs.Add(rdbF);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RdbFExists(rdbF.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(rdbF);
        }

        // PATCH: odata/RdbFs(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<RdbF> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            RdbF rdbF = await db.RdbFs.FindAsync(key);
            if (rdbF == null)
            {
                return NotFound();
            }

            patch.Patch(rdbF);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RdbFExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(rdbF);
        }

        // DELETE: odata/RdbFs(5)
        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            RdbF rdbF = await db.RdbFs.FindAsync(key);
            if (rdbF == null)
            {
                return NotFound();
            }

            db.RdbFs.Remove(rdbF);
            await db.SaveChangesAsync();

            return NoContent();
        }

        // GET: odata/RdbFs(5)/Fournisseur
        [EnableQuery]
        public SingleResult<Fournisseur> GetFournisseur([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.RdbFs.Where(m => m.Id == key).Select(m => m.Fournisseur));
        }

        // GET: odata/RdbFs(5)/RdbFItems
        [EnableQuery]
        public IQueryable<RdbFItem> GetRdbFItems([FromODataUri] Guid key)
        {
            return db.RdbFs.Where(m => m.Id == key).SelectMany(m => m.RdbFItems);
        }

        private bool RdbFExists(Guid key)
        {
            return db.RdbFs.Count(e => e.Id == key) > 0;
        }
    }
}
