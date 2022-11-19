using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesselWebCenter.Data.ImportViewModels
{
    public class CompanyViewModel
    {
        [Required]
        public int Id { get; set; }

        [JsonProperty("CompanyName")]
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;
       
    }  

            
}
