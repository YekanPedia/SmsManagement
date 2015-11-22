
namespace YekanPedia.SmsManagement.ViewModel
{
    public class SMSServiceSendResult
    {
        public SMSServiceSendStatus Status { get; set; }
        public string StatusMessage { get; set; }
        public short[] DestinationNumberStatus { get; set; }
        public long[] VerificationCode { get; set; }
    }
}