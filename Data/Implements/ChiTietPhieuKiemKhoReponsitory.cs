using Common.Model;
using Data.Functions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implements
{
    public class ChiTietPhieuKiemKhoReponsitory : GenericReponsitoryFuctions<ChiTietPhieuKiemKho>
    {
        public ChiTietPhieuKiemKhoReponsitory(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
