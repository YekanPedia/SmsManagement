using System;
using System.Linq;
using System.ServiceModel;
using YekanPedia.SmsManagement.Bussiness.Implementation;
using YekanPedia.SmsManagement.InfraStructure.Utility;
using YekanPedia.SmsManagement.ViewModel;

namespace YekanPedia.SmsManagement.SmsService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
        ConcurrencyMode = ConcurrencyMode.Reentrant,
        AddressFilterMode = AddressFilterMode.Any,
        IgnoreExtensionDataObject = true,
        AutomaticSessionShutdown = false,
        ValidateMustUnderstand = false,
        IncludeExceptionDetailInFaults = true)]
    public class SMSService : ISMSService
    {
        /// <summary>
        /// Send Many Sms With One Call
        /// </summary>
        /// <param name="smsList">List Of SMS Property</param>
        /// <returns>Return Send SMS Result</returns>
        public SMSServiceSendResult Send(SendSMSList smsList)
        {
            var result = new SMSServiceSendResult();
            try
            {
                #region Parameter Validation

                #region Check Parameter Value
                if (
                            (string.IsNullOrEmpty(smsList.Username) || string.IsNullOrEmpty(smsList.Password)) ||
                            (smsList.DestinationNumbers == null || smsList.DestinationNumbers.Count() == 0 || smsList.DestinationNumbers.Where(x => string.IsNullOrEmpty(x)).Count() > 0) ||
                            (smsList.Messages == null || smsList.Messages.Count() == 0 || smsList.Messages.Where(x => string.IsNullOrEmpty(x)).Count() > 0)
                    )
                {
                    result.Status = SMSServiceSendStatus.Error;
                    result.StatusMessage = ResultMessage.InvalidParameter.ToString();
                    return result;
                }

                if (smsList.DestinationNumbers.Count() > 1 && smsList.Messages.Count() > 1 && smsList.DestinationNumbers.Count() != smsList.Messages.Count())
                {
                    result.Status = SMSServiceSendStatus.Error;
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

                if (destinationNumberStatus.Where(x => x == -1).Any())
                {
                    result.Status = SMSServiceSendStatus.Error;
                    result.StatusMessage = ResultMessage.InvalidDestinationNumber.ToString();
                    result.DestinationNumberStatus = destinationNumberStatus;
                    return result;
                }
                #endregion

                #endregion

                #region Application Validation
                var appStatus = Validation.IsValidApp(smsList.Username, smsList.Password, Validation.GetIP());
                if (appStatus < 0)
                {
                    result.Status = SMSServiceSendStatus.Error;
                    result.StatusMessage = ResultMessage.InvalidApplicationUsernamePassword.ToString();

                    return result;
                }
                #endregion


                var sourceTel = "sourceTel";
                var sendBussiness = new AsanakSendSms();
                if (smsList.DestinationNumbers.Count() == 1)
                {
                    #region Send One SMS
                    result = sendBussiness.Send(sourceTel, smsList.DestinationNumbers, smsList.Messages);
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
                    result = sendBussiness.Send(sourceTel, smsList.DestinationNumbers, message);
                    result.DestinationNumberStatus = destinationNumberStatus;
                    #endregion
                }
                else if (smsList.DestinationNumbers.Count() > 1 && smsList.Messages.Count() > 1)
                {
                    #region Send Many SMS With Many Message
                    result = sendBussiness.Send(sourceTel, smsList.DestinationNumbers, smsList.Messages);
                    result.DestinationNumberStatus = destinationNumberStatus;
                    #endregion
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Status = SMSServiceSendStatus.Error;
                result.StatusMessage = "Internal Error : " + ex.Message; ;
                return result;
            }
        }

    }
}
