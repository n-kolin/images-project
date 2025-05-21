using FilesProj.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesProj.Core.IServices
{
    public interface IPyService
    {
        Task<PyResponse> ProcessPromptAsync(PromptModel promptModel);

    }
}
