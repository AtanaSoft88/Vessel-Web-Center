using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesselWebCenter.Data.Configuration.ImportViewModels
{
    public class VesselViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string CallSign { get; set; } = null!;
        [Required]
        public bool IsLaden { get; set; }
        [Required]
        public int LengthOverall { get; set; }
        [Required]
        public int BreadthMax { get; set; }
        [Required]
        public int VesselType { get; set; }

        public string? CargoTypeOnBoard { get; set; } // Can be null if under Ballast or VesselType = Tug
        [Required]
        public int ManningCompanyId { get; set; }
    }
}
