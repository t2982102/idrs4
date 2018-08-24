using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using idrs4.Models;
using idrs4.PoJo;
using idrs4.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace idrs4.Services
{
    public static class EmailSenderExtensions
    {
        
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {

            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email,string title, string body)
        {
            return emailSender.SendEmailAsync(email, title,body);
        }
    }
    public class SendEmailByTianTian
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUrlHelper _Url;
        private readonly IEmailSender _emailSender;
        private readonly IHostingEnvironment _hostingEnvironment;
        public SendEmailByTianTian(UserManager<ApplicationUser> userManager, IUrlHelper Url, IEmailSender emailSender, IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _Url = Url;
            _emailSender = emailSender;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task SendEmailByUserIdAsync(string id,string Scheme)
        {
            Result rs = new Result();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                rs = Result.ErrorResult(-3);
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = _Url.EmailConfirmationLink(user.Id, code, Scheme);
            var email = user.Email;

            string filePath = "/Extensions/EmailType/EmailType.html";
            
            filePath = _hostingEnvironment.ContentRootPath+ filePath;
            string emailhtml = "";
            using (var reader = new StreamReader(filePath))
            {
                emailhtml = reader.ReadToEnd();
            }
            emailhtml = emailhtml.Replace("{0}", callbackUrl).Replace("{1}", DateTime.Now.ToLongDateString().ToString()).Replace("{2}", "尊敬的朋友");
            await _emailSender.SendEmailConfirmationAsync(email,"确定注册信息", emailhtml);
            //var code = await _userManager.GenerateEmailConfirmationTokenAsync(id);
        }
        public async Task SendEmailByUserIdAsync(ApplicationUser user, string Scheme)
        {
            Result rs = new Result();
            //var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                rs = Result.ErrorResult(-3);
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = _Url.EmailConfirmationLink(user.Id, code, Scheme);
            var email = user.Email;

            string filePath = "/Extensions/EmailType/EmailType.html";

            filePath = _hostingEnvironment.ContentRootPath + filePath;
            string emailhtml = "";
            using (var reader = new StreamReader(filePath))
            {
                emailhtml = reader.ReadToEnd();
            }
            emailhtml = emailhtml.Replace("{0}", callbackUrl).Replace("{1}", DateTime.Now.ToLongDateString().ToString()).Replace("{2}", "尊敬的朋友");
            await _emailSender.SendEmailConfirmationAsync(email, "确定注册信息", emailhtml);
            //var code = await _userManager.GenerateEmailConfirmationTokenAsync(id);
        }
    }
}
