namespace EnterpriseAddLogs.Services
{
    using Autofac;

    public sealed class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ServicesModule).Assembly).AsImplementedInterfaces();
        }
    }
}
