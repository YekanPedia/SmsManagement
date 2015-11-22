using System.Net.Security;
using System.ServiceModel;
using YekanPedia.SmsManagement.ViewModel;

namespace YekanPedia.SmsManagement.SmsService
{
    [ServiceContract(SessionMode = SessionMode.Allowed,
        ProtectionLevel = ProtectionLevel.None)]
    public interface ISMSService
    {
        [OperationContract]
        SMSServiceSendResult Send(SendSMSList SMSList);

    }
}
