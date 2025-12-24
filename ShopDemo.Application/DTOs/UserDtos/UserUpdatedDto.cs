using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDemo.Application.DTOs.UserDtos
{
    public class UserUpdatedDto
    {
        public string FullName { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
}
