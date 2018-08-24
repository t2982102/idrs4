
using idrs4.PoJo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace idrs4.Services
{
    public class SMSSender : ISMSSender
    {
        public IConfiguration _configuration;
        public string accountname;
        public string accountpwd;

        public string smssendadress;

        public string smsblackpath;
        public SMSSender(IConfiguration configuration)
        {

            _configuration = configuration;
            accountname = _configuration.GetSection("SMSConfig")["accountname"];
            accountpwd = _configuration.GetSection("SMSConfig")["accountpwd"];
            smssendadress = _configuration.GetSection("SMSConfig")["smssendadress"];
            smsblackpath = _configuration.GetSection("SMSConfig")["smsblackpath"];
        }
        public async Task<Result> SendSMSAsync(string PhoneNumber, string SMSContent)
        {
            
            Result rs = new Result();
            foreach (string str in System.IO.File.ReadAllLines(smsblackpath, Encoding.Default))
            {
                string newStr = "";
                if (SMSContent.Contains(str))
                {
                    foreach (char c in str)
                    {
                        newStr += c + "_";
                    }
                }
                newStr = newStr.TrimEnd('_');
                SMSContent = SMSContent.Replace(str, newStr);
            }

            Result rsj = new Result();
            //ResultJson rsj = new ResultJson();
            string tmp = "";
            if (PhoneNumber != "")
            {
                string accountname = this.accountname;
                string accountpwd = this.accountpwd;
                Encoding encoding = Encoding.GetEncoding("GBK");
                string postData = "accountname=" + accountname;
                postData += ("&accountpwd=" + accountpwd);
                postData += ("&mobilecodes=" + PhoneNumber);
                postData += ("&msgcontent=" + SMSContent + "【安阳益和热力】");
                byte[] data = encoding.GetBytes(postData);
                // Prepare web request
                //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://csdk.zzwhxx.com:8002/submitsms.aspx");
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(smssendadress);
                myRequest.Method = "POST";
                myRequest.Timeout = 10000;
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentLength = data.Length;
                // 异步发送短信 
                Stream newStream = await myRequest.GetRequestStreamAsync();
                // Send the data.
                newStream.Write(data, 0, data.Length);
                newStream.Close();
                //接收返回信息：
                HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse();
                StreamReader sreader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                tmp = sreader.ReadToEnd();
            }

            switch (tmp)
            {
                case "0":
                    rsj.ErrorCode = 0;
                    rsj.ErrorMessage = "成功";
                    break;
                case "-1":
                    rsj.ErrorCode = -1;
                    rsj.ErrorMessage = "系统异常";
                    break;
                case "-60":
                    rsj.ErrorCode = -60;
                    rsj.ErrorMessage = "账户停用";
                    break;
                case "100":
                    rsj.ErrorCode = 100;
                    rsj.ErrorMessage = "系统异常";
                    break;
                case "101":
                    rsj.ErrorCode = 101;
                    rsj.ErrorMessage = "消息结构错 参数错误";
                    break;
                case "102":
                    rsj.ErrorCode = 102;
                    rsj.ErrorMessage = "帐户认证错误";
                    break;
                case "103":
                    rsj.ErrorCode = 103;
                    rsj.ErrorMessage = "提交短信余额不足";
                    break;
                case "104":
                    rsj.ErrorCode = 104;
                    rsj.ErrorMessage = "提交短信帐户计费失败";
                    break;
                case "105":
                    rsj.ErrorCode = 105;
                    rsj.ErrorMessage = "提交短信号码数目和解析数目不一致";
                    break;
                case "106":
                    rsj.ErrorCode = 106;
                    rsj.ErrorMessage = "黑字典不通过";
                    break;
                case "110":
                    rsj.ErrorCode = 110;
                    rsj.ErrorMessage = "帐户状态异常";
                    break;
                case "200":
                    rsj.ErrorCode = 200;
                    rsj.ErrorMessage = "网络通信异常";
                    break;
                default:
                    break;
            }
            return rsj;
            //throw new NotImplementedException();
        }
    }
}
