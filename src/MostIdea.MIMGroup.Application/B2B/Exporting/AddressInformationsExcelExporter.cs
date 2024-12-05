using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MostIdea.MIMGroup.DataExporting.Excel.NPOI;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.B2B.Exporting
{
    public class AddressInformationsExcelExporter : NpoiExcelExporterBase, IAddressInformationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AddressInformationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAddressInformationForViewDto> addressInformations)
        {
            return null;
            /*
            return CreateExcelPackage(
                "AddressInformations.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("AddressInformations"));

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, 2, addressInformations,
                        _ => _.AddressInformation.Name
                        );

                });*/
        }
    }
}