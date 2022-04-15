using Microsoft.AspNetCore.Mvc;

using Repositories.Entities;
using Repositories.Interfaces;

using Services;
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
    private readonly IPackageService _packageService;

    public ShippingServiceController(ICourierRepository courierRepository,
        ISenderRepository senderRepository,
        IPackageRepository packageRepository,
        IPostMachineRepository postMachineRepository,
        IPackageService packageService)
    {
        _courierRepository = courierRepository;
        _senderRepository = senderRepository;
        _packageRepository = packageRepository;
        _postMachineRepository = postMachineRepository;
        _packageService = packageService;
    }

    [HttpGet("courier/{CourierId}", Name = "GetAsync")]
    public async Task<ActionResult<Courier>> GetAsync(string courierId)
    {
        var courier = await _courierRepository.GetAsync(courierId);

        if (courier == null) return NotFound();

        return courier;
    }

    [HttpPost("courier/{CourierId}", Name = "PostAsync")]
    [ActionName(nameof(PostAsync))]
    public async Task<ActionResult<Courier>> PostAsync([FromBody] Courier courier)
    {
        await _courierRepository.CreateAsync(courier);

        return CreatedAtRoute(nameof(PostAsync), new { id = courier._id }, courier);
    }
    [HttpGet("new-packages", Name = "GetUnassignedPackages")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Package>>> GetUnassignedPackages()
    {
        return await _packageService.GetUnassignedAsync();
    }

    [HttpPut("courier/{CourierId}/{PackageId}", Name = "AddCourierPackage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ActionName(nameof(AddCourierPackage))]
    public async Task<ActionResult<Courier>> AddCourierPackage(String CourierId, String PackageId)
    {
        List<Package> packages = await _packageService.GetUnassignedAsync();
        var courier = await _courierRepository.GetAsync(CourierId);
        var package = await _packageRepository.GetAsync(PackageId);
        if (package == null || courier == null)
        {
            return BadRequest();
        }
        if (packages.Contains(package))
        {
            var courierPackages = courier.Packages;
            packages.Add(package);
            courier.Packages = packages;
            await _courierRepository.UpdateAsync(courier._id, courier);
            StatusCode(200, courier);
        }
        else
        {
            Conflict();
        }
        return courier;
    }


}

