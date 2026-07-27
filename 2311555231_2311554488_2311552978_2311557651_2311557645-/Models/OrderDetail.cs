namespace _2311555231_2311554488_2311552978_2311557651_2311557645_.Models
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
