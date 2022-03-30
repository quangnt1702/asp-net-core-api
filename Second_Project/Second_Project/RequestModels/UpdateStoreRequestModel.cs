namespace Second_Project.RequestModels
{
    public class UpdateStoreRequestModel
    {
        public string StoreName { get; set; }
        public string Address { get; set; }
        public int? Status { get; set; }
    }
}