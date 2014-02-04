namespace Perevorot.Domain.Core.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        //TODO: don't use ThreadStatic!
        //http://stackoverflow.com/questions/3531303/threadstatic-member-lose-value-on-every-page-load
        private static PerevorotEntities _context;
        private readonly bool _owner;

        public UnitOfWork()
        {
            if (_context == null)
            {
                _owner = true;
                _context = new PerevorotEntities();
            }
        }

        #region IUnitOfWork Members

        public void Dispose()
        {
            if (_owner)
            {
                _context.Dispose();
                _context = null;
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        #endregion

        public static PerevorotEntities GetContext()
        {
            return _context;
        }
    }
}