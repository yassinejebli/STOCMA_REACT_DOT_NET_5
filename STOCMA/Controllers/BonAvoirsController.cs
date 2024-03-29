﻿using System;
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
    public class BonAvoirsController : ODataController
    {
        private readonly ApplicationDbContext db;

        public BonAvoirsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<BonAvoir> GetBonAvoirs()
        {
            return (IQueryable<BonAvoir>)this.db.BonAvoirs.OrderByDescending(x => x.Date);
        }

        [EnableQuery]
        public SingleResult<BonAvoir> GetBonAvoir([FromODataUri] Guid key)
        {
            return SingleResult.Create<BonAvoir>(this.db.BonAvoirs.Where<BonAvoir>((Expression<Func<BonAvoir, bool>>)(bonAvoir => bonAvoir.Id == key)));
        }

        [EnableQuery]
        public async Task<IActionResult> Put([FromODataUri] Guid key, BonAvoir newBonAvoir)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            BonAvoir bonAvoir = await db.BonAvoirs.FindAsync((object)key);
            if (bonAvoir == null)
                return (IActionResult)this.NotFound();

            //----------------------------------------------Updating QteStock
            foreach (var biOld in bonAvoir.BonAvoirItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdSite == biOld.IdSite && x.IdArticle == biOld.IdArticle);
                articleSite.QteStock += biOld.Qte;
            }
            foreach (var biNew in newBonAvoir.BonAvoirItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdSite == biNew.IdSite && x.IdArticle == biNew.IdArticle);
                articleSite.QteStock -= biNew.Qte;
            }


            //-----------------------------------------------Updating document items
            db.BonAvoirItems.RemoveRange(bonAvoir.BonAvoirItems);
            db.BonAvoirItems.AddRange(newBonAvoir.BonAvoirItems);

            bonAvoir.Date = newBonAvoir.Date;
            bonAvoir.NumBon = newBonAvoir.NumBon;
            bonAvoir.IdSite = newBonAvoir.IdSite;


            //-----------------------------------------------Updating payment
            var company = db.Companies.FirstOrDefault();
            var AVOIR_PAIEMENT_TYPE_ID = "399d159e-9ce0-4fcc-957a-08a65bbeecb8";
            if (company.UseVAT)
            {
                var Total = newBonAvoir.BonAvoirItems.Sum(x => (x.Qte * x.Pu) * (1 + (db.Articles.Find(x.IdArticle).TVA ?? 20) / 100));
                var payment = db.PaiementFactureFs.FirstOrDefault(x => x.IdBonAvoir == bonAvoir.Id);
                if (payment != null)
                {
                    payment.Credit = Total;
                    payment.Date = bonAvoir.Date;
                    payment.Comment = "Avoir " + bonAvoir.NumBon;
                }
                else
                {
                    PaiementFactureF paiement = new PaiementFactureF()
                    {
                        Id = Guid.NewGuid(),
                        IdBonAvoir = newBonAvoir.Id,
                        IdFournisseur = newBonAvoir.IdFournisseur,
                        Credit = Total,
                        IdTypePaiement = new Guid(AVOIR_PAIEMENT_TYPE_ID),
                        Date = newBonAvoir.Date,
                        Comment = "Avoir " + newBonAvoir.NumBon
                    };
                    db.PaiementFactureFs.Add(paiement);
                }
            }
            else
            {
                var payment = db.PaiementFs.FirstOrDefault(x => x.IdBonAvoir == bonAvoir.Id);
                var Total = newBonAvoir.BonAvoirItems.Sum(x => x.Qte * x.Pu);
                if (payment != null)
                {
                    payment.Credit = Total;
                    payment.Date = bonAvoir.Date;
                    payment.Comment = "Avoir " + bonAvoir.NumBon;
                }
                else
                {
                    PaiementF paiement = new PaiementF()
                    {
                        Id = Guid.NewGuid(),
                        IdBonAvoir = newBonAvoir.Id,
                        IdFournisseur = newBonAvoir.IdFournisseur,
                        Credit = Total,
                        IdTypePaiement = new Guid(AVOIR_PAIEMENT_TYPE_ID),
                        Date = newBonAvoir.Date,
                        Comment = "Avoir " + newBonAvoir.NumBon
                    };
                    db.PaiementFs.Add(paiement);
                }
            }
            ////////////////////////////////////////////


            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.BonAvoirExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            var bonAvoirWithItems = db.BonAvoirs.Where(x => x.Id == bonAvoir.Id);
            return Ok(bonAvoirWithItems);
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] BonAvoir bonAvoir)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);


            //---------------------------Updating Qte stock
            foreach (var bi in bonAvoir.BonAvoirItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdArticle == bi.IdArticle && x.IdSite == bi.IdSite);
                articleSite.QteStock -= bi.Qte;
            }

            //----------------------------Transaction
            var company = db.Companies.FirstOrDefault();
            var AVOIR_PAIEMENT_TYPE_ID = "399d159e-9ce0-4fcc-957a-08a65bbeecb8";

            if (company.UseVAT)
            {
                var Total = bonAvoir.BonAvoirItems.Sum(x => (x.Qte * x.Pu) * (1 + (db.Articles.Find(x.IdArticle).TVA ?? 20) / 100));
                PaiementFactureF paiement = new PaiementFactureF()
                {
                    Id = Guid.NewGuid(),
                    IdBonAvoir = bonAvoir.Id,
                    IdFournisseur = bonAvoir.IdFournisseur,
                    Credit = Total,
                    IdTypePaiement = new Guid(AVOIR_PAIEMENT_TYPE_ID),
                    Date = bonAvoir.Date,
                    Comment = "Avoir " + bonAvoir.NumBon
                };
                db.PaiementFactureFs.Add(paiement);
            }
            else
            {
                var Total = bonAvoir.BonAvoirItems.Sum(x => x.Qte * x.Pu);
                PaiementF paiement = new PaiementF()
                {
                    Id = Guid.NewGuid(),
                    IdBonAvoir = bonAvoir.Id,
                    IdFournisseur = bonAvoir.IdFournisseur,
                    Credit = Total,
                    IdTypePaiement = new Guid(AVOIR_PAIEMENT_TYPE_ID),
                    Date = bonAvoir.Date,
                    Comment = "Avoir " + bonAvoir.NumBon
                };
                db.PaiementFs.Add(paiement);
            }

            db.BonAvoirs.Add(bonAvoir);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.BonAvoirExists(bonAvoir.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            var bonAvoirWithItems = db.BonAvoirs.Where(x => x.Id == bonAvoir.Id);
            return Ok(bonAvoirWithItems);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<BonAvoir> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            BonAvoir bonAvoir = await this.db.BonAvoirs.FindAsync((object)key);
            if (bonAvoir == null)
                return (IActionResult)this.NotFound();
            patch.Patch(bonAvoir);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.BonAvoirExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated(bonAvoir);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            BonAvoir async = await db.BonAvoirs.FindAsync((object)key);
            if (async == null)
                return (IActionResult)this.NotFound();

            //--------------------------updating QteStock
            foreach (var bi in async.BonAvoirItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdSite == bi.IdSite && x.IdArticle == bi.IdArticle);
                articleSite.QteStock += bi.Qte;
            }

            db.PaiementFs.RemoveRange(async.PaiementFs);
            db.PaiementFactureFs.RemoveRange(async.PaiementFactureFs);
            db.BonAvoirItems.RemoveRange(async.BonAvoirItems);
            db.BonAvoirs.Remove(async);
            await db.SaveChangesAsync();
            return NoContent();
        }

        [EnableQuery]
        public IQueryable<BonAvoirItem> GetBonAvoirItems([FromODataUri] Guid key)
        {
            return this.db.BonAvoirs.Where<BonAvoir>((Expression<Func<BonAvoir, bool>>)(m => m.Id == key)).SelectMany<BonAvoir, BonAvoirItem>((Expression<Func<BonAvoir, IEnumerable<BonAvoirItem>>>)(m => m.BonAvoirItems));
        }

        [EnableQuery]
        public SingleResult<BonReception> GetBonReception([FromODataUri] Guid key)
        {
            return SingleResult.Create<BonReception>(this.db.BonAvoirs.Where<BonAvoir>((Expression<Func<BonAvoir, bool>>)(m => m.Id == key)).Select<BonAvoir, BonReception>((Expression<Func<BonAvoir, BonReception>>)(m => m.BonReception)));
        }

        [EnableQuery]
        public SingleResult<Fournisseur> GetFournisseur([FromODataUri] Guid key)
        {
            return SingleResult.Create<Fournisseur>(this.db.BonAvoirs.Where<BonAvoir>((Expression<Func<BonAvoir, bool>>)(m => m.Id == key)).Select<BonAvoir, Fournisseur>((Expression<Func<BonAvoir, Fournisseur>>)(m => m.Fournisseur)));
        }


        private bool BonAvoirExists(Guid key)
        {
            return this.db.BonAvoirs.Count<BonAvoir>((Expression<Func<BonAvoir, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
