using MostIdea.MIMGroup.Models.Users;
using MostIdea.MIMGroup.ViewModels;
using Xamarin.Forms;

namespace MostIdea.MIMGroup.Views
{
    public partial class UsersView : ContentPage, IXamarinView
    {
        public UsersView()
        {
            InitializeComponent();
        }

        public async void ListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            await ((UsersViewModel) BindingContext).LoadMoreUserIfNeedsAsync(e.Item as UserListModel);
        }
    }
}