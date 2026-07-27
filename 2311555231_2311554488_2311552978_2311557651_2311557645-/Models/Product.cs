namespace _2311555231_2311554488_2311552978_2311557651_2311557645_.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new();
    }
}