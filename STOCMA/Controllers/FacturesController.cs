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
    public class FacturesController : ODataController
    {
        private readonly ApplicationDbContext db;

        public FacturesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery(EnsureStableOrdering = false, MaxExpansionDepth = 5)]
        public IQueryable<Facture> GetFactures()
        {
            return (IQueryable<Facture>)this.db.Factures.OrderByDescending(x => x.Date);
        }

        [EnableQuery(MaxExpansionDepth = 5)]
        public SingleResult<Facture> GetFacture([FromODataUri] Guid key)
        {
            return SingleResult.Create<Facture>(this.db.Factures.Where<Facture>((Expression<Func<Facture, bool>>)(facture => facture.Id == key)));
        }

        [EnableQuery]
        public async Task<IActionResult> Put([FromODataUri] Guid key, Facture newFacture)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            Facture facture = await this.db.Factures.FindAsync((object)key);
            if (facture == null)
                return (IActionResult)this.NotFound();

            //-----------------------------------------------Updating document items
            facture.ModificationDate = DateTime.Now;
            facture.Date = newFacture.Date;
            facture.Ref = newFacture.Ref;
            facture.Note = newFacture.Note;
            facture.IdTypePaiement = newFacture.IdTypePaiement;
            facture.WithDiscount = newFacture.WithDiscount;
            facture.Comment = newFacture.Comment;
            facture.DateEcheance = newFacture.DateEcheance;
            facture.ClientName = newFacture.ClientName;
            var numBonGenerator = new DocNumberGenerator();

            facture.NumBon = numBonGenerator.getNumDocByCompany(newFacture.Ref - 1, newFacture.Date);

            //------------------------Updating bon livraisons
            var oldBonLivraisonIDs = facture.BonLivraisons.Select(x => x.Id);
            var originalBonLivraisons = db.BonLivraisons.Where(x => oldBonLivraisonIDs.Contains(x.Id));
            await originalBonLivraisons.ForEachAsync(x => x.IdFacture = null);

            var newBonLivraisonIDs = newFacture.BonLivraisons.Select(x => x.Id);
            var newOriginalBonLivraisons = db.BonLivraisons.Where(x => newBonLivraisonIDs.Contains(x.Id));
            await newOriginalBonLivraisons.ForEachAsync(x => x.IdFacture = newFacture.Id);

            //------------------------Updating payment

            var payment = db.PaiementFactures.FirstOrDefault(x => x.IdFacture == facture.Id);
            var ACHAT_PAIEMENT_TYPE_ID = "399d159e-9ce0-4fcc-957a-08a65bbeecb7";


            var Total = newOriginalBonLivraisons
                    .SelectMany(x => x.BonLivraisonItems)
                    .Sum(x => ((x.Qte * x.Pu) - (x.PercentageDiscount ? (x.Qte * x.Pu * (x.Discount ?? 0.0f) / 100) : x.Discount ?? 0.0f)) * (1 + (x.Article.TVA ?? 20) / 100));

            //espece
            var company = db.Companies.FirstOrDefault();
            var ESPECE_PAYMENT_TYPE = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2");

            if (company.UseVAT && newFacture?.IdTypePaiement == ESPECE_PAYMENT_TYPE)
            {
                Total *= (1 + 0.0025f);
            }

            if (payment != null)
            {
                payment.Debit = Total;
                payment.Date = newFacture.Date;
            }
            else
            {
                PaiementFacture paiement = new PaiementFacture()
                {
                    Id = Guid.NewGuid(),
                    IdFacture = facture.Id,
                    IdClient = newFacture.IdClient,
                    Debit = Total,
                    IdTypePaiement = new Guid(ACHAT_PAIEMENT_TYPE_ID),
                    Date = newFacture.Date
                };
                db.PaiementFactures.Add(paiement);
            }

            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.FactureExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            var factureWithItems = db.Factures.Where(x => x.Id == facture.Id);
            return Ok(factureWithItems);
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] Facture facture)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);

            var bonLivraisonIDs = facture.BonLivraisons.Select(x => x.Id);
            var originalBonLivraisons = db.BonLivraisons.Where(x => bonLivraisonIDs.Contains(x.Id));
            await originalBonLivraisons.ForEachAsync(x => x.IdFacture = facture.Id);
            facture.BonLivraisons = null;
            var numBonGenerator = new DocNumberGenerator();
            var currentYear = DateTime.Now.Year;
            var lastDoc = db.Factures.Where(x => x.Date.Year == currentYear && x.IdSite == facture.IdSite).OrderByDescending(x => x.Ref).FirstOrDefault();
            var lastRef = lastDoc != null ? lastDoc.Ref : 0;
            facture.Ref = lastRef + 1;
            facture.NumBon = numBonGenerator.getNumDocByCompany(lastRef, facture.Date);
            //-----------------------------------------------Updating payment
            var ACHAT_PAIEMENT_TYPE_ID = "399d159e-9ce0-4fcc-957a-08a65bbeecb7";
            var Total = originalBonLivraisons
                    .SelectMany(x => x.BonLivraisonItems)
                    .Sum(x => ((x.Qte * x.Pu) - (x.PercentageDiscount ? (x.Qte * x.Pu * (x.Discount ?? 0.0f) / 100) : x.Discount ?? 0.0f)) * (1 + (x.Article.TVA ?? 20) / 100));

            //espece
            var company = db.Companies.FirstOrDefault();
            var ESPECE_PAYMENT_TYPE = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2");

            if (company.UseVAT && facture?.IdTypePaiement == ESPECE_PAYMENT_TYPE)
            {
                Total *= (1 + 0.0025f);
            }

            PaiementFacture paiement = new PaiementFacture()
            {
                Id = Guid.NewGuid(),
                IdFacture = facture.Id,
                IdClient = facture.IdClient,
                Debit = Total,
                IdTypePaiement = new Guid(ACHAT_PAIEMENT_TYPE_ID),
                Date = facture.Date
            };
            db.PaiementFactures.Add(paiement);
            db.Factures.Add(facture);

            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.FactureExists(facture.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            var factureWithItems = db.Factures.Where(x => x.Id == facture.Id);
            return Ok(factureWithItems);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<Facture> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            Facture facture = await this.db.Factures.FindAsync((object)key);
            if (facture == null)
                return (IActionResult)this.NotFound();
            patch.Patch(facture);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.FactureExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<Facture>(facture);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            Facture facture = await this.db.Factures.FindAsync((object)key);
            if (facture == null)
                return NotFound();

            //dettach BLs
            var bonLivraisonIDs = facture.BonLivraisons.Select(x => x.Id);
            var originalBonLivraisons = db.BonLivraisons.Where(x => bonLivraisonIDs.Contains(x.Id));
            await originalBonLivraisons.ForEachAsync(x => x.IdFacture = null);

            db.PaiementFactures.RemoveRange(facture.PaiementFactures);
            db.FactureItems.RemoveRange(facture.FactureItems);
            this.db.Factures.Remove(facture);
            int num = await this.db.SaveChangesAsync();
            return NoContent();
        }

        [EnableQuery]
        public SingleResult<BonLivraison> GetBonLivraison([FromODataUri] Guid key)
        {
            return SingleResult.Create<BonLivraison>(this.db.Factures.Where<Facture>((Expression<Func<Facture, bool>>)(m => m.Id == key)).Select<Facture, BonLivraison>((Expression<Func<Facture, BonLivraison>>)(m => m.BonLivraison)));
        }

        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] Guid key)
        {
            return SingleResult.Create<Client>(this.db.Factures.Where<Facture>((Expression<Func<Facture, bool>>)(m => m.Id == key)).Select<Facture, Client>((Expression<Func<Facture, Client>>)(m => m.Client)));
        }

        [EnableQuery]
        public IQueryable<FactureItem> GetFactureItems([FromODataUri] Guid key)
        {
            return this.db.Factures.Where<Facture>((Expression<Func<Facture, bool>>)(m => m.Id == key)).SelectMany<Facture, FactureItem>((Expression<Func<Facture, IEnumerable<FactureItem>>>)(m => m.FactureItems));
        }

        private bool FactureExists(Guid key)
        {
            return this.db.Factures.Count<Facture>((Expression<Func<Facture, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
