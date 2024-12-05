using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MostIdea.MIMGroup.DataExporting.Excel.NPOI;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.B2B.Exporting
{
    public class BrandsExcelExporter : NpoiExcelExporterBase, IBrandsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BrandsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBrandForViewDto> brands)
        {
            return null;
            /*
            return CreateExcelPackage(
                "Brands.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Brands"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description")
                        );

                    AddObjects(
                        sheet, 2, brands,
                        _ => _.Brand.Name,
                        _ => _.Brand.Description
                        );

                });*/
        }
    }
}