using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Repositories.Entities;

namespace Repositories.Interfaces
{
    public interface ISenderRepository
    {
        Task<List<Sender>> GetAsync();

        Task<Sender> GetAsync(string id);

        Task<Sender> CreateAsync(Sender sender);

        Task<Sender> UpdateAsync(string id, Sender senderIn);

        Task<string> DeleteAsync(string id);
    }
}
