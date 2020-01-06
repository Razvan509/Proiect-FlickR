using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
<<<<<<< HEAD

        //[Required(ErrorMessage = "Categoria este obligatorie")]
        public int CategoryId { get; set; }

        // public virtual Category Category { get; set; }
        public virtual Category Category { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

=======
        
        public Category Category { get; set; }
>>>>>>> a8b7b82cfa6e28e63914c58770123ce8d8d5270f

        public DateTime Time { get; set; }

        public String Description { get; set; }

    }
}