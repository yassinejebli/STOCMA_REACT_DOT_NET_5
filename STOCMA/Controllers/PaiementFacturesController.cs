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
    public class PaiementFacturesController : ODataController
    {
        private readonly ApplicationDbContext db;

        public PaiementFacturesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery(MaxExpansionDepth = 5, EnsureStableOrdering = false)]
        public IQueryable<PaiementFacture> GetPaiementFactures()
        {
            return db.PaiementFactures.OrderByDescending(x => new { x.Date, x.Id });
        }

        [EnableQuery(MaxExpansionDepth = 5)]
        public SingleResult<PaiementFacture> GetPaiementFacture([FromODataUri] Guid key)
        {
            return SingleResult.Create<PaiementFacture>(this.db.PaiementFactures.Where<PaiementFacture>((Expression<Func<PaiementFacture, bool>>)(paiementFacture => paiementFacture.Id == key)));
        }

        public async Task<IActionResult> Put([FromODataUri] Guid key, Delta<PaiementFacture> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            PaiementFacture paiementFacture = await this.db.PaiementFactures.FindAsync((object)key);
            if (paiementFacture == null)
                return (IActionResult)this.NotFound();
            patch.Put(paiementFacture);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.PaiementFactureExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<PaiementFacture>(paiementFacture);
        }

        public async Task<IActionResult> Post([FromBody] PaiementFacture paiementFacture)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            this.db.PaiementFactures.Add(paiementFacture);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.PaiementFactureExists(paiementFacture.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            return (IActionResult)this.Created<PaiementFacture>(paiementFacture);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<PaiementFacture> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            PaiementFacture paiementFacture = await this.db.PaiementFactures.FindAsync((object)key);
            if (paiementFacture == null)
                return (IActionResult)this.NotFound();
            patch.Patch(paiementFacture);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.PaiementFactureExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<PaiementFacture>(paiementFacture);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            PaiementFacture async = await this.db.PaiementFactures.FindAsync((object)key);
            if (async == null)
                return (IActionResult)this.NotFound();
            this.db.PaiementFactures.Remove(async);
            int num = await this.db.SaveChangesAsync();
            return (IActionResult)this.NoContent();
        }

        [EnableQuery]
        public SingleResult<Facture> GetFacture([FromODataUri] Guid key)
        {
            return SingleResult.Create<Facture>(this.db.PaiementFactures.Where<PaiementFacture>((Expression<Func<PaiementFacture, bool>>)(m => m.Id == key)).Select<PaiementFacture, Facture>((Expression<Func<PaiementFacture, Facture>>)(m => m.Facture)));
        }

        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] Guid key)
        {
            return SingleResult.Create<Client>(this.db.PaiementFactures.Where<PaiementFacture>((Expression<Func<PaiementFacture, bool>>)(m => m.Id == key)).Select<PaiementFacture, Client>((Expression<Func<PaiementFacture, Client>>)(m => m.Client)));
        }

        [EnableQuery]
        public SingleResult<TypePaiement> GetTypePaiementFacture([FromODataUri] Guid key)
        {
            return SingleResult.Create<TypePaiement>(this.db.PaiementFactures.Where<PaiementFacture>((Expression<Func<PaiementFacture, bool>>)(m => m.Id == key)).Select<PaiementFacture, TypePaiement>((Expression<Func<PaiementFacture, TypePaiement>>)(m => m.TypePaiement)));
        }

        private bool PaiementFactureExists(Guid key)
        {
            return this.db.PaiementFactures.Count<PaiementFacture>((Expression<Func<PaiementFacture, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
