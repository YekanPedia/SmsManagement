using System;
using System.Linq;
using YekanPedia.SmsManagement.Bussiness.Interface;
using YekanPedia.SmsManagement.Provider.SmsProvider.Asanak;
using YekanPedia.SmsManagement.ViewModel;

namespace YekanPedia.SmsManagement.Bussiness.Implementation
{
    public class AsanakSendSms : ISendSms
    {
        public SMSServiceSendResult Send(string sourceTel, string[] destinationTels, string[] messages)
        {
            var result = new SMSServiceSendResult();
            try
            {
                #region Prepare For Send SMS
                int sendCount = destinationTels.Count();
                var sourceTels = new string[sendCount];
                var unicode = new int[sendCount];
                for (int i = 0; i < sendCount; i++)
                    sourceTels[i] = sourceTel;
                for (int i = 0; i < sendCount; i++)
                    unicode[i] = 1;
                #endregion

                #region Send SMS
                var smsService = new AsanakSmsProvider();
                var send = smsService.Send(sourceTels, destinationTels, messages, unicode);
                result.Status = SMSServiceSendStatus.Success;
                result.StatusMessage = send.ResultMessage;
                return result;
                #endregion
            }
            catch (Exception ex)
            {
                result.Status = SMSServiceSendStatus.Error;
                result.StatusMessage = "Exception : " + ex.Message; ;
                return result;
            }
        }
    }
}
