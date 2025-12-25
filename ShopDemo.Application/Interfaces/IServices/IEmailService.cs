using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDemo.Application.Interfaces.IServices
{
    public interface IEmailService
    {
        Task SendOtpEmailAsync(string email, string otp);
    }
}
