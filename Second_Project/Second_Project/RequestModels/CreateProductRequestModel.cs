using Second_Project.Models;

namespace Second_Project.RequestModels
{
    public class CreateProductRequestModel
    {
        public string ProductName { get; set; }

        public string Provider { get; set; }

        public decimal UnitPrice { get; set; }

        public int UnitsInPrice { get; set; }
        
        public string Image { get; set; }
        
        public int CategoryId { get; set; }
    }
}