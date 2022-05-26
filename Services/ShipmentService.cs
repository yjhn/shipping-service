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

        public async Task<IEnumerable<Shipment>> GetUnassignedAsync()
        {
            return await _shipmentRepository.Shipments.
                Where(s => s.CourierId == null).
                Include(s => s.Sender).
                Include(s => s.Courier).
                Include(s => s.SourceMachine).
                Include(s => s.DestinationMachine).ToListAsync();
        }

        public async Task<IEnumerable<Shipment>> GetAssignedAsync(long courierId)
        {
            return await _shipmentRepository.Shipments.
                Where(s => s.CourierId == courierId).
                Include(s => s.Sender).
                Include(s => s.Courier).
                Include(s => s.SourceMachine).
                Include(s => s.DestinationMachine).ToListAsync();
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
            s.SrcPmCourierUnlockCode = _postMachineService.GeneratePostMachineUnlockCode(p);
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

            s.Status = ShipmentStatus.InSourcePostMachine;
            s.SrcPmSenderUnlockCode = null;
            s.SrcPmCourierUnlockCode = _postMachineService.GeneratePostMachineUnlockCode(p);
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
            return await _shipmentRepository.Shipments
                .Where(s => s.DestinationMachineId == postMachineId && s.DestPmReceiverUnlockCode == unlockCode)
                .FirstOrDefaultAsync();
        }
    }
}
