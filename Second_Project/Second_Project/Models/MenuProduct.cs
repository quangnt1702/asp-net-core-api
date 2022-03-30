using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Second_Project.Models
{
    [Table("Menu_Product")]
    public partial class MenuProduct
    {
        [Key]
        public int MenuId { get; set; }
        [Key]
        public int ProductId { get; set; }

        [ForeignKey(nameof(MenuId))]
        [InverseProperty("MenuProducts")]
        public virtual Menu Menu { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty("MenuProducts")]
        public virtual Product Product { get; set; }
    }
}
