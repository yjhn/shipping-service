namespace shipping_service.Entities
{
    public class Package
    {
        public string? _id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public PostMachine SourceMachine { get; set; }

        public PostMachine DestinationMachine { get; set; }

    }
}
