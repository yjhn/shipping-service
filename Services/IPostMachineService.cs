using shipping_service.Persistence.Entities;

namespace shipping_service.Services
{
    public interface IPostMachineService
    {
        IQueryable<PostMachine> PostMachines { get; }
        int GeneratePostMachineUnlockCode(PostMachine p);
    }
}
