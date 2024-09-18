using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Models
{
    public class StoreProduct
    {
        public int Id { get; set; }
        [Required]
        public int StoreId { get; set; }
       
        public virtual Store? Store { get; set; }

        [Required]
        public int ProductId { get; set; }
        
        public virtual Product? Product { get; set; }
        
        public int Quantity_Stocks { get; set; }
        public decimal PriceProduct { get; set; }

    }
}
