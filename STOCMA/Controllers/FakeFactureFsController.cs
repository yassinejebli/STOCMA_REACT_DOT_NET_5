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
    public class FakeFactureFsController : ODataController
    {
        private readonly ApplicationDbContext db;

        public FakeFactureFsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET: odata/FakeFactureFs
        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<FakeFactureF> GetFakeFactureFs()
        {
            return db.FakeFacturesF.OrderByDescending(x => x.Date);
        }

        // GET: odata/FakeFactureFs(5)
        [EnableQuery]
        public SingleResult<FakeFactureF> GetFakeFactureF([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.FakeFacturesF.Where(fakeFactureF => fakeFactureF.Id == key));
        }

        // PUT: odata/FakeFactureFs(5)
        [EnableQuery]
        public IActionResult Put([FromODataUri] Guid key, FakeFactureF newFakeFactureF)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FakeFactureF fakeFactureF = db.FakeFacturesF.Find(key);
            if (fakeFactureF == null)
            {
                return NotFound();
            }

            fakeFactureF.Date = newFakeFactureF.Date;
            fakeFactureF.NumBon = newFakeFactureF.NumBon;
            fakeFactureF.IdTypePaiement = newFakeFactureF.IdTypePaiement;
            fakeFactureF.Comment = newFakeFactureF.Comment;

            //----------------------------------------------Updating QteStock
            foreach (var fiOld in fakeFactureF.FakeFactureFItems)
            {
                var article = db.ArticleFactures.Find(fiOld.IdArticleFacture);
                article.QteStock -= fiOld.Qte;
            }
            foreach (var fiNew in newFakeFactureF.FakeFactureFItems)
            {
                var article = db.ArticleFactures.Find(fiNew.IdArticleFacture);
                article.QteStock += fiNew.Qte;
            }

            //-----------------------------------------------Updating document items
            db.FakeFactureFItems.RemoveRange(fakeFactureF.FakeFactureFItems);
            db.FakeFactureFItems.AddRange(newFakeFactureF.FakeFactureFItems);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FakeFactureFExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var fakeFactureFWithItems = db.FakeFacturesF.Where(x => x.Id == fakeFactureF.Id);
            return Ok(fakeFactureFWithItems);
        }

        [EnableQuery]
        public IActionResult Post(FakeFactureF fakeFactureF)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //-------------------------------------------updating QteStock
            foreach (var fi in fakeFactureF.FakeFactureFItems)
            {
                var article = db.ArticleFactures.Find(fi.IdArticleFacture);
                if (article.QteStock <= 0)
                {
                    article.PA = fi.Pu;
                }
                else
                {
                    //PUMP
                    var PA = (Decimal)(((article.QteStock * article.PA) + (fi.Qte * fi.Pu)) / (article.QteStock + fi.Qte));
                    article.PA = (float)Math.Round(PA, 2, MidpointRounding.AwayFromZero);
                }
                article.QteStock += fi.Qte;
            }


            db.FakeFacturesF.Add(fakeFactureF);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (FakeFactureFExists(fakeFactureF.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            var fakeFactureFWithItems = db.FakeFacturesF.Where(x => x.Id == fakeFactureF.Id);
            return Ok(fakeFactureFWithItems);
        }

        // PATCH: odata/FakeFactureFs(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IActionResult Patch([FromODataUri] Guid key, Delta<FakeFactureF> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FakeFactureF fakeFactureF = db.FakeFacturesF.Find(key);
            if (fakeFactureF == null)
            {
                return NotFound();
            }

            patch.Patch(fakeFactureF);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FakeFactureFExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(fakeFactureF);
        }

        // DELETE: odata/FakeFactureFs(5)
        public IActionResult Delete([FromODataUri] Guid key)
        {
            FakeFactureF fakeFactureF = db.FakeFacturesF.Find(key);
            if (fakeFactureF == null)
            {
                return NotFound();
            }
            foreach (var fi in fakeFactureF.FakeFactureFItems)
            {
                var article = db.ArticleFactures.Find(fi.IdArticleFacture);
                article.QteStock -= fi.Qte;
            }

            db.FakeFactureFItems.RemoveRange(fakeFactureF.FakeFactureFItems);
            db.FakeFacturesF.Remove(fakeFactureF);
            db.SaveChanges();

            return NoContent();
        }

        // GET: odata/FakeFactureFs(5)/FakeFactureFItems
        [EnableQuery]
        public IQueryable<FakeFactureFItem> GetFakeFactureFItems([FromODataUri] Guid key)
        {
            return db.FakeFacturesF.Where(m => m.Id == key).SelectMany(m => m.FakeFactureFItems);
        }

        // GET: odata/FakeFactureFs(5)/Fournisseur
        [EnableQuery]
        public SingleResult<Fournisseur> GetFournisseur([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.FakeFacturesF.Where(m => m.Id == key).Select(m => m.Fournisseur));
        }

        private bool FakeFactureFExists(Guid key)
        {
            return db.FakeFacturesF.Count(e => e.Id == key) > 0;
        }
    }
}
