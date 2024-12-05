using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MostIdea.MIMGroup.DataExporting.Excel.NPOI;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.B2B.Exporting
{
    public class SalesConsultantsExcelExporter : NpoiExcelExporterBase, ISalesConsultantsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SalesConsultantsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSalesConsultantForViewDto> salesConsultants)
        {
            return CreateExcelPackage(
                "SalesConsultants.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("SalesConsultants"));

                    AddHeader(
                        sheet,
                        (L("User")) + L("Name"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, salesConsultants,
                        _ => _.UserName,
                        _ => _.UserName2
                        );

                });
        }
    }
}