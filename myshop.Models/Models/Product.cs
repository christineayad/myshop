using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal price { get; set; }
        [Required]
        [DisplayName("Category")]
        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        [DisplayName("Image")]
        [ValidateNever]

        public string Img { get; set; }
    }
}
