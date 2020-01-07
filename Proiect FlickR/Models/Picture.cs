using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

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

        //[Required(ErrorMessage = "Categoria este obligatorie")]
        public int CategoryId { get; set; }

        // public virtual Category Category { get; set; }
        public virtual Category Category { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        public DateTime Time { get; set; }

        public String Description { get; set; }

        public String ThumbPath { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int AlbumId { get; set; }

    }
}