namespace YekanPedia.SmsManagement.Provider.SmsProvider.Asanak
{
    public interface IAsanakSmsProvider
    {
        SendSmsResult Send(string[] SourceTels, string[] DestinationTels, string[] Messages, int[] Encoding);

        ReciveSmsResult Receive(long LastReceiveMilisecond, string[] Receivers = null, int Count = 100);

        GetSmsStatusResult GetSmsStatus(long[] MessageIDs);

        GetCreditResult GetCredit();

        int BalanceLimit();
        
    }
}
