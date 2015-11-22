using System;

namespace YekanPedia.SmsManagement.ViewModel
{
    public class SentSMS
    {
        public MainSMSStatus DeliveryStatus { get; set; }
        public DateTime SentDateTime { get; set; }
        public string SourceNumber { get; set; }
        public string DestinationNumber { get; set; }
        public string Message { get; set; }
    }
}