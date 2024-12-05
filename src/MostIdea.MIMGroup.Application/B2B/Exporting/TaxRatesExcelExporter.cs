using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MostIdea.MIMGroup.DataExporting.Excel.NPOI;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.B2B.Exporting
{
    public class TaxRatesExcelExporter : NpoiExcelExporterBase, ITaxRatesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TaxRatesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTaxRateForViewDto> taxRates)
        {
            return null;

            //return CreateExcelPackage(
            //    "TaxRates.xlsx",
            //    excelPackage =>
            //    {

            //        var sheet = excelPackage.CreateSheet(L("TaxRates"));

            //        AddHeader(
            //            sheet,
            //            L("Name"),
            //            L("Rate")
            //            );

            //        AddObjects(
            //            sheet, 2, taxRates,
            //            _ => _.TaxRate.Name,
            //            _ => _.TaxRate.Rate
            //            );

            //    });
        }
    }
}