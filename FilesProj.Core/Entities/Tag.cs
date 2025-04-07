using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesProj.Core.Entities
{
    [Table("Tags")]

    public class Tag
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public List<File> Files { get; set; } = new List<File>();
        public List<Frame> Frames { get; set; } = new List<Frame>();

    }
}
