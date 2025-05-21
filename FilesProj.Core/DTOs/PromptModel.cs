using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesProj.Core.DTOs
{
    public class PromptModel
    {
        public string Prompt { get; set; }
        public Dictionary<string, object> CurrentState { get; set; } =
            new Dictionary<string, object>();
        public Dictionary<string, object> ImageParams { get; set; } =
            new Dictionary<string, object>();
    }
}
