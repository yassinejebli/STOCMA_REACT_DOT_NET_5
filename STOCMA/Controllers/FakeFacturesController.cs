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
    public class FakeFacturesController : ODataController
    {
        private readonly ApplicationDbContext db;

        public FakeFacturesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<FakeFacture> GetFakeFactures()
        {
            return (IQueryable<FakeFacture>)this.db.FakeFactures.OrderByDescending(x => new { x.Date.Year, x.Ref });
        }

        [EnableQuery]
        public SingleResult<FakeFacture> GetFakeFacture([FromODataUri] Guid key)
        {
            return SingleResult.Create<FakeFacture>(this.db.FakeFactures.Where<FakeFacture>((Expression<Func<FakeFacture, bool>>)(fakeFacture => fakeFacture.Id == key)));
        }

        // article duplicated issue and QteStock > 0
        [EnableQuery]
        public async Task<IActionResult> Put([FromODataUri] Guid key, FakeFacture newFakeFacture)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            FakeFacture fakeFacture = await this.db.FakeFactures.FindAsync((object)key);
            if (fakeFacture == null)
                return (IActionResult)this.NotFound();


            fakeFacture.Date = newFakeFacture.Date;
            fakeFacture.Ref = newFakeFacture.Ref;
            fakeFacture.Note = newFakeFacture.Note;
            fakeFacture.IdTypePaiement = newFakeFacture.IdTypePaiement;
            fakeFacture.WithDiscount = newFakeFacture.WithDiscount;
            fakeFacture.Comment = newFakeFacture.Comment;
            fakeFacture.DateEcheance = newFakeFacture.DateEcheance;
            fakeFacture.ClientName = newFakeFacture.ClientName;
            fakeFacture.ClientICE = newFakeFacture.ClientICE;

            var numBonGenerator = new DocNumberGenerator(db);

            fakeFacture.NumBon = numBonGenerator.getNumDocByCompany( newFakeFacture.Ref - 1, newFakeFacture.Date);

            //----------------------------------------------Updating QteStock
            foreach (var fiOld in fakeFacture.FakeFactureItems)
            {
                var article = db.ArticleFactures.Find(fiOld.IdArticleFacture);
                article.QteStock += fiOld.Qte;
            }
            foreach (var fiNew in newFakeFacture.FakeFactureItems)
            {
                var article = db.ArticleFactures.Find(fiNew.IdArticleFacture);
                article.QteStock -= fiNew.Qte;
            }

            db.FakeFactureItems.RemoveRange(fakeFacture.FakeFactureItems);
            db.FakeFactureItems.AddRange(newFakeFacture.FakeFactureItems);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.FakeFactureExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            var fakeFactureWithItems = db.FakeFactures.Where(x => x.Id == fakeFacture.Id);
            return Ok(fakeFactureWithItems);
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] FakeFacture fakeFacture)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);


            var numBonGenerator = new DocNumberGenerator(db);
            var currentYear = DateTime.Now.Year;
            var lastDoc = db.FakeFactures.Where(x => x.Date.Year == currentYear).OrderByDescending(x => x.Ref).FirstOrDefault();
            var lastRef = lastDoc != null ? lastDoc.Ref : 0;
            fakeFacture.Ref = lastRef + 1;
            fakeFacture.NumBon = numBonGenerator.getNumDocByCompany( lastRef, fakeFacture.Date);
            //-------------------------------------------updating QteStock
            foreach (var fi in fakeFacture.FakeFactureItems)
            {
                var article = db.ArticleFactures.Find(fi.IdArticleFacture);
                article.QteStock -= fi.Qte;
            }

            this.db.FakeFactures.Add(fakeFacture);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.FakeFactureExists(fakeFacture.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            var fakeFactureWithItems = db.FakeFactures.Where(x => x.Id == fakeFacture.Id);
            return Ok(fakeFactureWithItems);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<FakeFacture> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            FakeFacture fakeFacture = await this.db.FakeFactures.FindAsync((object)key);
            if (fakeFacture == null)
                return (IActionResult)this.NotFound();
            patch.Patch(fakeFacture);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.FakeFactureExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<FakeFacture>(fakeFacture);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            FakeFacture async = await this.db.FakeFactures.FindAsync((object)key);
            if (async == null)
                return (IActionResult)this.NotFound();

            //--------------------------updating QteStock
            foreach (var fi in async.FakeFactureItems)
            {
                var article = db.ArticleFactures.Find(fi.IdArticleFacture);
                article.QteStock += fi.Qte;
            }

            db.FakeFactureItems.RemoveRange(async.FakeFactureItems);
            db.FakeFactures.Remove(async);
            await db.SaveChangesAsync();
            return (IActionResult)this.NoContent();
        }

        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] Guid key)
        {
            return SingleResult.Create<Client>(this.db.FakeFactures.Where<FakeFacture>((Expression<Func<FakeFacture, bool>>)(m => m.Id == key)).Select<FakeFacture, Client>((Expression<Func<FakeFacture, Client>>)(m => m.Client)));
        }

        [EnableQuery]
        public IQueryable<FakeFactureItem> GetFakeFactureItems([FromODataUri] Guid key)
        {
            return this.db.FakeFactures.Where<FakeFacture>((Expression<Func<FakeFacture, bool>>)(m => m.Id == key)).SelectMany<FakeFacture, FakeFactureItem>((Expression<Func<FakeFacture, IEnumerable<FakeFactureItem>>>)(m => m.FakeFactureItems));
        }

        private bool FakeFactureExists(Guid key)
        {
            return this.db.FakeFactures.Count<FakeFacture>((Expression<Func<FakeFacture, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
