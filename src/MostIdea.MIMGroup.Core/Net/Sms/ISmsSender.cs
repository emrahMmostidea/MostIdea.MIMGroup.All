using System.Threading.Tasks;

namespace MostIdea.MIMGroup.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}