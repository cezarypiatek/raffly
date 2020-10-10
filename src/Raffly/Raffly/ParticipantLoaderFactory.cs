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
            var data = configurationRoot.GetSection("ParticipantsData").Get<Dictionary<string,string>>();
            data.TryGetValue("Type", out var sourceType);
            return sourceType switch
            {
                "ExcelFileParticipantReader" => CreateExcelFileParticipantReader(data["Source"]),
                _ => throw new ArgumentOutOfRangeException(nameof(sourceType), $"Unknown participant loader: {sourceType}")
            };
        }

        private static ExcelFileParticipantReader CreateExcelFileParticipantReader(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new InvalidOperationException("File with participant data not defined");
            }

            if (File.Exists(path) == false)
            {
                throw new FileNotFoundException("File with participants data doesn't exist", path);
            }

            return new ExcelFileParticipantReader(path);
        }
    }
}