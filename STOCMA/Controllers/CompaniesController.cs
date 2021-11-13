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
    public class CompaniesController : ODataController
    {
        private readonly ApplicationDbContext db;

        public CompaniesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery]
        public IQueryable<Company> GetCompanies()
        {
            return (IQueryable<Company>)this.db.Companies;
        }

        [EnableQuery]
        public SingleResult<Company> GetCompany([FromODataUri] Guid key)
        {
            return SingleResult.Create<Company>(this.db.Companies.Where<Company>((Expression<Func<Company, bool>>)(company => company.Id == key)));
        }

        public async Task<IActionResult> Put([FromODataUri] Guid key, Delta<Company> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            Company company = await this.db.Companies.FindAsync((object)key);
            if (company == null)
                return (IActionResult)this.NotFound();
            patch.Put(company);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.CompanyExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<Company>(company);
        }

        public async Task<IActionResult> Post([FromBody] Company company)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            this.db.Companies.Add(company);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (this.CompanyExists(company.Id))
                    return (IActionResult)this.Conflict();
                throw;
            }
            return (IActionResult)this.Created<Company>(company);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<Company> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            Company company = await this.db.Companies.FindAsync((object)key);
            if (company == null)
                return (IActionResult)this.NotFound();
            patch.Patch(company);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.CompanyExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<Company>(company);
        }

        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            Company async = await this.db.Companies.FindAsync((object)key);
            if (async == null)
                return (IActionResult)this.NotFound();
            this.db.Companies.Remove(async);
            int num = await this.db.SaveChangesAsync();
            return (IActionResult)this.NoContent();
        }


        private bool CompanyExists(Guid key)
        {
            return this.db.Companies.Count<Company>((Expression<Func<Company, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
