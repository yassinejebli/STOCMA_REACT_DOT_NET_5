﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize]
    public class BonLivraisonsController : ODataController
    {

        private readonly ApplicationDbContext db;

        public BonLivraisonsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<BonLivraison> GetBonLivraisons()
        {
            return db.BonLivraisons.OrderByDescending(x => x.Date.Year).ThenByDescending(x => x.Ref);
        }

        [EnableQuery(MaxExpansionDepth = 5)]
        public SingleResult<BonLivraison> GetBonLivraison([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.BonLivraisons.Where<BonLivraison>((Expression<Func<BonLivraison, bool>>)(bonLivraison => bonLivraison.Id == key)));
        }

        [EnableQuery]
        public async Task<IActionResult> Put([FromODataUri] Guid key, BonLivraison newBonLivraison)
        {
            BonLivraison bonLivraison = await db.BonLivraisons.FindAsync(key);
            if (bonLivraison == null)
                return NotFound();

            var oldNumBon = bonLivraison.NumBon;
            /////////////////////////////////////////////
            //----------------------------------------------Updating QteStock
            foreach (var biOld in bonLivraison.BonLivraisonItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdSite == biOld.IdSite && x.IdArticle == biOld.IdArticle);
                articleSite.QteStock += biOld.Qte;
            }
            foreach (var biNew in newBonLivraison.BonLivraisonItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdSite == biNew.IdSite && x.IdArticle == biNew.IdArticle);
                articleSite.QteStock -= biNew.Qte;
            }


            //-----------------------------------------------Updating document items
            db.BonLivraisonItems.RemoveRange(bonLivraison.BonLivraisonItems);
            db.BonLivraisonItems.AddRange(newBonLivraison.BonLivraisonItems);

            bonLivraison.ModificationDate = DateTime.Now;
            bonLivraison.Date = newBonLivraison.Date;
            bonLivraison.IdSite = newBonLivraison.IdSite;
            bonLivraison.Ref = newBonLivraison.Ref;
            bonLivraison.Note = newBonLivraison.Note;
            bonLivraison.IdTypePaiement = newBonLivraison.IdTypePaiement;
            bonLivraison.WithDiscount = newBonLivraison.WithDiscount;
            var numBonGenerator = new DocNumberGenerator(db);

            bonLivraison.NumBon = numBonGenerator.getNumDocByCompany(newBonLivraison.Ref - 1, newBonLivraison.Date);
            foreach (var bi in newBonLivraison.BonLivraisonItems)
            {
                var article = db.Articles.Find(bi.IdArticle);
                bi.PA = article.PA;
            }

            //-----------------------------------------------Updating payment
            var payment = db.Paiements.FirstOrDefault(x => x.IdBonLivraison == bonLivraison.Id);
            var ACHAT_PAIEMENT_TYPE_ID = "399d159e-9ce0-4fcc-957a-08a65bbeecb6";
            var Total = newBonLivraison.BonLivraisonItems.Sum(x => (x.Qte * x.Pu) - (x.PercentageDiscount ? (x.Qte * x.Pu * (x.Discount ?? 0.0f) / 100) : x.Discount ?? 0.0f));

            //Checking solde
            //var client = db.Clients.Find(bonLivraison.IdClient);
            //var hasSurpassedPlafond = !client.IsClientDivers && client.Plafond != 0 && (client.Solde + Total) > client.Plafond;
            //if (hasSurpassedPlafond)
            //    return StatusCode(HttpStatusCode.NotAcceptable);

            if (payment != null)
            {
                payment.Debit = Total;
                payment.Date = newBonLivraison.Date;
            }
            else
            {
                Paiement paiement = new Paiement()
                {
                    Id = Guid.NewGuid(),
                    IdBonLivraison = bonLivraison.Id,
                    IdClient = newBonLivraison.IdClient,
                    Debit = Total,
                    IdTypePaiement = new Guid(ACHAT_PAIEMENT_TYPE_ID),
                    Date = newBonLivraison.Date,
                };
                db.Paiements.Add(paiement);
            }

            if (db.Clients.Find(bonLivraison.IdClient).IsClientDivers)
            {
                var ESPECE_PAIEMENT_TYPE_ID = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2");
                var paymentClientDivers = db.Paiements.FirstOrDefault(x => x.Comment == "BL " + oldNumBon);
                if (paymentClientDivers != null)
                {
                    paymentClientDivers.Credit = Total;
                    paymentClientDivers.Date = newBonLivraison.Date;
                    paymentClientDivers.Comment = "BL " + newBonLivraison.NumBon;
                    paymentClientDivers.IdTypePaiement = newBonLivraison.IdTypePaiement ?? ESPECE_PAIEMENT_TYPE_ID;
                }
                else
                {
                    Paiement paiementClientDivers = new Paiement()
                    {
                        Id = Guid.NewGuid(),
                        //IdBonLivraison = bonLivraison.Id,
                        IdClient = bonLivraison.IdClient,
                        Credit = Total,
                        IdTypePaiement = newBonLivraison.IdTypePaiement ?? ESPECE_PAIEMENT_TYPE_ID,
                        Date = bonLivraison.Date,
                        Comment = "BL " + newBonLivraison.NumBon,
                    };
                    db.Paiements.Add(paiementClientDivers);
                }

            }
            ////////////////////////////////////////////


            try
            {
                await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.BonLivraisonExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }

            var bonLivraisonWithItems = db.BonLivraisons.First(x => x.Id == bonLivraison.Id);
            return Ok(bonLivraisonWithItems);
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] BonLivraison bonLivraison)
        {
            if (!this.ModelState.IsValid)
                return BadRequest(this.ModelState);
            var numBonGenerator = new DocNumberGenerator(db);
            var currentYear = DateTime.Now.Year;
            var lastDoc = db.BonLivraisons.Where(x => x.Date.Year == currentYear).OrderByDescending(x => x.Ref).FirstOrDefault();
            var lastRef = lastDoc != null ? lastDoc.Ref : 0;
            bonLivraison.User = User.Identity.Name;
            bonLivraison.IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bonLivraison.Ref = lastRef + 1;
            foreach (var bi in bonLivraison.BonLivraisonItems)
            {
                var article = db.Articles.Find(bi.IdArticle);
                bi.PA = article.PA;
            }
            bonLivraison.NumBon = numBonGenerator.getNumDocByCompany(lastRef, bonLivraison.Date);
            this.db.BonLivraisons.Add(bonLivraison);

            //-----------------------------------------------Updating payment
            var payment = db.Paiements.FirstOrDefault(x => x.IdBonLivraison == bonLivraison.Id);
            var ACHAT_PAIEMENT_TYPE_ID = "399d159e-9ce0-4fcc-957a-08a65bbeecb6";
            var Total = bonLivraison.BonLivraisonItems.Sum(x => (x.Qte * x.Pu) - (x.PercentageDiscount ? (x.Qte * x.Pu * (x.Discount ?? 0.0f) / 100) : x.Discount ?? 0.0f));


            //Check solde
            var client = db.Clients.Find(bonLivraison.IdClient);
            var hasSurpassedPlafond = !client.IsClientDivers && client.Plafond != 0 && client.Solde > client.Plafond;
            if (hasSurpassedPlafond)
                return StatusCode(406); //HttpStatusCode.NotAcceptable

            if (payment != null)
            {
                payment.Debit = Total;
                payment.Date = bonLivraison.Date;
            }
            else
            {
                Paiement paiement = new Paiement()
                {
                    Id = Guid.NewGuid(),
                    IdBonLivraison = bonLivraison.Id,
                    IdClient = bonLivraison.IdClient,
                    Debit = Total,
                    IdTypePaiement = new Guid(ACHAT_PAIEMENT_TYPE_ID),
                    Date = bonLivraison.Date
                };
                db.Paiements.Add(paiement);

                if (db.Clients.Find(bonLivraison.IdClient).IsClientDivers)
                {
                    var ESPECE_PAIEMENT_TYPE_ID = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2");
                    Paiement paiementClientDivers = new Paiement()
                    {
                        Id = Guid.NewGuid(),
                        //IdBonLivraison = bonLivraison.Id,
                        IdClient = bonLivraison.IdClient,
                        Credit = Total,
                        IdTypePaiement = bonLivraison.IdTypePaiement ?? ESPECE_PAIEMENT_TYPE_ID,
                        Date = bonLivraison.Date,
                        Comment = "BL " + bonLivraison.NumBon,
                    };
                    db.Paiements.Add(paiementClientDivers);
                }

            }

            //-------------------------------------------updating QteStock
            foreach (var bi in bonLivraison.BonLivraisonItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdArticle == bi.IdArticle && x.IdSite == bi.IdSite);
                articleSite.QteStock -= bi.Qte;
                articleSite.Counter += 1;
            }
            //-------------------------------------------

            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.BonLivraisonExists(bonLivraison.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            var bonLivraisonWithItems = db.BonLivraisons.First(x => x.Id == bonLivraison.Id);
            return Ok(bonLivraisonWithItems);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            BonLivraison async = await this.db.BonLivraisons.FindAsync((object)key);
            if (async == null)
                return (IActionResult)this.NotFound();

            //--------------------------updating QteStock
            foreach (var bi in async.BonLivraisonItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdSite == bi.IdSite && x.IdArticle == bi.IdArticle);
                articleSite.QteStock += bi.Qte;
            }

            db.Paiements.RemoveRange(async.Paiements);

            if (db.Clients.Find(async.IdClient).IsClientDivers)
            {
                var ESPECE_PAIEMENT_TYPE_ID = new Guid("399d159e-9ce0-4fcc-957a-08a65bbeecb2");
                var paymentClientDivers = db.Paiements.FirstOrDefault(x => x.Comment == "BL " + async.NumBon);
                if (paymentClientDivers != null)
                {
                    db.Paiements.Remove(paymentClientDivers);
                }
            }

            db.BonLivraisonItems.RemoveRange(async.BonLivraisonItems);
            db.BonLivraisons.Remove(async);
            int num = await db.SaveChangesAsync();
            return NoContent();
        }

        [EnableQuery]
        public IQueryable<BonLivraisonItem> GetBonLivraisonItems([FromODataUri] Guid key)
        {
            return this.db.BonLivraisons.Where<BonLivraison>((Expression<Func<BonLivraison, bool>>)(m => m.Id == key)).SelectMany<BonLivraison, BonLivraisonItem>((Expression<Func<BonLivraison, IEnumerable<BonLivraisonItem>>>)(m => m.BonLivraisonItems));
        }

        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] Guid key)
        {
            return SingleResult.Create<Client>(this.db.BonLivraisons.Where<BonLivraison>((Expression<Func<BonLivraison, bool>>)(m => m.Id == key)).Select<BonLivraison, Client>((Expression<Func<BonLivraison, Client>>)(m => m.Client)));
        }

        private bool BonLivraisonExists(Guid key)
        {
            return this.db.BonLivraisons.Count<BonLivraison>((Expression<Func<BonLivraison, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
