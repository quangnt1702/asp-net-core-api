using System;
using Second_Project.Models;

namespace Second_Project.ViewModels
{
    public class ProductViewModel
    {
        public int? ProductId { get; set; }

        [String]
        [Sort]
        public string ProductName { get; set; }

        [String]
        public string  Provider { get; set; }
        
        public decimal? UnitPrice { get; set; }

        public int?  UnitsInStock { get; set; }
        
        [DateRange]
        public DateTime? CreatedDate { get; set; }
        
        [Boolean]
        public bool? IsDelete { get; set; }
        
        [Skip]
        public string Image { get; set; }
        
        [Child]
        public CategoryViewModel Category { get; set; }

        [Sort]
        public string  SortBy { get; set; }

    }
}