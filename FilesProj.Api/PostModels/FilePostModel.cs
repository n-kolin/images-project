using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FilesProj.Api.PostModels
{
    public class FilePostModel
    {
        
        public string Name { get; set; }
        public string Path { get; set; }
        public string FullPath { get; set; }
        public string Type { get; set; }
        public decimal Size { get; set; }
        public int CreatedBy { get; set; }
        public int? FolderId { get; set; }
    }
}
