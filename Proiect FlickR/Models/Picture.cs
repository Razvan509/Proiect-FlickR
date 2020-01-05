using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proiect_FlickR.Models
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }
        
        public String Name { get; set; }
        [Required]
        [DisplayName("Upload Picture")]
        public String Path { get; set; }
        



    }
}