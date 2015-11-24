namespace YekanPedia.SmsManagement.InfraStructure
{
    using System;
    using System.Text.RegularExpressions;

    public static class Validation
    {
        public static bool IsValidMobile(string Mobile)
        {
            try
            {
                var rgx = new Regex(@"^(09|9|989|0989|\+989)[0-9]{9}$");
                return rgx.IsMatch(Mobile);
            }
            catch
            {
                return false;
            }
        }
        public static string SetMobilePattern(string mobile)
        {
            string Pattern = mobile;

            if (string.IsNullOrEmpty(mobile)) return null;
            switch (mobile.Length)
            {
                case 10:
                    {
                        Pattern = "98" + mobile;
                        break;
                    }
                case 11:
                    {
                        if (mobile.StartsWith("0", StringComparison.Ordinal))
                        {
                            Pattern = "98" + mobile.Substring(1);
                        }
                        break;
                    }
                case 13:
                    {
                        Pattern = mobile.Substring(1);
                        break;
                    }
            }
            return Pattern;
        }
    }
}
