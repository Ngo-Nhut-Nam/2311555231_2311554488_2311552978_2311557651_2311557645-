namespace _2311555231_2311554488_2311552978_2311557651_2311557645_.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new();
    }
}
