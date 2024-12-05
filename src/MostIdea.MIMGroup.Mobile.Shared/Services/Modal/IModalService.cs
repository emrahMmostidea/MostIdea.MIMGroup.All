using System.Threading.Tasks;
using MostIdea.MIMGroup.Views;
using Xamarin.Forms;

namespace MostIdea.MIMGroup.Services.Modal
{
    public interface IModalService
    {
        Task ShowModalAsync(Page page);

        Task ShowModalAsync<TView>(object navigationParameter) where TView : IXamarinView;

        Task<Page> CloseModalAsync();
    }
}
