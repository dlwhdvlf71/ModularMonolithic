using MediatR;

namespace Shared.DDD
{
    public interface IDomainEvent : INotification
    {
        public Guid EventId => Guid.NewGuid();
        public DateTime OccurredOn => DateTime.UtcNow;
        public string EventyType => GetType().AssemblyQualifiedName!;
    }
}