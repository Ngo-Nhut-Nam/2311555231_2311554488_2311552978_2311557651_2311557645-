namespace _2311555231_2311554488_2311552978_2311557651_2311557645_.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = new();
    }
}
