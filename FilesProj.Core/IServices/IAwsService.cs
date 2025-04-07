using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesProj.Core.IServices
{
    public interface IAwsService
    {
        Task<string> GetPreSignedUrlAsync(int userId, string path, string fileName, string contentType);

        Task<string> GetDownloadUrlAsync(int userId, string path);
        Task<string> GetPreSignedUrlAsync(string fileName, string contentType);

        Task<string> GetDownloadUrlAsync(string fileName);

    }
}


