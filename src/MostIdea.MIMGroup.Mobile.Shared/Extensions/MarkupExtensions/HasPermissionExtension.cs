using System;
using MostIdea.MIMGroup.Core;
using MostIdea.MIMGroup.Core.Dependency;
using MostIdea.MIMGroup.Services.Permission;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MostIdea.MIMGroup.Extensions.MarkupExtensions
{
    [ContentProperty("Text")]
    public class HasPermissionExtension : IMarkupExtension
    {
        public string Text { get; set; }
        
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ApplicationBootstrapper.AbpBootstrapper == null || Text == null)
            {
                return false;
            }

            var permissionService = DependencyResolver.Resolve<IPermissionService>();
            return permissionService.HasPermission(Text);
        }
    }
}