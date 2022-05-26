using Microsoft.EntityFrameworkCore;

using shipping_service.Persistence.Entities;
using shipping_service.Repositories;

namespace shipping_service.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly IPostMachineService _postMachineService;
        private readonly IShipmentRepository _shipmentRepository;

        public ShipmentService(IShipmentRepository shipmentRepository,
            IPostMachineService p)
        {
            _shipmentRepository = shipmentRepository;
            _postMachineService = p;
        }

        public IQueryable<Shipment> Shipments => _shipmentRepository.Shipments;

        public async Task<IEnumerable<Shipment>> GetUnassignedInSourceMachineAsync()
        {
            return await _shipmentRepository.Shipments
                .Where(s => s.CourierId == null && s.Status == ShipmentStatus.InSourcePostMachine)
                .Include(s => s.Sender)
                .Include(s => s.Courier)
                .Include(s => s.SourceMachine)
                .Include(s => s.DestinationMachine)
                .ToListAsync();
        }

        public async Task<IEnumerable<Shipment>> GetAssignedAsync(long courierId)
        {
            return await _shipmentRepository.Shipments.
                Where(s => s.CourierId == courierId)
                .Include(s => s.Sender)
                .Include(s => s.Courier)
                .Include(s => s.SourceMachine)
                .Include(s => s.DestinationMachine)
                .ToListAsync();
        }

        public async Task<Shipment?> GetById(long id)
        {
            return await _shipmentRepository.GetAsync(id);
        }

        public async Task CreateAsync(Shipment s)
        {
            // Generate sender unlock code.
            s.SrcPmSenderUnlockCode = await _postMachineService.GeneratePostMachineUnlockCode(s.SourceMachineId);
            // Generate receiver unlock code.
            s.DestPmReceiverUnlockCode =
                await _postMachineService.GeneratePostMachineUnlockCode(s.DestinationMachineId);
            await _shipmentRepository.CreateAsync(s);
        }

        public async Task ChangeShipmentStatusToSrc(Shipment s, PostMachine p)
        {
            if (s.Status != ShipmentStatus.RegisteredForSending)
            {
                throw new InvalidOperationException("Cannot change status: " + s.Status
                                                                             + " to ShipmentStatus.InSourcePostMachine");
            }

            s.Status = ShipmentStatus.InSourcePostMachine;
            s.SrcPmSenderUnlockCode = null;
            // Courier code is generated when assigning shipment to courier.
            await _shipmentRepository.UpdateAsync(s);
        }

        public async Task ChangeShipmentStatusToDest(Shipment s, PostMachine p)
        {
            if (s.Status != ShipmentStatus.Shipping)
            {
                throw new InvalidOperationException("Cannot change status: " + s.Status
                                                                             + " to ShipmentStatus.InDestinationPostMachine");
            }

            // Receiver unlock code is generated when the shipment is created.
            s.Status = ShipmentStatus.InDestinationPostMachine;
            s.DestPmCourierUnlockCode = null;
            await _shipmentRepository.UpdateAsync(s);
        }

        public async Task ChangeShipmentStatusToDelivered(Shipment s)
        {
            if (s.Status != ShipmentStatus.InDestinationPostMachine)
            {
                throw new InvalidOperationException("Cannot change status: " + s.Status
                                                                             + " to ShipmentStatus.Delivered");
            }

            s.Status = ShipmentStatus.Delivered;
            s.DestPmReceiverUnlockCode = null;
            await _shipmentRepository.UpdateAsync(s);
        }

        public async Task ChangeShipmentStatusToShipping(Shipment s, PostMachine p)
        {
            if (s.Status != ShipmentStatus.InSourcePostMachine)
            {
                throw new InvalidOperationException("Cannot change status: " + s.Status
                                                                             + " to ShipmentStatus.Shipping");
            }

            s.Status = ShipmentStatus.Shipping;
            s.SrcPmCourierUnlockCode = null;
            s.DestPmCourierUnlockCode = _postMachineService.GeneratePostMachineUnlockCode(p);
            await _shipmentRepository.UpdateAsync(s);
        }

        public async Task<Shipment?> GetShFromSrcSenderCode(long postMachineId, int unlockCode)
        {
            return await _shipmentRepository.Shipments
                .Where(s => s.SourceMachineId == postMachineId && s.SrcPmSenderUnlockCode == unlockCode)
                .FirstOrDefaultAsync();
        }

        public async Task<Shipment?> GetShFromSrcCourierCode(long postMachineId, int unlockCode)
        {
            return await _shipmentRepository.Shipments
                .Where(s => s.SourceMachineId == postMachineId && s.SrcPmCourierUnlockCode == unlockCode)
                .FirstOrDefaultAsync();
        }

        public async Task<Shipment?> GetShFromDestCourierCode(long postMachineId, int unlockCode)
        {
            return await _shipmentRepository.Shipments
                .Where(s => s.DestinationMachineId == postMachineId && s.DestPmCourierUnlockCode == unlockCode)
                .FirstOrDefaultAsync();
        }

        public async Task<Shipment?> GetShFromDestReceiverCode(long postMachineId, int unlockCode)
        {
            Shipment? sh = await _shipmentRepository.Shipments
                .Where(s => s.DestinationMachineId == postMachineId && s.DestPmReceiverUnlockCode == unlockCode)
                .FirstOrDefaultAsync();
            return sh is not { Status: ShipmentStatus.InDestinationPostMachine } ? null : sh;
        }

        public async Task<Shipment?> SelectIncludeAll(long id)
        {
            return await _shipmentRepository.Shipments
                .Include(s => s.Sender)
                .Include(s => s.Courier)
                .Include(s => s.SourceMachine)
                .Include(s => s.DestinationMachine)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public string GenerateIdHash(long id)
        {
            return ComputeBase64(id);
        }

        public bool IsValidIdHash(long id, string hash)
        {
            return hash == GenerateIdHash(id);
        }

        private static string ComputeBase64(long data)
        {
            byte[] bytes = BitConverter.GetBytes(data + 1_000_000).Take(3).ToArray();
            string s = Convert.ToBase64String(bytes);
            // ToBase64String generates strings which are potentially unsafe for use in URLs
            s = s.Split('=')[0]; // Remove any trailing '='s
            s = s.Replace('+', '-'); // 62nd char of encoding
            s = s.Replace('/', '_'); // 63rd char of encoding
            return s;
        }

        public async Task AssignShipmentToCourier(Shipment s, Courier c)
        {
            s.Courier = c;
            c.CurrentShipments.Add(s);
            s.SrcPmCourierUnlockCode = await _postMachineService.GeneratePostMachineUnlockCode(s.SourceMachineId);
            await _shipmentRepository.UpdateAsync(s);
        }
    }
}
