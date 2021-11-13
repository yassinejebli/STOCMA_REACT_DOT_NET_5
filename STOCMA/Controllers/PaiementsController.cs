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
    public class PaiementsController : ODataController
    {
        private readonly ApplicationDbContext db;

        public PaiementsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<Paiement> GetPaiements()
        {
            return (IQueryable<Paiement>)this.db.Paiements.OrderByDescending(x => x.Date);
        }

        [EnableQuery]
        public SingleResult<Paiement> GetPaiement([FromODataUri] Guid key)
        {
            return SingleResult.Create<Paiement>(this.db.Paiements.Where<Paiement>((Expression<Func<Paiement, bool>>)(paiement => paiement.Id == key)));
        }

        public async Task<IActionResult> Put([FromODataUri] Guid key, Delta<Paiement> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            Paiement paiement = await this.db.Paiements.FindAsync((object)key);
            if (paiement == null)
                return (IActionResult)this.NotFound();
            patch.Put(paiement);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.PaiementExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<Paiement>(paiement);
        }

        public async Task<IActionResult> Post([FromBody] Paiement paiement)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            this.db.Paiements.Add(paiement);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.PaiementExists(paiement.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            return (IActionResult)this.Created<Paiement>(paiement);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<Paiement> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            Paiement paiement = await this.db.Paiements.FindAsync((object)key);
            if (paiement == null)
                return (IActionResult)this.NotFound();
            patch.Patch(paiement);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.PaiementExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<Paiement>(paiement);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            Paiement async = await this.db.Paiements.FindAsync((object)key);
            if (async == null)
                return (IActionResult)this.NotFound();
            this.db.Paiements.Remove(async);
            int num = await this.db.SaveChangesAsync();
            return (IActionResult)this.NoContent();
        }

        [EnableQuery]
        public SingleResult<BonLivraison> GetBonLivraison([FromODataUri] Guid key)
        {
            return SingleResult.Create<BonLivraison>(this.db.Paiements.Where<Paiement>((Expression<Func<Paiement, bool>>)(m => m.Id == key)).Select<Paiement, BonLivraison>((Expression<Func<Paiement, BonLivraison>>)(m => m.BonLivraison)));
        }

        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] Guid key)
        {
            return SingleResult.Create<Client>(this.db.Paiements.Where<Paiement>((Expression<Func<Paiement, bool>>)(m => m.Id == key)).Select<Paiement, Client>((Expression<Func<Paiement, Client>>)(m => m.Client)));
        }

        [EnableQuery]
        public SingleResult<TypePaiement> GetTypePaiement([FromODataUri] Guid key)
        {
            return SingleResult.Create<TypePaiement>(this.db.Paiements.Where<Paiement>((Expression<Func<Paiement, bool>>)(m => m.Id == key)).Select<Paiement, TypePaiement>((Expression<Func<Paiement, TypePaiement>>)(m => m.TypePaiement)));
        }

        private bool PaiementExists(Guid key)
        {
            return this.db.Paiements.Count<Paiement>((Expression<Func<Paiement, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
