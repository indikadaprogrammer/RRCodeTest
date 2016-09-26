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
        public RRCodeTestDBEntities(string connectionString)
            : base(connectionString)
        {
        }
    }
}
