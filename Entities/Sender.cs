namespace shipping_service.Entities
{
    public class Sender
    {
        public string? _id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public IList<Package>? Package { get; set; }

    }
}
