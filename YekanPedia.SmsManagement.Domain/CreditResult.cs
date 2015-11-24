namespace YekanPedia.SmsManagement.Domain
{
    public class CreditResult
    {
        public SmsSendStatus Status { get; set; }
        public long Number { get; set; }
        public string ResultMessage { get; set; }
    }
}
