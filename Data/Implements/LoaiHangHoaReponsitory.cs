using Common.Model;
using Data.Functions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;

namespace Data.Implements
{
    public class LoaiHangHoaReponsitory : GenericReponsitoryFuctions<LoaiHangHoa>, ILoaiHangHoaRepository
    {
        public LoaiHangHoaReponsitory(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
