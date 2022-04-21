using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shipping_service.Persistence.Entities
{
    public class PostMachine
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string Address { get; set; }
        public ICollection<Package> PackagesWithThisSource { get; set; }
        public ICollection<Package> PackagesWithThisDestination { get; set; }
    }
}
