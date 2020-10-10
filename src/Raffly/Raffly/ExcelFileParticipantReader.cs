using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace Raffly
{
    class ExcelFileParticipantReader : IParticipantReader
    {
        private readonly string _path;

        public ExcelFileParticipantReader(string path)
        {
            _path = path;
        }

        public IReadOnlyList<Participant> Load()
        {
            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(_path)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var table = worksheet.Tables[0];
                return EnumerateParticipants(table).ToList();
            }

        }

        private static IEnumerable<Participant> EnumerateParticipants(ExcelTable table)
        {
            for (int i = table.Range.Start.Row +1; i <= table.Range.End.Row; i++)
            {
                var moniker = table.WorkSheet.Cells[i, 12].GetValue<string>()?.Trim();
                var email = table.WorkSheet.Cells[i, 13].GetValue<string>()?.Trim();
                var prizes = table.WorkSheet.Cells[i, 14].GetValue<string>()?.Split(";") ?? Array.Empty<string>();
                if (string.IsNullOrWhiteSpace(moniker) == false && string.IsNullOrWhiteSpace(email) == false && prizes.Length > 0)
                {
                    yield return new Participant()
                    {
                        Moniker = moniker,
                        Contact = new EmailAddress(){Email = email},
                        Preferences = new Dictionary<string, ParticipantPreference>()
                        {
                            ["Prize"] = new PricePreference()
                            {
                                AnticipatedPrizes = prizes
                            }
                        }
                    };
                }
            }
        }
    }
}