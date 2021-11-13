
using System;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class JournalConnexion
    {
        [Key]
        public Guid Id { get; set; }

        public string User { get; set; }

        public DateTime Date { get; set; }
    }
}
