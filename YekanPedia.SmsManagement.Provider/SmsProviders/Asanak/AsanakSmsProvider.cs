using System;
using System.Linq;
using YekanPedia.SmsManagement.Provider.AsanakProvider;

namespace YekanPedia.SmsManagement.Provider.SmsProvider.Asanak
{
    public class AsanakSmsProvider : IAsanakSmsProvider
    {
        public static readonly CompositeSmsGateway _webService = new CompositeSmsGatewayClient();
        public static readonly userCredential _userCredential = new userCredential();
        private static readonly string _userName = "";
        private static readonly string _passWord = ""; 
        private static readonly int _balanceLimit = 400000;

        /// <summary>
        /// Set The Correct Milisecond To Call Receive Method
        /// </summary>
        /// <param name="Milisecond">long Milisecond</param>
        /// <returns></returns>
        private long CheckCallTime(long Milisecond)
        {
            var date = (new DateTime(1970, 1, 1, 3, 30, 0).AddMilliseconds(Milisecond)).AddHours(1);
            if ((DateTime.Now - date) > (DateTime.Now - DateTime.Now.AddDays(-1)))
            {
                return (long)(DateTime.Now.AddHours(-5) - new DateTime(1970, 1, 1, 3, 30, 0)).TotalMilliseconds;
            }
            return Milisecond;
        }

        public AsanakSmsProvider()
        {
            _userCredential.username = _userName;
            _userCredential.password = _passWord;
        }

        /// <summary>
        /// Send SMS With Asanak Provider
        /// </summary>
        /// <param name="SourceTels">string[] SourceTels</param>
        /// <param name="DestinationTels">string[] DestinationTels</param>
        /// <param name="Messages">string[] Messages</param>
        /// <param name="Encoding">int[] Encoding</param>
        /// <returns></returns>
        public SendSmsResult Send(string[] SourceTels, string[] DestinationTels, string[] Messages, int[] Encoding)
        {
            var _sendSmsResult = new SendSmsResult();

            #region Send SMS
            try
            {
                var _sms = new sendSms(_userCredential, SourceTels, DestinationTels, Messages, Encoding);
                var _sendResult = _webService.sendSms(_sms);

                _sendSmsResult.Status = _sendResult.@return.status == 0 ? SMSServiceStatusType.Success : SMSServiceStatusType.Error;
                _sendSmsResult.ResultMessage = _sendResult.@return.errorMsg;
                _sendSmsResult.MessageID = _sendResult.@return.msgIdArray;

                if (_sendResult.@return.status == 0)
                {
                    _sendSmsResult.Status = SMSServiceStatusType.Success;
                    _sendSmsResult.MessageID = _sendResult.@return.msgIdArray;
                    _sendSmsResult.ResultMessage = "Success";
                }
                else if (_sendResult.@return.status == 2)
                {
                    _sendSmsResult.Status = SMSServiceStatusType.Error;
                    _sendSmsResult.MessageID = null;
                    _sendSmsResult.ResultMessage = "Not Enough Credit";
                }
                else
                {
                    _sendSmsResult.Status = SMSServiceStatusType.Error;
                    _sendSmsResult.MessageID = null;
                    _sendSmsResult.ResultMessage = "Error : " + _sendResult.@return.errorMsg;
                }

                return _sendSmsResult;
            }
            catch (Exception ex)
            {
                _sendSmsResult.Status = SMSServiceStatusType.Error;
                _sendSmsResult.MessageID = null;
                _sendSmsResult.ResultMessage = "Exception : " + ex.Message;

                return _sendSmsResult;
            }
            #endregion
        }

