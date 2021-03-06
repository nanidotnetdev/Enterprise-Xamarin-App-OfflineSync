﻿namespace EnterpriseAddLogs.Helpers
{
    using EnterpriseAddLogs.ViewModels;
    using System;
    using Xamarin.Forms;

    public sealed class ViewResolver: IViewResolver
    {
        public Page ResolveView<TViewModel>()
            where TViewModel : PageViewModel
        {
            var viewModelType = typeof(TViewModel);

            var viewName = viewModelType.AssemblyQualifiedName.Replace(
                viewModelType.Name,
                viewModelType.Name.Replace("ViewModel", string.Empty));


            var nn = viewName.Replace("Model", string.Empty);
            var tt = Type.GetType(nn);

            return Ioc.Resolve(Type.GetType(viewName.Replace("Model", string.Empty))) as Page;
        }
    }
}
