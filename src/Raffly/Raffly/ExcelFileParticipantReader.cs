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
        public string ImposePrizes { get; set; }
        public Dictionary<string,string> FieldMapping { get; set; }
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
                var moniker = TryGetMonikerForRow(table, i);
                var email = TryGetEmailForRow(table, i);
                var prizes = TryGetPrizesForRow(table, i);
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

        private string[] TryGetPrizesForRow(ExcelTable table, int i)
        {
            var prizesString = string.IsNullOrWhiteSpace(_settings.ImposePrizes) ?
                table.WorkSheet.Cells[$"{_settings.FieldMapping[FieldNames.Prizes]}{i}"].GetValue<string>()
                : _settings.ImposePrizes;
            return prizesString?.Split(";") ?? Array.Empty<string>();
        }

        private string TryGetEmailForRow(ExcelTable table, int i)
        {
            return table.WorkSheet.Cells[$"{_settings.FieldMapping[FieldNames.Email]}{i}"].GetValue<string>()?.Trim();
        }

        private string TryGetMonikerForRow(ExcelTable table, int i)
        {

            var address = $"{_settings.FieldMapping[FieldNames.Moniker]}{i}";
            return table.WorkSheet.Cells[address].GetValue<string>()?.Trim();
        }
    }
}