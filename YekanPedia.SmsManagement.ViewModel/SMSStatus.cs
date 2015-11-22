namespace YekanPedia.SmsManagement.ViewModel
{
    public enum SMSStatus : byte
    {
        Waiting = 1,
        Sending = 2,
        Pending = 3,
        HasError = 4,
        Sent = 5,
        Deleted = 6,
        NoCharge = 7
    }
}
