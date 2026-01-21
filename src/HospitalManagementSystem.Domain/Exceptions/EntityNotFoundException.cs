namespace HospitalManagementSystem.Domain.Exceptions
{
    // Exception ketika entity tidak ditemukan
    public class EntityNotFoundException : DomainException
    {
        public string EntityName { get; }
        public object EntityId { get; }

        public EntityNotFoundException(string entityName, object entityId)
            : base($"{entityName} with ID '{entityId}' was not found.")
        {
            EntityName = entityName;
            EntityId = entityId;
        }

        public EntityNotFoundException(string entityName, object entityId, string message)
            : base(message)
        {
            EntityName = entityName;
            EntityId = entityId;
        }
    }
}
