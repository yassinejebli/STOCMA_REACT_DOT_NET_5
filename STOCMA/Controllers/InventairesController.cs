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
    public class InventairesController : ODataController
    {
        private readonly ApplicationDbContext db;

        public InventairesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<Inventaire> GetInventaires()
        {
            return (IQueryable<Inventaire>)this.db.Inventaires.OrderByDescending(x => x.Date);
        }

        [EnableQuery]
        public SingleResult<Inventaire> GetInventaire([FromODataUri] Guid key)
        {
            return SingleResult.Create<Inventaire>(this.db.Inventaires.Where<Inventaire>((Expression<Func<Inventaire, bool>>)(inventaire => inventaire.Id == key)));
        }

        //[EnableQuery]
        //public async Task<IActionResult> Put([FromODataUri] Guid key, Inventaire newInventaire)
        //{
        //    if (!this.ModelState.IsValid)
        //        return (IActionResult)this.BadRequest(this.ModelState);
        //    Inventaire inventaire = await this.db.Inventaires.FindAsync((object)key);
        //    if (inventaire == null)
        //        return (IActionResult)this.NotFound();

        //    inventaire.Date = newInventaire.Date;
        //    inventaire.Titre = newInventaire.Titre;
        //    //-----------------------------------------------Updating document items
        //    db.InventaireItems.RemoveRange(inventaire.InventaireItems);
        //    db.InventaireItems.AddRange(newInventaire.InventaireItems);
        //    try
        //    {
        //        int num = await this.db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        if (!this.InventaireExists(key))
        //            return (IActionResult)this.NotFound();
        //        throw;
        //    }
        //    var inventaireWithItems = db.Inventaires.Where(x => x.Id == inventaire.Id);
        //    return Content(HttpStatusCode.Created, SingleResult.Create(inventaireWithItems));
        //}

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] Inventaire inventaire)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);

            //Updating stock
            inventaire.InventaireItems.ToList().ForEach(x =>
            {
                var articleSite = db.ArticleSites.FirstOrDefault(y => y.IdSite == inventaire.IdSite && x.IdArticle == y.IdArticle);
                articleSite.QteStock = x.QteStockReel;
                if (x.IdCategory != null)
                    articleSite.Article.IdCategorie = x.IdCategory;
                //articleSite.Article.IdCategorie = "";
            });

            this.db.Inventaires.Add(inventaire);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.InventaireExists(inventaire.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            var inventaireWithItems = db.Inventaires.Where(x => x.Id == inventaire.Id);
            return Ok(inventaireWithItems);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<Inventaire> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            Inventaire inventaire = await this.db.Inventaires.FindAsync((object)key);
            if (inventaire == null)
                return (IActionResult)this.NotFound();
            patch.Patch(inventaire);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.InventaireExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<Inventaire>(inventaire);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            Inventaire async = await this.db.Inventaires.FindAsync((object)key);
            if (async == null)
                return (IActionResult)this.NotFound();

            db.InventaireItems.RemoveRange(async.InventaireItems);
            this.db.Inventaires.Remove(async);
            int num = await this.db.SaveChangesAsync();
            return (IActionResult)this.NoContent();
        }

        [EnableQuery]
        public IQueryable<InventaireItem> GetInventaireITems([FromODataUri] Guid key)
        {
            return this.db.Inventaires.Where(m => m.Id == key).SelectMany(m => m.InventaireItems);
        }

        private bool InventaireExists(Guid key)
        {
            return this.db.Inventaires.Count<Inventaire>((Expression<Func<Inventaire, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
