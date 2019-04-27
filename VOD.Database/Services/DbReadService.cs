using VOD.Database.Contexts;

namespace VOD.Database.Services
{
    public class DbReadService : IDbReadService
    {
        #region Properties
        private VODContext _db;
        #endregion

        #region Constructor
        public DbReadService(VODContext db)
        {
            _db = db;
        }
        #endregion

    }
}
