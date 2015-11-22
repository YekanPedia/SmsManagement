using System;
using StructureMap;
using YekanPedia.SmsManagement.Bussiness.Interface;
using YekanPedia.SmsManagement.Bussiness.Implementation;

namespace MellatPortal.DependencyResolver
{
    public static class ObjectFactory
    {
        private static IContainer container;
        public static void Initialize()
        {
            container = new Container(x =>
            {
                x.For<ISendSms>().Use<AsanakSendSms>();
                
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

        public static void DisposeAndClearAll()
        {
            //if (System.Web.HttpContext.Current == null)
            //{
            //    new StructureMap.Web.Pipeline.HybridLifecycle().FindCache(null).DisposeAndClear();
            //}
            //else
            //{
            //    StructureMap.Web.Pipeline.HttpContextLifecycle.DisposeAndClearAll();
            //}
        }

    }
}
