using Booking.Cancun.Domain.Notifications;

namespace Booking.Cancun.Domain.Exceptions;

public class DomainException : Exception
{
    private readonly List<DomainNotification> _domainNotifications;

    public DomainException(List<DomainNotification> domainNotifications)
    {
        _domainNotifications = domainNotifications ?? new List<DomainNotification>();
    }

    public DomainException(DomainNotification domainNotification)
    {
        _domainNotifications = new List<DomainNotification>();
        _domainNotifications.Add(domainNotification);
    }

    public IReadOnlyCollection<DomainNotification> DomainNotifications => _domainNotifications.AsReadOnly();
}
