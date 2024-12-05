using Abp.Application.Services.Dto;

namespace MostIdea.MIMGroup.Authorization.Users.Dto
{
    public interface IGetLoginAttemptsInput: ISortedResultRequest
    {
        string Filter { get; set; }
    }
}