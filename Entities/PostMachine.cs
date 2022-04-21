namespace shipping_service.Entities
{
    public class PostMachine
    {
        public string? _id { get; set; }

        public string Address { get; set; }

        public IList<Package>? Package { get; set; }
    }
}
