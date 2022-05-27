namespace shipping_service.Models
{
    public enum DbUpdateResult
    {
        Success,
        // Indicates that the entity was updated by another user.
        // The entity changes were not saved.
        ConcurrentUpdateError
    }
}
