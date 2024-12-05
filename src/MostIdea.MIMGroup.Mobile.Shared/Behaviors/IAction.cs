using Xamarin.Forms.Internals;

namespace MostIdea.MIMGroup.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}