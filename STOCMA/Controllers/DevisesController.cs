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
using STOCMA.Utils;

namespace STOCMA.Controllers
{
    //[Authorize]
    public class DevisesController : ODataController
    {
        private readonly ApplicationDbContext db;

        public DevisesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<Devis> GetDevises()
        {
            return (IQueryable<Devis>)this.db.Devises.OrderByDescending(x => x.Date);
        }

        [EnableQuery(MaxExpansionDepth = 5)]
        public SingleResult<Devis> GetDevis([FromODataUri] Guid key)
        {
            return SingleResult.Create<Devis>(this.db.Devises.Where<Devis>((Expression<Func<Devis, bool>>)(devis => devis.Id == key)));
        }

        [EnableQuery]
        public async Task<IActionResult> Put([FromODataUri] Guid key, Devis newDevis)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            Devis devis = await this.db.Devises.FindAsync((object)key);
            if (devis == null)
                return (IActionResult)this.NotFound();
            //-----------------------------------------------Updating document items
            db.DevisItems.RemoveRange(devis.DevisItems);
            db.DevisItems.AddRange(newDevis.DevisItems);

            devis.Ref = newDevis.Ref;
            devis.Date = newDevis.Date;
            devis.Note = newDevis.Note;
            devis.ClientName = newDevis.ClientName;
            devis.WithDiscount = newDevis.WithDiscount;
            devis.IdTypePaiement = newDevis.IdTypePaiement;
            devis.ValiditeOffre = newDevis.ValiditeOffre;
            devis.TransportExpedition = newDevis.TransportExpedition;
            devis.DelaiLivrasion = newDevis.DelaiLivrasion;
            var numBonGenerator = new DocNumberGenerator(db);

            devis.NumBon = numBonGenerator.getNumDocByCompany(newDevis.Ref - 1, newDevis.Date);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.DevisExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            var devisWithItems = db.Devises.Where(x => x.Id == devis.Id);
            return Ok(devisWithItems);
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] Devis devis)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);

            var numBonGenerator = new DocNumberGenerator(db);
            var currentYear = DateTime.Now.Year;
            var lastDoc = db.Devises.Where(x => x.Date.Year == currentYear && x.IdSite == devis.IdSite).OrderByDescending(x => x.Ref).FirstOrDefault();
            var lastRef = lastDoc != null ? lastDoc.Ref : 0;
            devis.Ref = lastRef + 1;
            devis.NumBon = numBonGenerator.getNumDocByCompany(lastRef, devis.Date);
            this.db.Devises.Add(devis);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.DevisExists(devis.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            var devisWithItems = db.Devises.Where(x => x.Id == devis.Id);
            return Ok(devisWithItems);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<Devis> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            Devis devis = await this.db.Devises.FindAsync((object)key);
            if (devis == null)
                return (IActionResult)this.NotFound();
            patch.Patch(devis);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.DevisExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<Devis>(devis);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            Devis async = await this.db.Devises.FindAsync((object)key);
            if (async == null)
                return (IActionResult)this.NotFound();
            db.DevisItems.RemoveRange(async.DevisItems);
            db.Devises.Remove(async);
            await this.db.SaveChangesAsync();
            return (IActionResult)this.NoContent();
        }

        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] Guid key)
        {
            return SingleResult.Create<Client>(this.db.Devises.Where<Devis>((Expression<Func<Devis, bool>>)(m => m.Id == key)).Select<Devis, Client>((Expression<Func<Devis, Client>>)(m => m.Client)));
        }

        [EnableQuery]
        public IQueryable<DevisItem> GetDevisItems([FromODataUri] Guid key)
        {
            return this.db.Devises.Where<Devis>((Expression<Func<Devis, bool>>)(m => m.Id == key)).SelectMany<Devis, DevisItem>((Expression<Func<Devis, IEnumerable<DevisItem>>>)(m => m.DevisItems));
        }

        private bool DevisExists(Guid key)
        {
            return this.db.Devises.Count<Devis>((Expression<Func<Devis, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
