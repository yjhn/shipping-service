using shipping_service.Persistence.Entities;

namespace shipping_service.Services
{
    public interface IPostMachineService
    {
        int GeneratePostMachineUnlockCode(PostMachine p);
    }
}
