using System.ComponentModel.DataAnnotations;

namespace STOCMA.Models
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int Afficher { get; set; }

        public bool Disabled { get; set; } = false;
        public bool Enabled { get; set; } = false;

    }
}
