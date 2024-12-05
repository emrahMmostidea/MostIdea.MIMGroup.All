using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MostIdea.MIMGroup.DataExporting.Excel.NPOI;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.B2B.Exporting
{
    public class DistrictsExcelExporter : NpoiExcelExporterBase, IDistrictsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DistrictsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDistrictForViewDto> districts)
        {
            return null;
            /* 
              return CreateExcelPackage(
                  "Districts.xlsx",
                  excelPackage =>
                  {

                      var sheet = excelPackage.CreateSheet(L("Districts"));

                      AddHeader(
                          sheet,
                          L("Name"),
                          (L("City")) + L("Name")
                          );

                      AddObjects(
                          sheet, 2, districts,
                          _ => _.District.Name,
                          _ => _.CityName
                          );

                  });*/
        }
    }
}