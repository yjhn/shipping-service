using Microsoft.EntityFrameworkCore;

using shipping_service.Persistence.Entities;
using shipping_service.Repositories;

namespace shipping_service.Services
{
    public class PostMachineService : IPostMachineService
    {
        private readonly IPostMachineRepository _postMachines;
private ICodeGenerator _generator;
        public PostMachineService(IPostMachineRepository repo, ICodeGenerator generator)
        {
            _postMachines = repo;
_generator = generator;
        }

        public IQueryable<PostMachine> PostMachines => _postMachines.PostMachines;

        public async Task<int> GeneratePostMachineUnlockCode(long postMachineId)
        {
            PostMachine p = await _postMachines.PostMachines
                .Include(p => p.ShipmentsWithThisSource)
                .Include(p => p.ShipmentsWithThisDestination)
                .FirstOrDefaultAsync(p => p.Id == postMachineId);
            return GeneratePostMachineUnlockCode(p);
        }

        public int GeneratePostMachineUnlockCode(PostMachine p)
        {
return _generator.GeneratePostMachineUnlockCode(p);
            }
        }
    }
