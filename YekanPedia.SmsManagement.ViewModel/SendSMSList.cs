using System;

namespace YekanPedia.SmsManagement.ViewModel
{
    public class SendSMSList
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string[] DestinationNumbers { get; set; }
        public string[] Messages { get; set; }
    }
}