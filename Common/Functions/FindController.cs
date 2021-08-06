using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Functions
{
    public class FindController
    {
        public static string Controller(string authorityName)
        {
            return authorityName.Replace(" ", "");
        }
    }
}
