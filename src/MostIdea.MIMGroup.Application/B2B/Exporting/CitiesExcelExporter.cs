using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MostIdea.MIMGroup.DataExporting.Excel.NPOI;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.B2B.Exporting
{
    public class CitiesExcelExporter : NpoiExcelExporterBase, ICitiesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CitiesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCityForViewDto> cities)
        {
            return null;
            /* 
              return CreateExcelPackage(
                  "Cities.xlsx",
                  excelPackage =>
                  {

                      var sheet = excelPackage.CreateSheet(L("Cities"));

                      AddHeader(
                          sheet,
                          L("Name"),
                          (L("Country")) + L("Name")
                          );

                      AddObjects(
                          sheet, 2, cities,
                          _ => _.City.Name,
                          _ => _.CountryName
                          );

                  });*/
        }
    }
}