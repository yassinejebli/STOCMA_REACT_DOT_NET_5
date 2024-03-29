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

namespace STOCMA.Controllers
{

    //[Authorize]
    public class ArticlesController : ODataController
    {
        private readonly ApplicationDbContext db;

        public ArticlesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET: odata/Articles
        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<Article> GetArticles()
        {
            return db.Articles.Include(x => x.ArticleSites).OrderBy(x => x.Description);
        }

        [EnableQuery]
        public IQueryable<Article> ArticlesGaz()
        {
            return db.Articles.Where(x => x.Ref.StartsWith("GZ"));
        }

        // GET: odata/Articles(5)
        [EnableQuery]
        public SingleResult<Article> GetArticle([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Articles.Where(article => article.Id == key));
        }

        //[EnableQuery]
        // PUT: odata/Articles(5)
        //[Authorize(Roles = "Admin")]
        //[ClaimsAuthorizeAttribute(type: "CanUpdateQteStock")]
        public IActionResult Put([FromODataUri] Guid key, float QteStock, int IdSite, bool Disabled, Delta<Article> patch)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Article article = db.Articles.Find(key);
            if (article == null)
            {
                return NotFound();
            }

            var articleSite = db.ArticleSites.Where(x => x.IdSite == IdSite && x.IdArticle == key).FirstOrDefault();
            articleSite.QteStock = QteStock;
            articleSite.Disabled = Disabled;
            patch.Put(article);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(article);
        }

        // POST: odata/Articles
        public IActionResult Post([FromBody] Article article, float QteStock, int IdSite = 1)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (Site site in db.Sites)
            {
                db.ArticleSites.Add(new ArticleSite
                {
                    IdArticle = article.Id,
                    IdSite = site.Id,
                    QteStock = site.Id != IdSite ? 0 : QteStock
                });
            }
            if (article.Ref == null || article.Ref == "")
                article.Ref = String.Format("A{0:00000}", article.RefAuto);
            db.Articles.Add(article);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ArticleExists(article.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(article);
        }

        // PATCH: odata/Articles(5)
        //[Authorize(Roles = "Admin")]
        //[ClaimsAuthorizeAttribute(type: "CanUpdateQteStock")]
        //[ClaimsAuthorization(ClaimType = "CanUpdateQteStock")]
        [AcceptVerbs("PATCH", "MERGE")]
        public IActionResult Patch([FromODataUri] Guid key, Delta<Article> patch, float QteStock, bool Disabled, int IdSite = 1)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Article article = db.Articles.Find(key);
            var articleSite = db.ArticleSites.Where(x => x.IdSite == IdSite && x.IdArticle == key).FirstOrDefault();
            articleSite.QteStock = QteStock;
            articleSite.Disabled = Disabled;

            if (article == null)
            {
                return NotFound();
            }

            patch.Patch(article);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(article);
        }

        // DELETE: odata/Articles(5)
        //public IActionResult Delete([FromODataUri] Guid key)
        //{
        //    Article article = db.Articles.Find(key);
        //    if (article == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Articles.Remove(article);
        //    db.SaveChanges();

        //    return NoContent();
        //}

        // GET: odata/Articles(5)/ArticleSites
        [EnableQuery]
        public IQueryable<ArticleSite> GetArticleSites([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.ArticleSites);
        }

        // GET: odata/Articles(5)/BonAvoirCItems
        [EnableQuery]
        public IQueryable<BonAvoirCItem> GetBonAvoirCItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.BonAvoirCItems);
        }

        // GET: odata/Articles(5)/BonAvoirItems
        [EnableQuery]
        public IQueryable<BonAvoirItem> GetBonAvoirItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.BonAvoirItems);
        }

        // GET: odata/Articles(5)/BonCommandeItems
        [EnableQuery]
        public IQueryable<BonCommandeItem> GetBonCommandeItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.BonCommandeItems);
        }

        // GET: odata/Articles(5)/BonLivraisonItems
        [EnableQuery]
        public IQueryable<BonLivraisonItem> GetBonLivraisonItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.BonLivraisonItems);
        }

        // GET: odata/Articles(5)/BonReceptionItems
        [EnableQuery]
        public IQueryable<BonReceptionItem> GetBonReceptionItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.BonReceptionItems);
        }

        // GET: odata/Articles(5)/Categorie
        [EnableQuery]
        public SingleResult<Categorie> GetCategorie([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Articles.Where(m => m.Id == key).Select(m => m.Categorie));
        }

        // GET: odata/Articles(5)/DevisItems
        [EnableQuery]
        public IQueryable<DevisItem> GetDevisItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.DevisItems);
        }

        // GET: odata/Articles(5)/DgbFItems
        [EnableQuery]
        public IQueryable<DgbFItem> GetDgbFItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.DgbFItems);
        }

        // GET: odata/Articles(5)/DgbItems
        [EnableQuery]
        public IQueryable<DgbItem> GetDgbItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.DgbItems);
        }

        // GET: odata/Articles(5)/FactureFItems
        [EnableQuery]
        public IQueryable<FactureFItem> GetFactureFItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.FactureFItems);
        }

        // GET: odata/Articles(5)/FactureItems
        [EnableQuery]
        public IQueryable<FactureItem> GetFactureItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.FactureItems);
        }

        // GET: odata/Articles(5)/RdbFItems
        [EnableQuery]
        public IQueryable<RdbFItem> GetRdbFItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.RdbFItems);
        }

        // GET: odata/Articles(5)/RdbItems
        [EnableQuery]
        public IQueryable<RdbItem> GetRdbItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.RdbItems);
        }

        // GET: odata/Articles(5)/TarifItems
        [EnableQuery]
        public IQueryable<TarifItem> GetTarifItems([FromODataUri] Guid key)
        {
            return db.Articles.Where(m => m.Id == key).SelectMany(m => m.TarifItems);
        }

        private bool ArticleExists(Guid key)
        {
            return db.Articles.Count(e => e.Id == key) > 0;
        }
    }
}
