using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MostIdea.MIMGroup.DataExporting.Excel.NPOI;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using MostIdea.MIMGroup.Storage;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace MostIdea.MIMGroup.B2B.Exporting
{
    public class HospitalVsUsersExcelExporter : NpoiExcelExporterBase, IHospitalVsUsersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HospitalVsUsersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHospitalVsUserForViewDto> hospitalVsUsers)
        {
            return CreateExcelPackage(
                "HospitalVsUsers.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("HospitalVsUsers"));

                    AddHeader(
                        sheet,
                        (L("Hospital")) + L("Name"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(sheet,hospitalVsUsers,_ => _.HospitalName);

                    //AddObjects(
                    //    sheet, 2, hospitalVsUsers,
                    //    _ => _.HospitalName,
                    //    _ => _.UserName
                    //    );

                });
        }
    }
}