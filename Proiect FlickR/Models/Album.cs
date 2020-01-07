using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proiect_FlickR.Models
{
    public class Album
    {
        [Key]
        public int Id { get; set; }

        public String Name { get; set; }

        public DateTime Date { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}