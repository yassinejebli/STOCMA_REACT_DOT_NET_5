using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using STOCMA.Data;

namespace STOCMA.Controllers
{
    public class SalesHistoryController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public SalesHistoryController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ActionResult getPriceLastSale(Guid IdClient, Guid IdArticle)
        {
            var price = 0.0f;
            var client = db.Clients.Find(IdClient);
            var dateBeforeOneYear = DateTime.Now.AddYears(-1);

            if (!client.IsClientDivers)
            {
                var bi = db.BonLivraisonItems
                .Where((x => x.IdArticle == IdArticle && x.BonLivraison.IdClient == IdClient && x.BonLivraison.Date >= dateBeforeOneYear))
                .OrderByDescending(q => q.BonLivraison.Date).Take(1).FirstOrDefault();
                if (bi != null)
                    price = bi.Pu;
                else
                    price = db.Articles.Find(IdArticle).PVD ?? 0;
            }
            else
            {
                price = db.Articles.Find(IdArticle).PVD ?? 0;
            }

            return Ok(price);
        }

        public ActionResult getPriceLastPurchase(Guid IdFournisseur, Guid IdArticle)
        {
            var dateBeforeOneYear = DateTime.Now.AddYears(-1);
            var price = 0.0f;
            var bi = db.BonReceptionItems
                .Where((x => x.IdArticle == IdArticle && x.BonReception.IdFournisseur == IdFournisseur && x.BonReception.Date >= dateBeforeOneYear))
                .OrderByDescending(q => q.BonReception.Date).Take(1).FirstOrDefault();
            if (bi != null)
                price = bi.Pu;
            else
                price = db.Articles.Find(IdArticle).PA;

            return Ok(price);
        }

        public ActionResult GetArticleByBarCode(string BarCode, Guid? IdClient, int IdSite)
        {
            var dateBeforeOneYear = DateTime.Now.AddYears(-1);
            var articleSite = db.ArticleSites.FirstOrDefault(x => x.Article.BarCode == BarCode && x.IdSite == IdSite && !x.Disabled);
            if (IdClient.HasValue && articleSite != null)
            {
                var lastSoldeTime = articleSite.Article.BonLivraisonItems.Where(x => x.BonLivraison.IdClient == IdClient && x.IdArticle == articleSite.IdArticle && x.BonLivraison.Date >= dateBeforeOneYear)
                .OrderByDescending(x => x.BonLivraison.Date).Take(1).FirstOrDefault();
                if (lastSoldeTime != null)
                    articleSite.Article.PVD = lastSoldeTime.Pu;
            }
            if (articleSite == null)
                return Ok(null);

            return Ok(new
            {
                articleSite.Article.BarCode,
                articleSite.Article.Designation,
                articleSite.Article.Id,
                articleSite.Article.PVD,
                articleSite.Article.MinStock,
                articleSite.Article.PA,
                articleSite.Article.Ref,
                articleSite.QteStock,
            });
        }

        public ActionResult GetArticleAchatByBarCode(string BarCode, Guid? IdFournisseur, int IdSite)
        {
            var dateBeforeOneYear = DateTime.Now.AddYears(-1);
            var articleSite = db.ArticleSites.FirstOrDefault(x => x.Article.BarCode == BarCode && x.IdSite == IdSite && !x.Disabled);
            if (IdFournisseur.HasValue && articleSite != null)
            {
                var lastPurchasedTime = articleSite.Article.BonReceptionItems.Where(x => x.BonReception.IdFournisseur == IdFournisseur && x.IdArticle == articleSite.IdArticle && x.BonReception.Date >= dateBeforeOneYear)
                .OrderByDescending(x => x.BonReception.Date).Take(1).FirstOrDefault();
                if (lastPurchasedTime != null)
                    articleSite.Article.PA = lastPurchasedTime.Pu;
            }
            if (articleSite == null)
                return Ok(null);

            return Ok(new
            {
                articleSite.Article.BarCode,
                articleSite.Article.Designation,
                articleSite.Article.Id,
                articleSite.Article.PVD,
                articleSite.Article.MinStock,
                articleSite.Article.PA,
                articleSite.Article.Ref,
                articleSite.QteStock,
            });
        }
    }
}