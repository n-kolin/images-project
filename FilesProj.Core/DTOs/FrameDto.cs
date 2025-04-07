using FilesProj.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesProj.Core.DTOs
{
    public class FrameDto
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }

        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public decimal Size { get; set; }
        //public bool IsDeleted { get; set; } = false;
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
