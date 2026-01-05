using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class CreateAttachmentDto
    {
        [Required]
        [MaxLength(255)]
        public string FileName { get; set; }

        [Required]
        [MaxLength(50)]
        public string FileType { get; set; } 

        [Required]
        public string FilePath { get; set; }
    }
}
