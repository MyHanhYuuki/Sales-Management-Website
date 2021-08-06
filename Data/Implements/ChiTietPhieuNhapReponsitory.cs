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
    public class ChiTietPhieuNhapReponsitory : GenericReponsitoryFuctions<ChiTietPhieuNhap>
    {
        public ChiTietPhieuNhapReponsitory(DbContext dbContext) : base (dbContext)
        {

        }
    }
}
