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
    public class FournisseursController : ODataController
    {
        private readonly ApplicationDbContext db;

        public FournisseursController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<Fournisseur> GetFournisseurs()
        {
            return (IQueryable<Fournisseur>)this.db.Fournisseurs.OrderBy(x => new { x.Disabled, x.Name });
        }

        [EnableQuery]
        public SingleResult<Fournisseur> GetFournisseur([FromODataUri] Guid key)
        {
            return SingleResult.Create<Fournisseur>(this.db.Fournisseurs.Where<Fournisseur>((Expression<Func<Fournisseur, bool>>)(fournisseur => fournisseur.Id == key)));
        }

        public async Task<IActionResult> Put([FromODataUri] Guid key, Delta<Fournisseur> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            Fournisseur fournisseur = await this.db.Fournisseurs.FindAsync((object)key);
            if (fournisseur == null)
                return (IActionResult)this.NotFound();
            patch.Put(fournisseur);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.FournisseurExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<Fournisseur>(fournisseur);
        }

        public async Task<IActionResult> Post([FromBody] Fournisseur fournisseur)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            this.db.Fournisseurs.Add(fournisseur);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.FournisseurExists(fournisseur.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            return (IActionResult)this.Created<Fournisseur>(fournisseur);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<Fournisseur> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            Fournisseur fournisseur = await this.db.Fournisseurs.FindAsync((object)key);
            if (fournisseur == null)
                return (IActionResult)this.NotFound();
            patch.Patch(fournisseur);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.FournisseurExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<Fournisseur>(fournisseur);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            Fournisseur async = await this.db.Fournisseurs.FindAsync((object)key);
            if (async == null)
                return (IActionResult)this.NotFound();
            this.db.Fournisseurs.Remove(async);
            int num = await this.db.SaveChangesAsync();
            return (IActionResult)this.NoContent();
        }

        [EnableQuery]
        public IQueryable<BonReception> GetBonReceptions([FromODataUri] Guid key)
        {
            return this.db.Fournisseurs.Where<Fournisseur>((Expression<Func<Fournisseur, bool>>)(m => m.Id == key)).SelectMany<Fournisseur, BonReception>((Expression<Func<Fournisseur, IEnumerable<BonReception>>>)(m => m.BonReceptions));
        }

        private bool FournisseurExists(Guid key)
        {
            return this.db.Fournisseurs.Count<Fournisseur>((Expression<Func<Fournisseur, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
