using Repositories.Entities;
using Repositories.Interfaces;

namespace Services
{
    public interface IPackageService
    {
Task<List<Package>> GetUnassignedAsync();
}
}
