namespace Second_Project.RequestModels
{
    public class UpdateProductRequestModel
    {
        public string ProductName { get; set; }

        public string Provider { get; set; }

        public decimal UnitPrice { get; set; }

        public int UnitsInStock { get; set; }
        
        public bool? IsDelete { get; set; }

        public string Image { get; set; }
        
        public int CategoryId { get; set; }
    }
}