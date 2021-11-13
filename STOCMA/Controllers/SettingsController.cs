using System;
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
    public class SettingsController : ODataController
    {
        private readonly ApplicationDbContext db;

        public SettingsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [EnableQuery]
        public IQueryable<Setting> GetSettings()
        {
            return (IQueryable<Setting>)this.db.Settings;
        }

        [EnableQuery]
        public SingleResult<Setting> GetSetting([FromODataUri] int key)
        {
            return SingleResult.Create<Setting>(this.db.Settings.Where<Setting>((Expression<Func<Setting, bool>>)(setting => setting.Id == key)));
        }

        public async Task<IActionResult> Put([FromODataUri] int key, Delta<Setting> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            Setting setting = await this.db.Settings.FindAsync((object)key);
            if (setting == null)
                return (IActionResult)this.NotFound();
            patch.Put(setting);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.SettingExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<Setting>(setting);
        }

        public async Task<IActionResult> Post([FromBody]  Setting setting)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            this.db.Settings.Add(setting);
            int num = await this.db.SaveChangesAsync();
            return (IActionResult)this.Created<Setting>(setting);
        }

        [AcceptVerbs(new string[] { "PATCH", "MERGE" })]
        public async Task<IActionResult> Patch([FromODataUri] int key, Delta<Setting> patch)
        {
            if (!this.ModelState.IsValid)
                return (IActionResult)this.BadRequest(this.ModelState);
            Setting setting = await this.db.Settings.FindAsync((object)key);
            if (setting == null)
                return (IActionResult)this.NotFound();
            patch.Patch(setting);
            try
            {
                int num = await this.db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!this.SettingExists(key))
                    return (IActionResult)this.NotFound();
                throw;
            }
            return (IActionResult)this.Updated<Setting>(setting);
        }

        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            Setting async = await this.db.Settings.FindAsync((object)key);
            if (async == null)
                return (IActionResult)this.NotFound();
            this.db.Settings.Remove(async);
            await this.db.SaveChangesAsync();
            return (IActionResult)NoContent();
        }

        private bool SettingExists(int key)
        {
            return this.db.Settings.Count<Setting>((Expression<Func<Setting, bool>>)(e => e.Id == key)) > 0;
        }
    }
}
