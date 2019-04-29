using System;
using System.Collections.Generic;
using System.Text;
using VOD.Database.Contexts;

namespace VOD.Database.Services
{
    public class DbWriteService : IDbWriteService
    {
        #region Properties
        private readonly VODContext _db;
        #endregion

        #region Constructor
        public DbWriteService(VODContext db)
        {
            _db = db;
        }
        #endregion
    }
}
