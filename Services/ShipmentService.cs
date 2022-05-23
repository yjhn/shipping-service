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

        public IEnumerable<Shipment> GetUnassigned()
        {
            return _shipmentRepository.Shipments.Where(s => s.CourierId == null);
        }

        public async Task<Shipment?> GetById(long id)
        {
            return await _shipmentRepository.GetAsync(id);
        }

        public void ChangeShipmentStatusToSrc(Shipment s, PostMachine p)
        {
            if (s.Status != ShipmentStatus.RegisteredForSending)
            {
                throw new InvalidOperationException("Cannot change status: " + s.Status
                                                                             + " to ShipmentStatus.InSourcePostMachine");
            }

            s.Status = ShipmentStatus.InSourcePostMachine;
            s.SrcPmSenderUnlockCode = null;
            s.SrcPmCourierUnlockCode = _postMachineService.GeneratePostMachineUnlockCode(p);
        }

        public void ChangeShipmentStatusToDest(Shipment s, PostMachine p)
        {
            if (s.Status != ShipmentStatus.Shipping)
            {
                throw new InvalidOperationException("Cannot change status: " + s.Status
                                                                             + " to ShipmentStatus.InDestinationPostMachine");
            }

            s.Status = ShipmentStatus.InDestinationPostMachine;
            s.DestPmCourierUnlockCode = null;
            // Receiver unlock code is generated when the shipment is created.
        }

        public void ChangeShipmentStatusToDelivered(Shipment s)
        {
            if (s.Status != ShipmentStatus.InDestinationPostMachine)
            {
                throw new InvalidOperationException("Cannot change status: " + s.Status
                                                                             + " to ShipmentStatus.Delivered");
            }

            s.Status = ShipmentStatus.Delivered;
            s.DestPmReceiverUnlockCode = null;
        }

        public void ChangeShipmentStatusToShipping(Shipment s, PostMachine p)
        {
            if (s.Status != ShipmentStatus.InSourcePostMachine)
            {
                throw new InvalidOperationException("Cannot change status: " + s.Status
                                                                             + " to ShipmentStatus.Shipping");
            }

            s.Status = ShipmentStatus.InSourcePostMachine;
            s.SrcPmSenderUnlockCode = null;
            s.SrcPmCourierUnlockCode = _postMachineService.GeneratePostMachineUnlockCode(p);
        }
    }
}
