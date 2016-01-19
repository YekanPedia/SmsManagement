namespace YekanPedia.SmsManagement.Service.Implement
{
    using System;
    using System.Linq;
    using Domain;
    using Interfaces;
    using ExternalService.Interfaces;

    public class SmsService : ISmsService
    {
        readonly IAsanakProviderAdaper _smsService;
        public SmsService(IAsanakProviderAdaper smsService)
        {
            _smsService = smsService;
        }

        public CreditResult GetCredit()
        {
            return _smsService.GetCredit();
        }

        public SmsServiceSendResult Send(string sourceTel, string[] destinationTels, string[] messages)
        {
            var result = new SmsServiceSendResult();
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
                var send = _smsService.Send(sourceTels, destinationTels, messages, unicode);
                result.Status = SmsSendStatus.Success;
                result.StatusMessage = send.ResultMessage;
                #endregion
            }
            catch (Exception ex)
            {
                result.Status = SmsSendStatus.Error;
                result.StatusMessage = "Exception : " + ex.Message;
            }
            return result;
        }
    }
}
