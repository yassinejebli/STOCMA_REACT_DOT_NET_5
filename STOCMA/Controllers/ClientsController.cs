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
    //[Authorize]
    public class ClientsController : ODataController
    {
        private readonly ApplicationDbContext db;

        public ClientsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET: odata/Clients
        [EnableQuery(EnsureStableOrdering = false)]
        public IQueryable<Client> GetClients()
        {
            return db.Clients.Include(x => x.Paiements).Include(x => x.PaiementFactures).OrderBy(x => x.Disabled).ThenBy(x => x.Name);
        }

        // GET: odata/Clients(5)
        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Clients.Include(x => x.Paiements).Include(x => x.PaiementFactures).Where(client => client.Id == key));
        }

        // PUT: odata/Clients(5)
        public async Task<IActionResult> Put([FromODataUri] Guid key, Delta<Client> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Client client = await db.Clients.FindAsync(key);
            if (client == null)
            {
                return NotFound();
            }

            patch.Put(client);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(client);
        }

        // POST: odata/Clients
        public async Task<IActionResult> Post([FromBody] Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Clients.Add(client);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClientExists(client.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(client);
        }

        // PATCH: odata/Clients(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<Client> patch)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Client client = await db.Clients.FindAsync(key);
            if (client == null)
            {
                return NotFound();
            }

            patch.Patch(client);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(client);
        }

        // DELETE: odata/Clients(5)
        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            Client client = await db.Clients.FindAsync(key);
            if (client == null)
            {
                return NotFound();
            }

            db.Clients.Remove(client);
            await db.SaveChangesAsync();

            return NoContent();
        }

        // GET: odata/Clients(5)/BonAvoirCs
        [EnableQuery]
        public IQueryable<BonAvoirC> GetBonAvoirCs([FromODataUri] Guid key)
        {
            return db.Clients.Where(m => m.Id == key).SelectMany(m => m.BonAvoirCs);
        }

        // GET: odata/Clients(5)/BonLivraisons
        [EnableQuery]
        public IQueryable<BonLivraison> GetBonLivraisons([FromODataUri] Guid key)
        {
            return db.Clients.Where(m => m.Id == key).SelectMany(m => m.BonLivraisons);
        }

        // GET: odata/Clients(5)/Devises
        [EnableQuery]
        public IQueryable<Devis> GetDevises([FromODataUri] Guid key)
        {
            return db.Clients.Where(m => m.Id == key).SelectMany(m => m.Devises);
        }

        // GET: odata/Clients(5)/Dgbs
        [EnableQuery]
        public IQueryable<Dgb> GetDgbs([FromODataUri] Guid key)
        {
            return db.Clients.Where(m => m.Id == key).SelectMany(m => m.Dgbs);
        }

        // GET: odata/Clients(5)/Factures
        [EnableQuery]
        public IQueryable<Facture> GetFactures([FromODataUri] Guid key)
        {
            return db.Clients.Where(m => m.Id == key).SelectMany(m => m.Factures);
        }

        // GET: odata/Clients(5)/FakeFactures
        [EnableQuery]
        public IQueryable<FakeFacture> GetFakeFactures([FromODataUri] Guid key)
        {
            return db.Clients.Where(m => m.Id == key).SelectMany(m => m.FakeFactures);
        }

        // GET: odata/Clients(5)/Paiements
        [EnableQuery]
        public IQueryable<Paiement> GetPaiements([FromODataUri] Guid key)
        {
            return db.Clients.Where(m => m.Id == key).SelectMany(m => m.Paiements);
        }

        // GET: odata/Clients(5)/Rdbs
        [EnableQuery]
        public IQueryable<Rdb> GetRdbs([FromODataUri] Guid key)
        {
            return db.Clients.Where(m => m.Id == key).SelectMany(m => m.Rdbs);
        }

        // GET: odata/Clients(5)/Revendeur
        [EnableQuery]
        public SingleResult<Revendeur> GetRevendeur([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Clients.Where(m => m.Id == key).Select(m => m.Revendeur));
        }

        private bool ClientExists(Guid key)
        {
            return db.Clients.Count(e => e.Id == key) > 0;
        }
    }
}
