namespace YekanPedia.SmsManagement.Provider
{
    using Domain;
    public class ProviderSmsResult
    {
        public SmsSendStatus Status { get; set; }
        public string ResultMessage { get; set; }
        public long?[] MessageID { get; set; }
    }
}
