using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
using STOCMA.Utils;

namespace STOCMA.Controllers
{
    //[Authorize]
    public class BonCommandesController : ODataController
    {
        private readonly ApplicationDbContext db;

        public BonCommandesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<BonCommande> GetBonCommandes()
        {
            return (IQueryable<BonCommande>)this.db.BonCommandes.OrderByDescending(x => x.Date);
        }

        [EnableQuery]
        public SingleResult<BonCommande> GetBonCommande([FromODataUri] Guid key)
        {
            return SingleResult.Create<BonCommande>(this.db.BonCommandes.Where<BonCommande>((Expression<Func<BonCommande, bool>>)(bonCommande => bonCommande.Id == key)));
        }

        [EnableQuery]
        public async Task<IActionResult> Put([FromODataUri] Guid key, BonCommande newBonCommande)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            BonCommande bonCommande = await this.db.BonCommandes.FindAsync((object)key);
            if (bonCommande == null)
                return (IActionResult)this.NotFound();

            db.BonCommandeItems.RemoveRange(bonCommande.BonCommandeItems);
            db.BonCommandeItems.AddRange(newBonCommande.BonCommandeItems);
            bonCommande.Date = newBonCommande.Date;
            bonCommande.Ref = newBonCommande.Ref;
            bonCommande.Note = newBonCommande.Note;

            var numBonGenerator = new DocNumberGenerator();

            bonCommande.NumBon = numBonGenerator.getNumDocByCompany(bonCommande.Ref - 1, newBonCommande.Date);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.BonCommandeExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            var bonCommandeWithItems = db.BonCommandes.Where(x => x.Id == bonCommande.Id);
            return Ok(bonCommandeWithItems);
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] BonCommande bonCommande)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);

            var numBonGenerator = new DocNumberGenerator();
            var currentYear = DateTime.Now.Year;
            var lastDoc = db.BonCommandes.Where(x => x.Date.Year == currentYear).OrderByDescending(x => x.Ref).FirstOrDefault();
            var lastRef = lastDoc != null ? lastDoc.Ref : 0;
            bonCommande.Ref = lastRef + 1;
            bonCommande.NumBon = numBonGenerator.getNumDocByCompany(lastRef, bonCommande.Date);

            this.db.BonCommandes.Add(bonCommande);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.BonCommandeExists(bonCommande.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            var bonCommandeWithItems = db.BonCommandes.Where(x => x.Id == bonCommande.Id);
            return Ok(bonCommandeWithItems);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<BonCommande> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            BonCommande bonCommande = await this.db.BonCommandes.FindAsync((object)key);
            if (bonCommande == null)
                return (IActionResult)this.NotFound();
            patch.Patch(bonCommande);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.BonCommandeExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<BonCommande>(bonCommande);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            BonCommande bonCommande = await this.db.BonCommandes.FindAsync((object)key);
            if (bonCommande == null)
                return (IActionResult)this.NotFound();

            db.BonCommandeItems.RemoveRange(bonCommande.BonCommandeItems);
            db.BonCommandes.Remove(bonCommande);
            await db.SaveChangesAsync();
            return (IActionResult)this.NoContent();
        }

        [EnableQuery]
        public IQueryable<BonCommandeItem> GetBonCommandeItems([FromODataUri] Guid key)
        {
            return this.db.BonCommandes.Where<BonCommande>((Expression<Func<BonCommande, bool>>)(m => m.Id == key)).SelectMany<BonCommande, BonCommandeItem>((Expression<Func<BonCommande, IEnumerable<BonCommandeItem>>>)(m => m.BonCommandeItems));
        }

        [EnableQuery]
        public SingleResult<Fournisseur> GetFournisseur([FromODataUri] Guid key)
        {
            return SingleResult.Create<Fournisseur>(this.db.BonCommandes.Where<BonCommande>((Expression<Func<BonCommande, bool>>)(m => m.Id == key)).Select<BonCommande, Fournisseur>((Expression<Func<BonCommande, Fournisseur>>)(m => m.Fournisseur)));
        }


        private bool BonCommandeExists(Guid key)
        {
            return this.db.BonCommandes.Count<BonCommande>((Expression<Func<BonCommande, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
