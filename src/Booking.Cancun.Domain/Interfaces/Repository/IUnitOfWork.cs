namespace Booking.Cancun.Domain.Interfaces.Repository;

public interface IUnitOfWork
{
    Task<bool> CommitAsync(CancellationToken cancellationToken = default);
}
