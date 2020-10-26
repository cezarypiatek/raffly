using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace Raffly
{
    public class ExcelFileParticipantReaderSettings
    {
        public string Source { get; set; }
        public Dictionary<string,int> FieldMapping { get; set; }
    }

    class ExcelFileParticipantReader : IParticipantReader
    {
        private readonly ExcelFileParticipantReaderSettings _settings;

        public ExcelFileParticipantReader(ExcelFileParticipantReaderSettings settings)
        {
            _settings = settings;
        }

        public IReadOnlyList<Participant> Load()
        {
            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(_settings.Source)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var table = worksheet.Tables[0];
                return EnumerateParticipants(table).ToList();
            }

        }

        static class FieldNames
        {
            public const string Moniker = "Moniker";
            public const string Email = "Email";
            public const string Prizes = "Prizes";
        }

        private IEnumerable<Participant> EnumerateParticipants(ExcelTable table)
        {
            for (int i = table.Range.Start.Row +1; i <= table.Range.End.Row; i++)
            {
                var moniker = table.WorkSheet.Cells[i, _settings.FieldMapping[FieldNames.Moniker]].GetValue<string>()?.Trim();
                var email = table.WorkSheet.Cells[i, _settings.FieldMapping[FieldNames.Email]].GetValue<string>()?.Trim();
                var prizes = table.WorkSheet.Cells[i, _settings.FieldMapping[FieldNames.Prizes]].GetValue<string>()?.Split(";") ?? Array.Empty<string>();
                if (string.IsNullOrWhiteSpace(moniker) == false && string.IsNullOrWhiteSpace(email) == false && prizes.Length > 0)
                {
                    yield return new Participant
                    {
                        Moniker = moniker,
                        Contact = new EmailAddress { Email = email },
                        Preferences = new Dictionary<string, ParticipantPreference>
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