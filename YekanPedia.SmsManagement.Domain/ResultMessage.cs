namespace YekanPedia.SmsManagement.Domain
{
    public enum ResultMessage : short
    {
        SendSuccess = 0,
        InvalidApplicationUsernamePassword = -1,
        InvalidApplicationIP = -2,
        DeactiveApplicationToSendSMS = -3,
        InvalidParameter = -4,
        InvalidParameterNumber = -5,
        InvalidDestinationNumber = -6,
        Error = -7,
        InvalidSourceNumber = -8,
        DontAllowDuplicate = -9,
        OverLimitDuplicateSend = 10
    }
}
