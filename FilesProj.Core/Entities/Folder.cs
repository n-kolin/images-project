using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace FilesProj.Core.Entities
{
    [Table("Folders")]

    public class Folder
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [JsonIgnore]

        public User User { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey(nameof(ParentId))]
        [JsonIgnore]

        public Folder? Parent { get; set; }
        [JsonIgnore]

        public List<Folder> SubFolders { get; set; } = new List<Folder>();
        [JsonIgnore]

        public List<File> Files { get; set; } = new List<File>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

    }
}
