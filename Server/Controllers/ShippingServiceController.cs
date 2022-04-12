using Microsoft.AspNetCore.Mvc;
using Repositories.Entities;
using Repositories.Interfaces;

namespace shipping_service.Server.Controllers;

[ApiVersion("1.0")]
[Route("/[controller]")]
[Produces("application/json")]
[ApiController]
public class ShippingServiceController : ControllerBase
{
    private readonly ICourierRepository _courierRepository;
    private readonly ISenderRepository _senderRepository;
    private readonly IPackageRepository _packageRepository;
    private readonly IPostMachineRepository _postMachineRepository;

    public ShippingServiceController(ICourierRepository courierRepository,
        ISenderRepository senderRepository,
        IPackageRepository packageRepository,
        IPostMachineRepository postMachineRepository)
    {
        _courierRepository = courierRepository;
        _senderRepository = senderRepository;
        _packageRepository = packageRepository;
        _postMachineRepository = postMachineRepository;
    }

    [HttpGet("{CourierId}", Name = "GetAsync")]
    public async Task<ActionResult<Courier>> GetAsync(string courierId)
    {
        var courier = await _courierRepository.GetAsync(courierId);

        if (courier == null) return NotFound();

        return courier;
    }

    [HttpPost("{CourierId}", Name = "PostAsync")]
    [ActionName(nameof(PostAsync))]
    public async Task<ActionResult<Courier>> PostAsync([FromBody] Courier courier)
    {
        await _courierRepository.CreateAsync(courier);

        return CreatedAtRoute(nameof(PostAsync), new { id = courier._id }, courier);
    }

}

