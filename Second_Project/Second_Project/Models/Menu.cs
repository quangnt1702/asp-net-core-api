using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Second_Project.Models
{
    [Table("Menu")]
    public partial class Menu
    {
        public Menu()
        {
            MenuProducts = new HashSet<MenuProduct>();
        }

        [Key]
        public int MenuId { get; set; }
        [Required]
        [StringLength(128)]
        public string MenuName { get; set; }
        public int? StoreId { get; set; }

        [ForeignKey(nameof(StoreId))]
        [InverseProperty("Menus")]
        public virtual Store Store { get; set; }
        [InverseProperty(nameof(MenuProduct.Menu))]
        public virtual ICollection<MenuProduct> MenuProducts { get; set; }
    }
}
