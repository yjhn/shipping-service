using shipping_service.Models;
using shipping_service.Persistence.Entities;

namespace shipping_service.Services
{
    public interface IShipmentService
    {
        IQueryable<Shipment> Shipments { get; }
        Task<Shipment?> GetById(long id);
        Task<Shipment?> GetByIdBypassCache(long id);
        Task CreateAsync(Shipment s);
        Task ChangeShipmentStatusToSrc(Shipment s, PostMachine p);
        Task ChangeShipmentStatusToDest(Shipment s, PostMachine p);
        Task ChangeShipmentStatusToDelivered(Shipment s);
        Task ChangeShipmentStatusToShipping(Shipment s, PostMachine p);
        Task<Shipment?> GetShFromSrcSenderCode(long postMachineId, int unlockCode);
        Task<Shipment?> GetShFromSrcCourierCode(long postMachineId, int unlockCode);
        Task<Shipment?> GetShFromDestCourierCode(long postMachineId, int unlockCode);
        Task<Shipment?> GetShFromDestReceiverCode(long postMachineId, int unlockCode);
        Task<IEnumerable<Shipment>> GetUnassignedInSourceMachineAsync();
        IEnumerable<Shipment> GetUnassignedInSourceMachine();
        Task<IEnumerable<Shipment>> GetAssignedAsync(long courierId);
        string GenerateIdHash(long id);
        bool IsValidIdHash(long id, string hash);
        Task<Shipment?> SelectIncludeAll(long id);
        Task<DbUpdateResult> AssignShipmentToCourier(Shipment s, Courier c);
        Task<DbUpdateResult> UnassignShipment(Courier c, Shipment s);
        void AssignFrom(Shipment from, Shipment to);
    }
}
