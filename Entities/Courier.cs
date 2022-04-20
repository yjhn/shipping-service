namespace shipping_service.Entities
{
    public class Courier
    {
        public string? _id { get; set; }

        public string Name { get; set; }

        public IList<Package>? Packages { get; set; }
    }
}
