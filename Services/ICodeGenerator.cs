using shipping_service.Persistence.Entities;

namespace shipping_service.Services
{
    public interface ICodeGenerator
    {
        int GeneratePostMachineUnlockCode(PostMachine p);
    }
}
