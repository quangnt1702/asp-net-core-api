using System.Collections.Generic;
using Second_Project.Models;

namespace Second_Project.ViewModels
{
    public class MenuViewModel
    {
                public int? MenuId { get; set; }
                public string MenuName { get; set; }
                public  StoreViewModel Store { get; set; }
    }
}