        /// <summary>
        /// Recive SMS From Asanak Provider
        /// </summary>
        /// <param name="DateFrom">DateTime DateFrom</param>
        /// <param name="Receivers">string[] Receivers</param>
        /// <param name="Count">int Count = 100</param>
        /// <returns></returns>
        public ReciveSmsResult Receive(long LastReceiveMilisecond, string[] Receivers = null, int Count = 100)
        {
            var _reciveSmsResult = new ReciveSmsResult();

            #region Recive SMS
            try
            {
                var _receivedSms = new getReceivedMsg(_userCredential, Receivers, CheckCallTime(LastReceiveMilisecond), Count > 100 ? 100 : Count);
                var _receivedSmsResponse = _webService.getReceivedMsg(_receivedSms);
                var Sms = _receivedSmsResponse.@return.receivedMegs;

                if (_receivedSmsResponse.@return.status == 0 && Sms != null)
                {
                    _reciveSmsResult.Status = SMSServiceStatusType.Success;
                    _reciveSmsResult.ResultMessage = "Success";
                    _reciveSmsResult.Messages = Sms;
                }
                else if (_receivedSmsResponse.@return.status == 0 && Sms == null)
                {
                    _reciveSmsResult.Status = SMSServiceStatusType.Success;
                    _reciveSmsResult.ResultMessage = "No New Message Receive";
                    _reciveSmsResult.Messages = null;
                }
                else if (_receivedSmsResponse.@return.status == 31)
                {
                    _reciveSmsResult.Status = SMSServiceStatusType.Error;
                    _reciveSmsResult.ResultMessage = "Requested DateTime Is Not Valid";
                    _reciveSmsResult.Messages = null;
                }
                else if (_receivedSmsResponse.@return.status == 501)
                {
                    _reciveSmsResult.Status = SMSServiceStatusType.Error;
                    _reciveSmsResult.ResultMessage = "Asanak Service Replay Busy";
                    _reciveSmsResult.Messages = null;
                }
                else
                {
                    _reciveSmsResult.Status = SMSServiceStatusType.Error;
                    _reciveSmsResult.ResultMessage = "Asanak Service Replay Internal Error";
                    _reciveSmsResult.Messages = null;
                }

                return _reciveSmsResult;
            }
            catch (Exception ex)
            {
                _reciveSmsResult.Status = SMSServiceStatusType.Error;
                _reciveSmsResult.ResultMessage = ex.Message;
                _reciveSmsResult.Messages = null;
                return _reciveSmsResult;
            }
            #endregion
        }

        /// <summary>
        /// Get SMS Status From Asanak Provider
        /// </summary>
        /// <param name="SmsIDs">long[] MessageIDs</param>
        /// <returns></returns>
        public GetSmsStatusResult GetSmsStatus(long[] MessageIDs)
        {
            var _getSmsStatusResult = new GetSmsStatusResult();
            #region Error Handling
            if (MessageIDs == null)
            {
                _getSmsStatusResult.Status = SMSServiceStatusType.Error;
                _getSmsStatusResult.ResultMessage = "برای دریافت وضعیت پیامک ها باید کد مربوط به پیامک های مورد نظر را وارد نمایید.";
                _getSmsStatusResult.SmsStatus = null;
                return _getSmsStatusResult;
            }
            else if (MessageIDs.Count() > 100)
            {
                _getSmsStatusResult.Status = SMSServiceStatusType.Error;
                _getSmsStatusResult.ResultMessage = "برای دریافت وضعیت پیامک ها نباید بیشتر از 100 کد پیامک وارد نمایید.";
                _getSmsStatusResult.SmsStatus = null;
                return _getSmsStatusResult;
            }
            #endregion

            #region Get Sms Status Report
            try
            {
                var _userCredit = new getUserCredit(_userCredential);
                getReportByMsgId _smsReport = new getReportByMsgId(_userCredential, MessageIDs);
                getReportByMsgIdResponse _getSmsReportResponse = _webService.getReportByMsgId(_smsReport);

                _getSmsStatusResult.Status = _getSmsReportResponse.@return.status == 0 ? SMSServiceStatusType.Success : SMSServiceStatusType.Error;
                _getSmsStatusResult.SmsStatus = _getSmsReportResponse.@return.reportItems;
                _getSmsStatusResult.ResultMessage = _getSmsReportResponse.@return.errorMsg;
                return _getSmsStatusResult;
            }
            catch (Exception ex)
            {
                _getSmsStatusResult.Status = SMSServiceStatusType.Error;
                _getSmsStatusResult.SmsStatus = null;
                _getSmsStatusResult.ResultMessage = ex.Message;
                return _getSmsStatusResult;
            }
            #endregion
        }

