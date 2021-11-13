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
    public class TarifItemsController : ODataController
    {
        private readonly ApplicationDbContext db;

        public TarifItemsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery]
        public IQueryable<TarifItem> GetTarifItems()
        {
            return (IQueryable<TarifItem>)this.db.TarifItems;
        }

        [EnableQuery]
        public SingleResult<TarifItem> GetTarifItem([FromODataUri] Guid key)
        {
            return SingleResult.Create<TarifItem>(this.db.TarifItems.Where<TarifItem>((Expression<Func<TarifItem, bool>>)(tarifItem => tarifItem.Id == key)));
        }

        public async Task<IActionResult> Put([FromODataUri] Guid key, Delta<TarifItem> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            TarifItem tarifItem = await this.db.TarifItems.FindAsync((object)key);
            if (tarifItem == null)
                return (IActionResult)this.NotFound();
            patch.Put(tarifItem);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.TarifItemExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<TarifItem>(tarifItem);
        }

        public async Task<IActionResult> Post([FromBody] TarifItem tarifItem)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            this.db.TarifItems.Add(tarifItem);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.TarifItemExists(tarifItem.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            return (IActionResult)this.Created<TarifItem>(tarifItem);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<TarifItem> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            TarifItem tarifItem = await this.db.TarifItems.FindAsync((object)key);
            if (tarifItem == null)
                return (IActionResult)this.NotFound();
            patch.Patch(tarifItem);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.TarifItemExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<TarifItem>(tarifItem);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            TarifItem async = await this.db.TarifItems.FindAsync((object)key);
            if (async == null)
                return (IActionResult)this.NotFound();
            this.db.TarifItems.Remove(async);
            int num = await this.db.SaveChangesAsync();
            return (IActionResult)this.NoContent();
        }

        [EnableQuery]
        public SingleResult<Article> GetArticle([FromODataUri] Guid key)
        {
            return SingleResult.Create<Article>(this.db.TarifItems.Where<TarifItem>((Expression<Func<TarifItem, bool>>)(m => m.Id == key)).Select<TarifItem, Article>((Expression<Func<TarifItem, Article>>)(m => m.Article)));
        }

        [EnableQuery]
        public SingleResult<Tarif> GetTarif([FromODataUri] Guid key)
        {
            return SingleResult.Create<Tarif>(this.db.TarifItems.Where<TarifItem>((Expression<Func<TarifItem, bool>>)(m => m.Id == key)).Select<TarifItem, Tarif>((Expression<Func<TarifItem, Tarif>>)(m => m.Tarif)));
        }

        private bool TarifItemExists(Guid key)
        {
            return this.db.TarifItems.Count<TarifItem>((Expression<Func<TarifItem, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
