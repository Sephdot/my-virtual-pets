using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_virtual_pets_class_library.DTO
{
    public class UpdateUserDTO
    {
        public Guid UserId { get; set; }
        public string? NewUsername { get; set; }
        public string? NewPassword { get; set; }
    }
}
