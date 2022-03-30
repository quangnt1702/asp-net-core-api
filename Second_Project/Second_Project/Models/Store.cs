using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Second_Project.Models
{
    [Table("Store")]
    public partial class Store
    {
        public Store()
        {
            Menus = new HashSet<Menu>();
        }

        [Key]
        public int StoreId { get; set; }
        [StringLength(128)]
        public string StoreName { get; set; }
        [StringLength(128)]
        public string Address { get; set; }
        public int? Status { get; set; }

        [InverseProperty(nameof(Menu.Store))]
        public virtual ICollection<Menu> Menus { get; set; }
    }
}
