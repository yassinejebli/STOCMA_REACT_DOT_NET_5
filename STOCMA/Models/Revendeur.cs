using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace STOCMA.Models
{
    public class Revendeur
    {
        [Key]
        public Guid Id { get; set; }


        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public virtual ICollection<Client> Clients { get; set; }


    }
}