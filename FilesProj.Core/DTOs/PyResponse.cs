using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesProj.Core.DTOs
{
    public class PyResponse
    {
        public string Status { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}
