using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MostIdea.MIMGroup.DataExporting.Excel.NPOI;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.B2B.Exporting
{
    public class HospitalsExcelExporter : NpoiExcelExporterBase, IHospitalsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HospitalsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHospitalForViewDto> hospitals)
        {
            return null;
            /*
            return CreateExcelPackage(
                "Hospitals.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Hospitals"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        (L("HospitalGroup")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, hospitals,
                        _ => _.Hospital.Name,
                        _ => _.HospitalGroupName
                        );

                });*/
        }
    }
}