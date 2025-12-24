using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDemo.Application.DTOs.RefreshTokenDtos
{
    public class RefreshTokenUpdateDto
    {
        public bool IsRevoked { get; set; } = false;
    }
}
