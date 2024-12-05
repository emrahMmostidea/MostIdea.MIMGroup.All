using System.ComponentModel;
using Abp.AutoMapper;
using MostIdea.MIMGroup.Authorization.Users.Dto;

namespace MostIdea.MIMGroup.Models.Users
{
    [AutoMapFrom(typeof(CreateOrUpdateUserInput))]
    public class UserCreateOrUpdateModel : CreateOrUpdateUserInput, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}