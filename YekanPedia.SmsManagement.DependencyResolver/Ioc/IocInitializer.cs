namespace YekanPedia.SmsManagement.DependencyResolver
{
    using System;
    using StructureMap;
    using Service.Interfaces;
    using StructureMap.Web.Pipeline;
    using Service.Implement;
    using ExternalService.Interfaces;
    using ExternalService.Implement;

    public static class IocInitializer
    {
        static IContainer container;
        public static void Initialize()
        {
            container = new Container(x =>
            {
                x.For<ISmsService>().Use<SmsService>();
                x.For<IAsanakProviderAdaper>().Use<AsanakProviderAdaper>();
            });
        }
        public static object GetInstance(Type pluginType)
        {
            return container.GetInstance(pluginType);
        }
        public static TPluginType GetInstance<TPluginType>()
        {
            return container.GetInstance<TPluginType>();
        }
        public static void HttpContextDisposeAndClearAll()
        {
            HttpContextLifecycle.DisposeAndClearAll();
        }
    }
}
