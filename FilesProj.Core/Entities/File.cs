using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FilesProj.Core.Entities
{
    [Table("Files")]
    public class File
    {
        [Key]
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [JsonIgnore]

        public User User { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Path { get; set; }
        [MaxLength(500)]
        public string FullPath { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Size { get; set; }
        //public bool IsDeleted { get; set; } = false;
        public int? FolderId { get; set; }
        [ForeignKey(nameof(FolderId))]
        [JsonIgnore]

        public Folder? Folder { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;


    }
}
