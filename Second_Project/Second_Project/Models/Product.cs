using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Second_Project.Models
{
    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            MenuProducts = new HashSet<MenuProduct>();
        }

        [Key]
        public int ProductId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }
        [StringLength(50)]
        public string Provider { get; set; }
        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int CategoryId { get; set; }
        [Column(TypeName = "datetime")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? CreatedDate { get; set; }=DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public bool? IsDelete { get; set; } = false;
        [StringLength(256)]
        public string Image { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty("Products")]
        public virtual Category Category { get; set; }
        [InverseProperty(nameof(MenuProduct.Product))]
        public virtual ICollection<MenuProduct> MenuProducts { get; set; }
    }
}
