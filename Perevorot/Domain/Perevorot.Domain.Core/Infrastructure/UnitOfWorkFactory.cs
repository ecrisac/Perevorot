namespace Perevorot.Domain.Core.Infrastructure
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork Create()
        {
            return new UnitOfWork();
        }

        public PerevorotEntities GetCurrentContext()
        {
            return UnitOfWork.GetContext();
        }
    }
}