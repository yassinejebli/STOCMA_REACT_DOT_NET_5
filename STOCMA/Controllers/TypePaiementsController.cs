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
    public class TypePaiementsController : ODataController
    {
        private readonly ApplicationDbContext db;

        public TypePaiementsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery]
        public IQueryable<TypePaiement> GetTypePaiements()
        {
            return (IQueryable<TypePaiement>)this.db.TypePaiements;
        }

        [EnableQuery]
        public SingleResult<TypePaiement> GetTypePaiement([FromODataUri] Guid key)
        {
            return SingleResult.Create<TypePaiement>(this.db.TypePaiements.Where<TypePaiement>((Expression<Func<TypePaiement, bool>>)(typePaiement => typePaiement.Id == key)));
        }

        public async Task<IActionResult> Put([FromODataUri] Guid key, Delta<TypePaiement> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            TypePaiement typePaiement = await this.db.TypePaiements.FindAsync((object)key);
            if (typePaiement == null)
                return (IActionResult)this.NotFound();
            patch.Put(typePaiement);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.TypePaiementExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<TypePaiement>(typePaiement);
        }

        public async Task<IActionResult> Post([FromBody] TypePaiement typePaiement)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            this.db.TypePaiements.Add(typePaiement);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.TypePaiementExists(typePaiement.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            return (IActionResult)this.Created<TypePaiement>(typePaiement);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<TypePaiement> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            TypePaiement typePaiement = await this.db.TypePaiements.FindAsync((object)key);
            if (typePaiement == null)
                return (IActionResult)this.NotFound();
            patch.Patch(typePaiement);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.TypePaiementExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<TypePaiement>(typePaiement);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            TypePaiement async = await this.db.TypePaiements.FindAsync((object)key);
            if (async == null)
                return (IActionResult)this.NotFound();
            this.db.TypePaiements.Remove(async);
            await this.db.SaveChangesAsync();
            return (IActionResult)this.NoContent();
        }

        [EnableQuery]
        public IQueryable<Paiement> GetPaiements([FromODataUri] Guid key)
        {
            return this.db.TypePaiements.Where<TypePaiement>((Expression<Func<TypePaiement, bool>>)(m => m.Id == key)).SelectMany<TypePaiement, Paiement>((Expression<Func<TypePaiement, IEnumerable<Paiement>>>)(m => m.Paiements));
        }

        private bool TypePaiementExists(Guid key)
        {
            return this.db.TypePaiements.Count<TypePaiement>((Expression<Func<TypePaiement, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