        /// <summary>
        /// Get Account Credit From Asanak Provider
        /// </summary>
        /// <returns></returns>
        public GetCreditResult GetCredit()
        {
            var _getCreditResult = new GetCreditResult();
            #region Get Account Credit
            try
            {
                var _userCredit = new getUserCredit(_userCredential);
                getUserCreditResponse _getUserCreditResponse = _webService.getUserCredit(_userCredit);

                if (_getUserCreditResponse.@return.status == 0)
                {
                    _getCreditResult.Status = SMSServiceStatusType.Success;
                    _getCreditResult.Number = _getUserCreditResponse.@return.credit;
                    _getCreditResult.ResultMessage = "Success";
                }
                else
                {
                    _getCreditResult.Status = SMSServiceStatusType.Error;
                    _getCreditResult.Number = 0;
                    _getCreditResult.ResultMessage = "Error : " + _getUserCreditResponse.@return.errorMsg;
                }

                return _getCreditResult;
            }
            catch (Exception ex)
            {
                _getCreditResult.Status = SMSServiceStatusType.Error;
                _getCreditResult.Number = 0;
                _getCreditResult.ResultMessage = "Exception : " + ex.Message;

                return _getCreditResult;
            }
            #endregion
        }

        /// <summary>
        /// Get Account Balance Limit From Asanak Provider
        /// </summary>
        /// <returns></returns>
        public int BalanceLimit()
        {
            return _balanceLimit;
        }
    }

    #region Define SMS Status
    /*'پیام ها کلاً با موفقیت ارسال شده اند. 0
            'شناسه کاربري اشتباه داده شده. 1
            'رمز عبور اشتباه داده شده است. 2
            'شماره ارسال کننده مشخص شده جز مجموعه شماره هاي تخصیص داده شده براي مشتري نمی باشد. 3 
            'شماره درگاه (پورت) هاي مشخص شده با نوع پیام مطابقت ندارد. 4
            'مشتري داراي اعتبار کافی براي ارسال پیام نمی باشد. 5
            'نوع پیام مشخص شده قابل پشتیبانی توسط سیستم نمی باشد. 6
            'پارامتر هاي ارسال شده به متد با یکدیگر در تناقض باشند، به عنوان مثال اگر تعداد شماره هاي 7
            '   بازگردانده خواهد شدINVALID COMMAND ، ها یکی نباشد Client ID گیرندگان پیام با تعداد .
            'تعداد پیام هایی که بایستی ارسال شوند از حد مجاز حداکثر تعداد پیام ارسالی در یک فراخوانی سرویس بیشتر است. 8
            'در صورتی که در هنگام فراخوانی شماره ارسال کننده مشخص نشده باشد و براي مشتري درخواست کننده شماره پیش فرض تخصیص داده نشده باشد. این پیغام Error داده می شود. 9
            'مشتري غیر فعال شده است. 10
            'در صورتی که طول پیغام داده شده بیشتر از حداکثر طول مجاز باشد. 11
            'در صورتی که فاصله زمانی بین درخواست ها کمتر از میزان تعیین شده براي این کاربر باشد. 15
     */
    public enum SMSMainStatus : byte
    {
        Success = 0,
        InvalidUsername = 1,
        InvalidPassword = 2,
        InvalidSourceTel = 3,
        InvalidPort = 4,
        DontHaveCredit = 5,
        InvalidEncoding = 6,
        InvalidCommand = 7,
        OutOffRangeSend = 8,
        InvalidSourceAndDestination = 9,
        DeactiveUser = 10,
        OutOffRangeMessageLength = 11,
        BadRequest = 12
    }


    /*
     1 منتظر ارسال
     2 درحال ارسال
     3 منتظر دریافت گیرنده
     4 عدم ارسال
     5 ارسال شده
     6 حذف شده
     7 عدم اعتبار کافی
     */

    public enum SMSStatus : byte
    {
        Waiting = 1,
        Sending = 2,
        Pending = 3,
        HasError = 4,
        Sent = 5,
        Deleted = 6,
        NoCharge = 7
    }
    #endregion


    public enum SMSServiceStatusType : byte
    {
        Success = 0,
        Error = 1,
    }
    public class SendSmsResult
    {
        public SMSServiceStatusType Status { get; set; }
        public string ResultMessage { get; set; }
        public long?[] MessageID { get; set; }
    }
    public class ReciveSmsResult
    {
        public SMSServiceStatusType Status { get; set; }
        public string ResultMessage { get; set; }
        public receivedMsg[] Messages { get; set; }
    }
    public class GetCreditResult
    {
        public SMSServiceStatusType Status { get; set; }
        public long Number { get; set; }
        public string ResultMessage { get; set; }
    }
    public class GetSmsStatusResult
    {
        public SMSServiceStatusType Status { get; set; }
        public reportItem[] SmsStatus { get; set; }
        public string ResultMessage { get; set; }
    }

}
