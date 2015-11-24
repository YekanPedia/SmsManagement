namespace YekanPedia.SmsManagement.Service.Interfaces
{
    using Domain;
    public interface ISmsService
    {
        SmsServiceSendResult Send(string sourceTels, string[] destinationTels, string[] messages);
        CreditResult GetCredit();
    }
}