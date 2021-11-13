
using System;
using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class Company
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string CompleteName { get; set; } // fullName
        public string AdresseSociete1 { get; set; }
        public string AdresseSociete2 { get; set; }
        public string AdresseSociete3 { get; set; }
        public string AdresseSociete4 { get; set; }

        public string QrCode { get; set; }

        public string Partner { get; set; }

        public string Header { get; set; }

        public string Footer { get; set; }

        public string Tel { get; set; }

        public string Fax { get; set; }

        public string Adresse { get; set; }

        public string CodeSecurite { get; set; }

        public bool UseVAT { get; set; } = false;
    }
}
