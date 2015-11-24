namespace YekanPedia.SmsManagement.SmsProxy
{
    using System.Net.Security;
    using System.ServiceModel;
    using Domain;

    [ServiceContract(SessionMode = SessionMode.Allowed, ProtectionLevel = ProtectionLevel.None)]
    public interface ISmsProxy
    {
        [OperationContract]
        SmsServiceSendResult Send(SmsList smsList);

        [OperationContract]
        CreditResult GetCredit();
    }
}
