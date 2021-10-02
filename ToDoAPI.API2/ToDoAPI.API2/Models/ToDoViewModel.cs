using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; //added for validation
using ToDoAPI.API.Models; 

namespace ToDoAPI.API.Models
{
    public class ToDoViewModel
    {
        [Key]
        public int TodoId { get; set; }
        [Required]        
        public string Action { get; set; }
        [Required]
        public bool Done { get; set; }

        public Nullable<int> CategoryId { get; set; }

        public virtual CategoryViewModel Category { get; set; }
    }
}