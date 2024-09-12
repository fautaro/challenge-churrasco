namespace Domain.Entities
{
    public class Products
    {
        public int Id { get; set; }
        public long SKU { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }


    }
}
