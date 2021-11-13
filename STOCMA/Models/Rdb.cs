
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class Rdb
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int Ref { get; set; }

        public string NumBon { get; set; }

        public DateTime Date { get; set; }

        public Guid IdClient { get; set; }

        public string User { get; set; }

        public string Type { get; set; }
        public virtual Client Client { get; set; }

        public virtual ICollection<RdbItem> RdbItems { get; set; }
    }
}
