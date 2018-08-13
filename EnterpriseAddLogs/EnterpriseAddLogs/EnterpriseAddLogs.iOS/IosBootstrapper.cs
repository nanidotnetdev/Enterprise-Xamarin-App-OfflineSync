namespace EnterpriseAddLogs.iOS
{
    using Autofac;
    using EnterpriseAddLogs;

    public sealed class IosBootstrapper: BootStrapper
    {
        protected override void RegisterModules(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<IosModule>();
        }
    }
}