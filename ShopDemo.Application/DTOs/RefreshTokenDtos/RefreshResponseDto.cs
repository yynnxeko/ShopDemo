using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDemo.Application.DTOs.RefreshTokenDtos
{
    public record RefreshResponseDto(string AccessToken, string RefreshToken, string Message);
}
