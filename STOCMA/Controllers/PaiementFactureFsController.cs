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
    public class PaiementFactureFsController : ODataController
    {
        private readonly ApplicationDbContext db;

        public PaiementFactureFsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<PaiementFactureF> GetPaiementFactureFs()
        {
            return db.PaiementFactureFs.OrderByDescending(x => x.Date);
        }

        [EnableQuery]
        public SingleResult<PaiementFactureF> GetPaiementFactureF([FromODataUri] Guid key)
        {
            return SingleResult.Create<PaiementFactureF>(this.db.PaiementFactureFs.Where<PaiementFactureF>((Expression<Func<PaiementFactureF, bool>>)(paiementFactureF => paiementFactureF.Id == key)));
        }

        public async Task<IActionResult> Put([FromODataUri] Guid key, Delta<PaiementFactureF> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            PaiementFactureF paiementFactureF = await this.db.PaiementFactureFs.FindAsync((object)key);
            if (paiementFactureF == null)
                return (IActionResult)this.NotFound();
            patch.Put(paiementFactureF);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.PaiementFactureFExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<PaiementFactureF>(paiementFactureF);
        }

        public async Task<IActionResult> Post([FromBody] PaiementFactureF paiementFactureF)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            this.db.PaiementFactureFs.Add(paiementFactureF);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.PaiementFactureFExists(paiementFactureF.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            return (IActionResult)this.Created<PaiementFactureF>(paiementFactureF);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<PaiementFactureF> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            PaiementFactureF paiementFactureF = await this.db.PaiementFactureFs.FindAsync((object)key);
            if (paiementFactureF == null)
                return (IActionResult)this.NotFound();
            patch.Patch(paiementFactureF);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.PaiementFactureFExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<PaiementFactureF>(paiementFactureF);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            PaiementFactureF async = await this.db.PaiementFactureFs.FindAsync((object)key);
            if (async == null)
                return (IActionResult)this.NotFound();
            this.db.PaiementFactureFs.Remove(async);
            int num = await this.db.SaveChangesAsync();
            return (IActionResult)this.NoContent();
        }

        [EnableQuery]
        public SingleResult<FactureF> GetFactureF([FromODataUri] Guid key)
        {
            return SingleResult.Create<FactureF>(this.db.PaiementFactureFs.Where<PaiementFactureF>((Expression<Func<PaiementFactureF, bool>>)(m => m.Id == key)).Select<PaiementFactureF, FactureF>((Expression<Func<PaiementFactureF, FactureF>>)(m => m.FactureF)));
        }

        [EnableQuery]
        public SingleResult<Fournisseur> GetFournisseur([FromODataUri] Guid key)
        {
            return SingleResult.Create<Fournisseur>(this.db.PaiementFactureFs.Where<PaiementFactureF>((Expression<Func<PaiementFactureF, bool>>)(m => m.Id == key)).Select<PaiementFactureF, Fournisseur>((Expression<Func<PaiementFactureF, Fournisseur>>)(m => m.Fournisseur)));
        }

        [EnableQuery]
        public SingleResult<TypePaiement> GetTypePaiementFactureF([FromODataUri] Guid key)
        {
            return SingleResult.Create<TypePaiement>(this.db.PaiementFactureFs.Where<PaiementFactureF>((Expression<Func<PaiementFactureF, bool>>)(m => m.Id == key)).Select<PaiementFactureF, TypePaiement>((Expression<Func<PaiementFactureF, TypePaiement>>)(m => m.TypePaiement)));
        }

        private bool PaiementFactureFExists(Guid key)
        {
            return this.db.PaiementFactureFs.Count<PaiementFactureF>((Expression<Func<PaiementFactureF, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
