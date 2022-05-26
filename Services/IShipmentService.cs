using shipping_service.Persistence.Entities;

namespace shipping_service.Services
{
    public interface IShipmentService
    {
        IQueryable<Shipment> Shipments { get; }
        IEnumerable<Shipment> GetUnassigned();
        Task<Shipment?> GetById(long id);
        Task CreateAsync(Shipment s);
        Task ChangeShipmentStatusToSrc(Shipment s, PostMachine p);
        Task ChangeShipmentStatusToDest(Shipment s, PostMachine p);
        Task ChangeShipmentStatusToDelivered(Shipment s);
        Task ChangeShipmentStatusToShipping(Shipment s, PostMachine p);
        Task<Shipment?> GetShFromSrcSenderCode(long postMachineId, int unlockCode);
        Task<Shipment?> GetShFromSrcCourierCode(long postMachineId, int unlockCode);
        Task<Shipment?> GetShFromDestCourierCode(long postMachineId, int unlockCode);
        Task<Shipment?> GetShFromDestReceiverCode(long postMachineId, int unlockCode);
    }
}
