namespace YekanPedia.SmsManagement.ViewModel
{
    public enum MainSMSStatus : short
    {
        NoInformation = -1,
        Created = 0,
        Pending = 1,
        Sending = 2,
        SendingError = 3,
        Delivered = 4,
        SendingFail = 5,
        ToManyRequest = 15
    }
}
