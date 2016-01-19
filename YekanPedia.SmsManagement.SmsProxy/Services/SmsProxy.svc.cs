namespace YekanPedia.SmsManagement.SmsProxy
{
    using System;
    using System.Linq;
    using System.ServiceModel;
    using InfraStructure;
    using Domain;
    using Service.Interfaces;

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
         ConcurrencyMode = ConcurrencyMode.Reentrant,
         AddressFilterMode = AddressFilterMode.Any,
         IgnoreExtensionDataObject = true,
         AutomaticSessionShutdown = false,
         ValidateMustUnderstand = false,
         IncludeExceptionDetailInFaults = true)]

    public class SmsProxy : ISmsProxy
    {
        readonly ISmsService _smsService;
        public SmsProxy(ISmsService smsService)
        {
            _smsService = smsService;
        }
        public CreditResult GetCredit()
        {
            return _smsService.GetCredit();
        }
        public SmsServiceSendResult Send(SmsList smsList)
        {
            var result = new SmsServiceSendResult();
            #region Application Validation
            if (AppSettings.UserName != smsList.Username || AppSettings.Password != smsList.Password)
            {
                result.Status = SmsSendStatus.Error;
                result.StatusMessage = ResultMessage.InvalidApplicationUsernamePassword.ToString();
                return result;
            }
            #endregion
            try
            {
                #region Check Parameter Value
                if (
                            (string.IsNullOrEmpty(smsList.Username) || string.IsNullOrEmpty(smsList.Password)) ||
                            (smsList.DestinationNumbers == null || smsList.DestinationNumbers.Count() == 0 || smsList.DestinationNumbers.Count(x => string.IsNullOrEmpty(x)) > 0) ||
                            (smsList.Messages == null || smsList.Messages.Count() == 0 || smsList.Messages.Count(x => string.IsNullOrEmpty(x)) > 0)
                    )
                {
                    result.Status = SmsSendStatus.Error;
                    result.StatusMessage = ResultMessage.InvalidParameter.ToString();
                    return result;
                }

                if (smsList.DestinationNumbers.Count() > 1 && smsList.Messages.Count() > 1 && smsList.DestinationNumbers.Count() != smsList.Messages.Count())
                {
                    result.Status = SmsSendStatus.Error;
                    result.StatusMessage = ResultMessage.InvalidParameterNumber.ToString();
                    return result;
                }
                #endregion
                #region Check Destination Number
                var destinationNumberStatus = new short[smsList.DestinationNumbers.Count()];
                for (int i = 0; i < smsList.DestinationNumbers.Count(); i++)
                {
                    if (Validation.IsValidMobile(smsList.DestinationNumbers[i]))
                    {
                        destinationNumberStatus[i] = 0;
                        smsList.DestinationNumbers[i] = Validation.SetMobilePattern(smsList.DestinationNumbers[i]);
                    }
                    else
                    {
                        destinationNumberStatus[i] = -1;
                    }
                }

                if (destinationNumberStatus.Any(x => x == -1))
                {
                    result.Status = SmsSendStatus.Error;
                    result.StatusMessage = ResultMessage.InvalidDestinationNumber.ToString();
                    result.DestinationNumberStatus = destinationNumberStatus;
                    return result;
                }
                #endregion
                var sourceTel = "sourceTel";
                if (smsList.DestinationNumbers.Count() == 1)
                {
                    #region Send One SMS
                    result = _smsService.Send(sourceTel, smsList.DestinationNumbers, smsList.Messages);
                    result.DestinationNumberStatus = destinationNumberStatus;
                    #endregion
                }
                else if (smsList.DestinationNumbers.Count() > 1 && smsList.Messages.Count() == 1)
                {
                    #region Send Many SMS With One Message
                    var message = new string[smsList.DestinationNumbers.Count()];
                    for (int i = 0; i < smsList.DestinationNumbers.Count(); i++)
                        message[i] = smsList.Messages[0];
                    smsList.Messages = message;
                    result = _smsService.Send(sourceTel, smsList.DestinationNumbers, message);
                    result.DestinationNumberStatus = destinationNumberStatus;
                    #endregion
                }
                else if (smsList.DestinationNumbers.Count() > 1 && smsList.Messages.Count() > 1)
                {
                    #region Send Many SMS With Many Message
                    result = _smsService.Send(sourceTel, smsList.DestinationNumbers, smsList.Messages);
                    result.DestinationNumberStatus = destinationNumberStatus;
                    #endregion
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Status = SmsSendStatus.Error;
                result.StatusMessage = ex.Message;
                return result;
            }
        }
    }
}
