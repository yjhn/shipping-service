using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Repositories.Entities;

namespace Repositories.Interfaces
{
    public interface IPostMachineRepository
    {
        Task<List<PostMachine>> GetAsync();

        Task<PostMachine> GetAsync(string id);

        Task<PostMachine> CreateAsync(PostMachine postMachine);

        Task<PostMachine> UpdateAsync(string id, PostMachine postMachineIn); 

        Task<string> DeleteAsync(string id);

    }
}
