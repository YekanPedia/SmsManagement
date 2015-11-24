namespace YekanPedia.SmsManagement.InfraStructure
{
    using System;
    using System.Collections.Generic;
    public static class SmsUtility
    {
        public static int MaxSmsLengthUnicode = 420;
        public static int MaxSmsLengthNonUnicode = 750;
        public static List<string> SmsSegmentChecker(string Message, bool PersianEncoding)
        {
            var result = new List<string>();
            int len = 0, offset = 0;
            var append = "";
            if (PersianEncoding)
            {
                while (offset < Message.Length)
                {
                    len = Math.Min(Message.Length - offset, MaxSmsLengthUnicode);
                    if (len == MaxSmsLengthUnicode) append = "...";
                    else append = "";
                    result.Add(Message.Substring(offset, len - append.Length) + append);

                    offset += len - append.Length;
                }
            }
            else
            {
                len = Math.Min(Message.Length - offset, MaxSmsLengthNonUnicode);
                if (len == MaxSmsLengthNonUnicode) append = "...";
                else append = "";
                result.Add(Message.Substring(offset, len - append.Length) + append);

                offset += len - append.Length;
            }

            return result;
        }
    }
}