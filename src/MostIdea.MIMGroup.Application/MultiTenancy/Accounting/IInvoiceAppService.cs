using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.MultiTenancy.Accounting.Dto;

namespace MostIdea.MIMGroup.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
