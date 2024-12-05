using MostIdea.MIMGroup.Models.Tenants;
using MostIdea.MIMGroup.ViewModels;
using Xamarin.Forms;

namespace MostIdea.MIMGroup.Views
{
    public partial class TenantsView : ContentPage, IXamarinView
    {
        public TenantsView()
        {
            InitializeComponent();
        }

        private async void ListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            await ((TenantsViewModel)BindingContext).LoadMoreTenantsIfNeedsAsync(e.Item as TenantListModel);
        }
    }
}