using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MostIdea.MIMGroup.DataExporting.Excel.NPOI;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.B2B.Exporting
{
    public class HospitalGroupsExcelExporter : NpoiExcelExporterBase, IHospitalGroupsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HospitalGroupsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHospitalGroupForViewDto> hospitalGroups)
        {
            return null;
            /*
            return CreateExcelPackage(
                "HospitalGroups.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("HospitalGroups"));

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, 2, hospitalGroups,
                        _ => _.HospitalGroup.Name
                        );

                }); */
        }
    }
}