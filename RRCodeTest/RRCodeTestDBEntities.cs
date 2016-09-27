using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRCodeTest
{
    public partial class RRCodeTestDBEntities : DbContext
    {
        /// <summary>
        /// Instantiates a RRCodeTestDBEntities object using the EDM connection string.
        /// </summary>
        /// <param name="connectionString">EDM Connection string</param>
        public RRCodeTestDBEntities(string connectionString)
            : base(connectionString)
        {
        }
    }
}
