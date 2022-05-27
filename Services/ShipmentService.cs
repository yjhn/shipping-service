using Microsoft.EntityFrameworkCore;

using shipping_service.Models;
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
                .Include(s => s.SourceMachine)
                .Include(s => s.DestinationMachine)
                .ToListAsync();
        }

        public IEnumerable<Shipment> GetUnassignedInSourceMachine()
        {
            return _shipmentRepository.Shipments
                .Where(s => s.CourierId == null && s.Status == ShipmentStatus.InSourcePostMachine)
                .Include(s => s.SourceMachine)
                .Include(s => s.DestinationMachine);
        }

        public async Task<IEnumerable<Shipment>> GetAssignedAsync(long courierId)
        {
            return await _shipmentRepository.Shipments
                .Where(s => s.CourierId == courierId)
                .Include(s => s.SourceMachine)
                .Include(s => s.DestinationMachine)
                .ToListAsync();
        }

        public async Task<Shipment?> GetById(long id)
        {
            return await _shipmentRepository.GetAsync(id);
        }

        public async Task<Shipment?> GetByIdBypassCache(long id)
        {
            return await _shipmentRepository.GetBypassCache(id);
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
                .SingleOrDefaultAsync();
        }

        public async Task<Shipment?> GetShFromSrcCourierCode(long postMachineId, int unlockCode)
        {
            return await _shipmentRepository.Shipments
                .Where(s => s.SourceMachineId == postMachineId && s.SrcPmCourierUnlockCode == unlockCode)
                .SingleOrDefaultAsync();
        }

        public async Task<Shipment?> GetShFromDestCourierCode(long postMachineId, int unlockCode)
        {
            return await _shipmentRepository.Shipments
                .Where(s => s.DestinationMachineId == postMachineId && s.DestPmCourierUnlockCode == unlockCode)
                .SingleOrDefaultAsync();
        }

        public async Task<Shipment?> GetShFromDestReceiverCode(long postMachineId, int unlockCode)
        {
            Shipment? sh = await _shipmentRepository.Shipments
                .Where(s => s.DestinationMachineId == postMachineId && s.DestPmReceiverUnlockCode == unlockCode)
                .SingleOrDefaultAsync();
            return sh is not { Status: ShipmentStatus.InDestinationPostMachine } ? null : sh;
        }

        public async Task<Shipment?> SelectIncludeAll(long id)
        {
            return await _shipmentRepository.Shipments
                .Include(s => s.Sender)
                .Include(s => s.Courier)
                .Include(s => s.SourceMachine)
                .Include(s => s.DestinationMachine)
                .SingleOrDefaultAsync(s => s.Id == id);
        }

        public string GenerateIdHash(long id)
        {
            return ComputeBase64(id);
        }

        public bool IsValidIdHash(long id, string hash)
        {
            return hash == GenerateIdHash(id);
        }

        public async Task<DbUpdateResult> AssignShipmentToCourier(Shipment s, Courier c)
        {
            s.CourierId = c.Id;
            s.SrcPmCourierUnlockCode = await _postMachineService.GeneratePostMachineUnlockCode(s.SourceMachineId);
            return await _shipmentRepository.UpdateAsync(s);
        }

        public async Task<DbUpdateResult> UnassignShipment(Courier c, Shipment s)
        {
            if (s.CourierId != c.Id)
            {
                throw new InvalidOperationException("Cannot unassign shipment as the courier IDs do not match");
            }

            if (s.Status != ShipmentStatus.InSourcePostMachine)
            {
                throw new InvalidOperationException(
                    "Cannot unassign shipment that is already taken from source post machine");
            }

            s.CourierId = null;
            s.SrcPmCourierUnlockCode = null;
            return await _shipmentRepository.UpdateAsync(s);
        }

        public void AssignFrom(Shipment from, Shipment to)
        {
            // Don't overwrite `xmin` as it is used for last update tracking.
            to.CourierId = from.CourierId;
            to.Created = from.Created;
            to.Modified = from.Modified;
            to.Description = from.Description;
            to.SenderId = from.SenderId;
            to.DestinationMachineId = from.DestinationMachineId;
            to.SourceMachineId = from.SourceMachineId;
            to.Status = from.Status;
            to.Title = from.Title;
            to.SrcPmSenderUnlockCode = from.SrcPmSenderUnlockCode;
            to.SrcPmCourierUnlockCode = from.SrcPmCourierUnlockCode;
            to.DestPmCourierUnlockCode = from.DestPmCourierUnlockCode;
            to.DestPmReceiverUnlockCode = from.DestPmReceiverUnlockCode;
        }

        public IEnumerable<Shipment> GetBySenderUsername(string username)
        {
            return Shipments.Where(s => s.Sender.Username == username);
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

        public void Detach(long id)
        {
            _shipmentRepository.Detach(id);
        }
    }
}
