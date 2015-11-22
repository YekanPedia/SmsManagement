using YekanPedia.SmsManagement.ViewModel;

namespace YekanPedia.SmsManagement.Bussiness.Interface
{
    public interface ISendSms
    {
        SMSServiceSendResult Send(string sourceTels, string[] destinationTels, string[] messages);

    }
}