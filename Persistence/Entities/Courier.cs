using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shipping_service.Persistence.Entities
{
    public class Courier
    {
        public ulong Id { get; set; }
        public string Username { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public byte[] HashedPassword { get; set; }
        public string Name { get; set; }
        public ICollection<Package> CurrentPackages { get; set; }
    }
}
