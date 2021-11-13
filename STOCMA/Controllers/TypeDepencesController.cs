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
    public class TypeDepencesController : ODataController
    {
        private readonly ApplicationDbContext db;

        public TypeDepencesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery]
        public IQueryable<TypeDepence> GetTypeDepences()
        {
            return (IQueryable<TypeDepence>)this.db.TypeDepences;
        }

        [EnableQuery]
        public SingleResult<TypeDepence> GetTypeDepence([FromODataUri] Guid key)
        {
            return SingleResult.Create<TypeDepence>(this.db.TypeDepences.Where<TypeDepence>((Expression<Func<TypeDepence, bool>>)(typeDepence => typeDepence.Id == key)));
        }

        public async Task<IActionResult> Put([FromODataUri] Guid key, Delta<TypeDepence> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            TypeDepence typeDepence = await this.db.TypeDepences.FindAsync((object)key);
            if (typeDepence == null)
                return (IActionResult)this.NotFound();
            patch.Put(typeDepence);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.TypeDepenceExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<TypeDepence>(typeDepence);
        }

        public async Task<IActionResult> Post([FromBody] TypeDepence typeDepence)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            this.db.TypeDepences.Add(typeDepence);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.TypeDepenceExists(typeDepence.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            return (IActionResult)this.Created<TypeDepence>(typeDepence);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<TypeDepence> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            TypeDepence typeDepence = await this.db.TypeDepences.FindAsync((object)key);
            if (typeDepence == null)
                return (IActionResult)this.NotFound();
            patch.Patch(typeDepence);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.TypeDepenceExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<TypeDepence>(typeDepence);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            TypeDepence async = await this.db.TypeDepences.FindAsync((object)key);
            if (async == null)
                return (IActionResult)this.NotFound();
            this.db.TypeDepences.Remove(async);
            int num = await this.db.SaveChangesAsync();
            return (IActionResult)this.NoContent();
        }

        [EnableQuery]
        public IQueryable<Depence> GetDepences([FromODataUri] Guid key)
        {
            return this.db.TypeDepences.Where<TypeDepence>((Expression<Func<TypeDepence, bool>>)(m => m.Id == key)).SelectMany<TypeDepence, Depence>((Expression<Func<TypeDepence, IEnumerable<Depence>>>)(m => m.Depences));
        }

        private bool TypeDepenceExists(Guid key)
        {
            return this.db.TypeDepences.Count<TypeDepence>((Expression<Func<TypeDepence, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
