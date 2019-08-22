namespace EnterpriseAddLogs.Droid
{
    using Autofac;

    public sealed class AndroidBootstrapper : BootStrapper
    {
        protected override void RegisterModules(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<AndroidModule>();
            containerBuilder.RegisterType<AuthenticationProvider>().As<IAuthenticate>().SingleInstance();
        }
    }
}