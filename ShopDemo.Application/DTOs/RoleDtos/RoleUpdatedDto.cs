using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDemo.Application.DTOs.RoleDtos
{
    public class RoleUpdatedDto
    {
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }
}
