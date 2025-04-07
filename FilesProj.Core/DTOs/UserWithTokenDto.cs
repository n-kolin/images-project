using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesProj.Core.DTOs
{
    public class UserWithTokenDto
    {
        public UserDto UserDto { get; set; }
        public string Token { get; set; }
    }
}
