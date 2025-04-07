using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FilesProj.Core.Entities
{
    [Table("FramedImgs")]

    public class FramedImg
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        
        public int ImageId { get; set; }
        [ForeignKey(nameof(ImageId))]
        public File Iamge { get; set; }
        public int FrameId { get; set; }
        [ForeignKey(nameof(FrameId))]
        public Frame Frame { get; set; }
        //public string Path { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        

    }
}
