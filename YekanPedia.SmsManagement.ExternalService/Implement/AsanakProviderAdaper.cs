namespace YekanPedia.SmsManagement.ExternalService.Implement
{
    using System;
    using Domain;
    using Interfaces;
    using Provider.AsanakProvider;
    using Provider;

    public class AsanakProviderAdaper : IAsanakProviderAdaper
    {
        readonly CompositeSmsGateway _webService;
        readonly userCredential _userCredential;
        static readonly string _userName = "";
        static readonly string _passWord = "";
        public AsanakProviderAdaper()
        {
            _userCredential.username = _userName;
            _userCredential.password = _passWord;
            _webService = new CompositeSmsGatewayClient();
            _userCredential = new userCredential();
        }
        public ProviderSmsResult Send(string[] SourceTels, string[] DestinationTels, string[] Messages, int[] Encoding)
        {
            var _sendSmsResult = new ProviderSmsResult();
            #region Send SMS
            try
            {
                var _sms = new sendSms(_userCredential, SourceTels, DestinationTels, Messages, Encoding);
                var _sendResult = _webService.sendSms(_sms);

                _sendSmsResult.Status = _sendResult.@return.status == 0 ? SmsSendStatus.Success : SmsSendStatus.Error;
                _sendSmsResult.ResultMessage = _sendResult.@return.errorMsg;
                _sendSmsResult.MessageID = _sendResult.@return.msgIdArray;

                if (_sendResult.@return.status == 0)
                {
                    _sendSmsResult.Status = SmsSendStatus.Success;
                    _sendSmsResult.MessageID = _sendResult.@return.msgIdArray;
                    _sendSmsResult.ResultMessage = "Success";
                }
                else if (_sendResult.@return.status == 2)
                {
                    _sendSmsResult.Status = SmsSendStatus.Error;
                    _sendSmsResult.MessageID = null;
                    _sendSmsResult.ResultMessage = "Not Enough Credit";
                }
                else
                {
                    _sendSmsResult.Status = SmsSendStatus.Error;
                    _sendSmsResult.MessageID = null;
                    _sendSmsResult.ResultMessage = "Error : " + _sendResult.@return.errorMsg;
                }

                return _sendSmsResult;
            }
            catch (Exception ex)
            {
                _sendSmsResult.Status = SmsSendStatus.Error;
                _sendSmsResult.MessageID = null;
                _sendSmsResult.ResultMessage = "Exception : " + ex.Message;

                return _sendSmsResult;
            }
            #endregion
        }
        public CreditResult GetCredit()
        {
            var _getCreditResult = new CreditResult();
            #region Get Account Credit
            try
            {
                var _userCredit = new getUserCredit(_userCredential);
                getUserCreditResponse _getUserCreditResponse = _webService.getUserCredit(_userCredit);

                if (_getUserCreditResponse.@return.status == 0)
                {
                    _getCreditResult.Status = SmsSendStatus.Success;
                    _getCreditResult.Number = _getUserCreditResponse.@return.credit;
                    _getCreditResult.ResultMessage = "Success";
                }
                else
                {
                    _getCreditResult.Status = SmsSendStatus.Error;
                    _getCreditResult.Number = 0;
                    _getCreditResult.ResultMessage = "Error : " + _getUserCreditResponse.@return.errorMsg;
                }

                return _getCreditResult;
            }
            catch (Exception ex)
            {
                _getCreditResult.Status = SmsSendStatus.Error;
                _getCreditResult.Number = 0;
                _getCreditResult.ResultMessage = "Exception : " + ex.Message;

                return _getCreditResult;
            }
            #endregion
        }
       
    }
}
