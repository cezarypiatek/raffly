using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Raffly
{
    static class ParticipantLoaderFactory
    {
        public static ExcelFileParticipantReader CreateParticipantLoader(IConfiguration configurationRoot)
        {
            var dataSettings = configurationRoot.GetSection("ParticipantsData");
            var sourceType = dataSettings.GetSection("Type").Get<string>();
            return sourceType switch
            {
                "ExcelFileParticipantReader" => CreateExcelFileParticipantReader(dataSettings.GetSection("Settings")),
                _ => throw new ArgumentOutOfRangeException(nameof(sourceType), $"Unknown participant loader: {sourceType}")
            };
        }

        private static ExcelFileParticipantReader CreateExcelFileParticipantReader(IConfigurationSection config)
        {
            var readerSettings = config.Get<ExcelFileParticipantReaderSettings>();

            if (string.IsNullOrWhiteSpace(readerSettings.Source))
            {
                throw new InvalidOperationException("File with participant data not defined");
            }

            if (File.Exists(readerSettings.Source) == false)
            {
                throw new FileNotFoundException("File with participants data doesn't exist", readerSettings.Source);
            }

            return new ExcelFileParticipantReader(readerSettings);
        }
    }
}