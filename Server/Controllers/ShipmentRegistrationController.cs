using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Mvc;

using Repositories;
using Repositories.Entities;
using Repositories.Interfaces;

namespace shipping_service.Server.Controllers
{
    [ApiVersion("1.0")]
    [Route("/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ShipmentRegistrationController : ControllerBase
    {

        private readonly ISenderRepository _senderRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly IPostMachineRepository _postMachineRepository;

        public ShipmentRegistrationController(SenderRepository senderRepository, PackageRepository packageRepository,
            PostMachineRepository postMachineRepository)
        {
            _senderRepository = senderRepository;
            _packageRepository = packageRepository;
            _postMachineRepository = postMachineRepository;
        }

        [HttpGet($"{nameof(RetrievePostMachines)}")]
        public async Task<IEnumerable<PostMachine>> RetrievePostMachines()
        {
            return await _postMachineRepository.GetAsync();
        }

        [HttpPost($"{nameof(CreatePackage)}")]
        public async Task CreatePackage([FromBody] Package package)
        {
            await _packageRepository.CreateAsync(package);
        }

    }

}