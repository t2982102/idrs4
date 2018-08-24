using idrs4.PoJo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idrs4.Services
{
    public interface ISMSSender
    {
        Task<Result> SendSMSAsync(string PhoneNumber,string SMSContent);

    }
}
