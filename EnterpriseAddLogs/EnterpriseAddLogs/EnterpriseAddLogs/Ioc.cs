namespace EnterpriseAddLogs
{
    using System;
    using Autofac;

    public static class Ioc
    {
        public static IContainer Container { get; set; }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static object Resolve(Type objectType)
        {
            return Container.Resolve(objectType);
        }
    }
}
