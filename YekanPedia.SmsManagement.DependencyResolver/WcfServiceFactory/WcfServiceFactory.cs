namespace YekanPedia.SmsManagement.DependencyResolver.ServiceFactory
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using DeoendencyResolver.ServiceFactory;

    public class WcfServiceFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new WcfServiceHost(serviceType, baseAddresses);
        }
    }
}
