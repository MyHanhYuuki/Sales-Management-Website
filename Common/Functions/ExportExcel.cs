using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Functions
{
    public class ExportExcel
    {
        private readonly string _filePath;
        private readonly byte[] _contents;
        private readonly string _contentType;
        public ExportExcel(string filePath, string contentType = null)
        {
            if (filePath == null) throw new ArgumentNullException("filePath");

            _filePath = filePath;
            _contentType = contentType;
        }
        public ExportExcel(byte[] contents, string contentType)
        {
            if (contents == null) throw new ArgumentNullException("contents");

            _contents = contents;
            _contentType = contentType;
        }
    }
}
