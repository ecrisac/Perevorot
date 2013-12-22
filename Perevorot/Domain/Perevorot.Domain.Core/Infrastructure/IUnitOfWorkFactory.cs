namespace Perevorot.Domain.Core.Infrastructure
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();

        PerevorotEntities GetCurrentContext();
    }
}