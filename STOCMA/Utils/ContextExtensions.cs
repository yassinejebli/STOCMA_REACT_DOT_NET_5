using System;
using System.Data.Entity;
using System.Linq;
using STOCMA.Data;
using STOCMA.Models;

namespace STOCMA.Utils
{
    public static class ContextExtensions
    {
        public static IQueryable<object> Set(this ApplicationDbContext _context, Type t)
        {
            return (IQueryable<object>)_context.GetType().GetMethod("Set").MakeGenericMethod(t).Invoke(_context, null);
        }
    }

}

