namespace YekanPedia.SmsManagement.Domain
{
    public class SmsServiceSendResult
    {
        public SmsSendStatus Status { get; set; }
        public string StatusMessage { get; set; }
        public short[] DestinationNumberStatus { get; set; }
        public long[] VerificationCode { get; set; }
    }
}