using FilesProj.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = FilesProj.Core.Entities.File;

namespace FilesProj.Core.DTOs
{
    public class FolderDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int CreatedBy { get; set; }
        public int? ParentId { get; set; }


        public List<Folder> SubFolders { get; set; } = new List<Folder>();

        public List<File> Files { get; set; } = new List<File>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;

    }
}
