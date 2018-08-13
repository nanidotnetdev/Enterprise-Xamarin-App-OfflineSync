namespace EnterpriseAddLogs
{
    using Autofac;
    using EnterpriseAddLogs.Services;

    public class BootStrapper
    {
        public IContainer Bootstrap()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<UiModule>();
            containerBuilder.RegisterModule<ServicesModule>();

            RegisterModules(containerBuilder);
            return containerBuilder.Build();
        }

        protected virtual void RegisterModules(ContainerBuilder containerBuilder)
        {
        }
    }
}
