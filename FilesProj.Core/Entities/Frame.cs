using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesProj.Core.Entities
{
    [Table("Frames")]

    public class Frame
    {
        [Key]
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public User User { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Path { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Size { get; set; }
        //public bool IsDeleted { get; set; } = false;
        public List<Tag> Tags { get; set; } = new List<Tag>();

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;



    }
}
