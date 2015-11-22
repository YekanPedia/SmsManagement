using System.Collections.Generic;

namespace YekanPedia.SmsManagement.ViewModel
{
    public class SentSMSReport
    {
        public SMSServiceSendStatus Status { get; set; }
        public ResultMessage StatusMessage { get; set; }
        public List<SentSMS> SentSMSDetails { get; set; }
    }
}