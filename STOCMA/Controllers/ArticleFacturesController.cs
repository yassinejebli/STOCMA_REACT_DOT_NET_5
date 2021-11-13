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

namespace STOCMA.Controllers
{
    public class ArticleFacturesController : ODataController
    {
        private readonly ApplicationDbContext db;

        public ArticleFacturesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<ArticleFacture> GetArticleFactures()
        {
            return (IQueryable<ArticleFacture>)this.db.ArticleFactures.OrderByDescending(x => x.QteStock);
        }

        [EnableQuery]
        public SingleResult<ArticleFacture> GetArticleFacture([FromODataUri] Guid key)
        {
            return SingleResult.Create<ArticleFacture>(this.db.ArticleFactures.Where<ArticleFacture>((Expression<Func<ArticleFacture, bool>>)(articleFacture => articleFacture.Id == key)));
        }

        public async Task<IActionResult> Put([FromODataUri] Guid key, Delta<ArticleFacture> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            ArticleFacture articleFacture = await this.db.ArticleFactures.FindAsync((object)key);
            if (articleFacture == null)
                return (IActionResult)this.NotFound();
            patch.Put(articleFacture);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.ArticleFactureExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<ArticleFacture>(articleFacture);
        }

        public async Task<IActionResult> Post([FromBody] ArticleFacture articleFacture)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            this.db.ArticleFactures.Add(articleFacture);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.ArticleFactureExists(articleFacture.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            return (IActionResult)this.Created<ArticleFacture>(articleFacture);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<ArticleFacture> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            ArticleFacture articleFacture = await this.db.ArticleFactures.FindAsync((object)key);
            if (articleFacture == null)
                return (IActionResult)this.NotFound();
            patch.Patch(articleFacture);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.ArticleFactureExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<ArticleFacture>(articleFacture);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            ArticleFacture async = await this.db.ArticleFactures.FindAsync((object)key);
            if (async == null)
                return (IActionResult)this.NotFound();
            this.db.ArticleFactures.Remove(async);
            int num = await this.db.SaveChangesAsync();
            return (IActionResult)this.NoContent();
        }

        [EnableQuery]
        public IQueryable<FakeFactureItem> GetFakeFactureItems([FromODataUri] Guid key)
        {
            return this.db.ArticleFactures.Where<ArticleFacture>((Expression<Func<ArticleFacture, bool>>)(m => m.Id == key)).SelectMany<ArticleFacture, FakeFactureItem>((Expression<Func<ArticleFacture, IEnumerable<FakeFactureItem>>>)(m => m.FakeFactureItems));
        }

        private bool ArticleFactureExists(Guid key)
        {
            return this.db.ArticleFactures.Count<ArticleFacture>((Expression<Func<ArticleFacture, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
