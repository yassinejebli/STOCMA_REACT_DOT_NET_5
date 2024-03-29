﻿using System;
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
    public class BonReceptionsController : ODataController
    {
        private readonly ApplicationDbContext db;

        public BonReceptionsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET: odata/BonReceptions
        //[ClaimsAuthorizeAttribute("CanManageBonReceptions")]
        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<BonReception> GetBonReceptions()
        {
            return db.BonReceptions.OrderByDescending(x => x.Date);
        }

        // GET: odata/BonReceptions(5)
        [EnableQuery]
        public SingleResult<BonReception> GetBonReception([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.BonReceptions.Where(bonReception => bonReception.Id == key));
        }

        // PUT: odata/BonReceptions(5)
        [EnableQuery]
        public async Task<IActionResult> Put([FromODataUri] Guid key, BonReception newBonReception)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BonReception bonReception = await db.BonReceptions.FindAsync(key);
            if (bonReception == null)
            {
                return NotFound();
            }

            //----------------------------------------------Updating QteStock
            foreach (var biOld in bonReception.BonReceptionItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdSite == bonReception.IdSite && x.IdArticle == biOld.IdArticle);
                articleSite.QteStock -= biOld.Qte;
            }
            foreach (var biNew in newBonReception.BonReceptionItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdSite == bonReception.IdSite && x.IdArticle == biNew.IdArticle);
                articleSite.QteStock += biNew.Qte;
            }

            //-----------------------------------------------Updating document items
            db.BonReceptionItems.RemoveRange(bonReception.BonReceptionItems);
            db.BonReceptionItems.AddRange(newBonReception.BonReceptionItems);
            bonReception.ModificationDate = DateTime.Now;
            bonReception.Date = newBonReception.Date;
            bonReception.NumBon = newBonReception.NumBon;

            //-----------------------------------------------Updating payment
            var payment = db.PaiementFs.FirstOrDefault(x => x.IdBonReception == bonReception.Id);
            var ACHAT_PAIEMENT_TYPE_ID = "399d159e-9ce0-4fcc-957a-08a65bbeecb7";
            var Total = newBonReception.BonReceptionItems.Sum(x => x.Qte * x.Pu);
            if (payment != null)
            {
                payment.Debit = Total;
                payment.Date = newBonReception.Date;
            }
            else
            {
                PaiementF paiement = new PaiementF()
                {
                    Id = Guid.NewGuid(),
                    IdBonReception = bonReception.Id,
                    IdFournisseur = newBonReception.IdFournisseur,
                    Debit = Total,
                    IdTypePaiement = new Guid(ACHAT_PAIEMENT_TYPE_ID),
                    Date = newBonReception.Date
                };
                db.PaiementFs.Add(paiement);
            }
            ////////////////////////////////////////////


            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BonReceptionExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var bonReceptionWithItems = db.BonReceptions.Where(x => x.Id == bonReception.Id);
            return Ok(bonReceptionWithItems);
            //return Updated(bonReception);
        }

        // POST: odata/BonReceptions
        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] BonReception bonReception)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            this.db.BonReceptions.Add(bonReception);

            //-----------------------------------------------Updating payment
            var ACHAT_PAIEMENT_TYPE_ID = "399d159e-9ce0-4fcc-957a-08a65bbeecb7";
            var Total = bonReception.BonReceptionItems.Sum(x => x.Qte * x.Pu);

            PaiementF paiement = new PaiementF()
            {
                Id = Guid.NewGuid(),
                IdBonReception = bonReception.Id,
                IdFournisseur = bonReception.IdFournisseur,
                Debit = Total,
                IdTypePaiement = new Guid(ACHAT_PAIEMENT_TYPE_ID),
                Date = bonReception.Date
            };
            db.PaiementFs.Add(paiement);

            //-------------------------------------------updating QteStock
            foreach (var bi in bonReception.BonReceptionItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdArticle == bi.IdArticle && x.IdSite == bonReception.IdSite);
                if (articleSite.QteStock <= 0)
                {
                    articleSite.Article.PA = bi.Pu;
                }
                else
                {
                    //PUMP
                    var PA = (Decimal)(((articleSite.QteStock * articleSite.Article.PA) + (bi.Qte * bi.Pu)) / (articleSite.QteStock + bi.Qte));
                    articleSite.Article.PA = (float)Math.Round(PA, 2, MidpointRounding.AwayFromZero);
                }
                articleSite.QteStock += bi.Qte;
                articleSite.Disabled = false;
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BonReceptionExists(bonReception.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            var bonReceptionWithItems = db.BonReceptions.Where(x => x.Id == bonReception.Id);
            return Ok(bonReceptionWithItems);
            //return Created(bonReception);
        }

        // PATCH: odata/BonReceptions(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<BonReception> patch)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BonReception bonReception = await db.BonReceptions.FindAsync(key);
            if (bonReception == null)
            {
                return NotFound();
            }

            patch.Patch(bonReception);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BonReceptionExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(bonReception);
        }

        // DELETE: odata/BonReceptions(5)
        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            BonReception bonReception = await db.BonReceptions.FindAsync(key);
            if (bonReception == null)
            {
                return NotFound();
            }

            foreach (var bi in bonReception.BonReceptionItems)
            {
                var articleSite = db.ArticleSites.FirstOrDefault(x => x.IdSite == bonReception.IdSite && x.IdArticle == bi.IdArticle);
                articleSite.QteStock -= bi.Qte;
            }

            db.PaiementFs.RemoveRange(bonReception.PaiementFs);
            db.BonReceptionItems.RemoveRange(bonReception.BonReceptionItems);
            db.BonReceptions.Remove(bonReception);
            await db.SaveChangesAsync();

            return NoContent();
        }

        // GET: odata/BonReceptions(5)/BonAvoirs
        [EnableQuery]
        public IQueryable<BonAvoir> GetBonAvoirs([FromODataUri] Guid key)
        {
            return db.BonReceptions.Where(m => m.Id == key).SelectMany(m => m.BonAvoirs);
        }

        // GET: odata/BonReceptions(5)/BonReceptionItems
        [EnableQuery]
        public IQueryable<BonReceptionItem> GetBonReceptionItems([FromODataUri] Guid key)
        {
            return db.BonReceptions.Where(m => m.Id == key).SelectMany(m => m.BonReceptionItems);
        }

        // GET: odata/BonReceptions(5)/FactureF
        [EnableQuery]
        public SingleResult<FactureF> GetFactureF([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.BonReceptions.Where(m => m.Id == key).Select(m => m.FactureF));
        }

        // GET: odata/BonReceptions(5)/Fournisseur
        [EnableQuery]
        public SingleResult<Fournisseur> GetFournisseur([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.BonReceptions.Where(m => m.Id == key).Select(m => m.Fournisseur));
        }

        // GET: odata/BonReceptions(5)/PaiementFs
        [EnableQuery]
        public IQueryable<PaiementF> GetPaiementFs([FromODataUri] Guid key)
        {
            return db.BonReceptions.Where(m => m.Id == key).SelectMany(m => m.PaiementFs);
        }

        private bool BonReceptionExists(Guid key)
        {
            return db.BonReceptions.Count(e => e.Id == key) > 0;
        }
    }
}
