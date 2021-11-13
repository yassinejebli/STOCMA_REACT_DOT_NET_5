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
    public class TypeDepensesController : ODataController
    {
        private readonly ApplicationDbContext db;

        public TypeDepensesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery]
        public IQueryable<TypeDepense> GetTypeDepenses()
        {
            return (IQueryable<TypeDepense>)this.db.TypeDepenses;
        }

        [EnableQuery]
        public SingleResult<TypeDepense> GetTypeDepense([FromODataUri] Guid key)
        {
            return SingleResult.Create<TypeDepense>(this.db.TypeDepenses.Where<TypeDepense>((Expression<Func<TypeDepense, bool>>)(typeDepense => typeDepense.Id == key)));
        }

        public async Task<IActionResult> Put([FromODataUri] Guid key, Delta<TypeDepense> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            TypeDepense typeDepense = await this.db.TypeDepenses.FindAsync((object)key);
            if (typeDepense == null)
                return (IActionResult)this.NotFound();
            patch.Put(typeDepense);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.TypeDepenseExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<TypeDepense>(typeDepense);
        }

        public async Task<IActionResult> Post([FromBody] TypeDepense typeDepense)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            this.db.TypeDepenses.Add(typeDepense);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.TypeDepenseExists(typeDepense.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            return (IActionResult)this.Created<TypeDepense>(typeDepense);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<TypeDepense> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            TypeDepense typeDepense = await this.db.TypeDepenses.FindAsync((object)key);
            if (typeDepense == null)
                return (IActionResult)this.NotFound();
            patch.Patch(typeDepense);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.TypeDepenseExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<TypeDepense>(typeDepense);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            TypeDepense async = await this.db.TypeDepenses.FindAsync((object)key);
            if (async == null)
                return (IActionResult)this.NotFound();
            this.db.TypeDepenses.Remove(async);
            int num = await this.db.SaveChangesAsync();
            return (IActionResult)this.NoContent();
        }


        private bool TypeDepenseExists(Guid key)
        {
            return this.db.TypeDepenses.Count<TypeDepense>((Expression<Func<TypeDepense, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
