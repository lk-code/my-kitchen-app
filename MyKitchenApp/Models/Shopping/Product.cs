namespace MyKitchenApp.Models.Shopping
{
    public class Product
    {
        public string Title { get; set; }
        public int Count { get; set; }
        public string MeasuringUnit { get; set; }

        public Product(string title, int count, string measuringUnit)
        {
            this.Title = title;
            this.Count = count;
            this.MeasuringUnit = measuringUnit;
        }
    }
}