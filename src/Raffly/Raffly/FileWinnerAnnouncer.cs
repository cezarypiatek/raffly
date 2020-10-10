using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Raffly
{
    class FileWinnerAnnouncer : IWinnerAnnouncer
    {
        private readonly string _path;

        public FileWinnerAnnouncer(string path)
        {
            _path = path;
        }

        public void Announce(IReadOnlyList<Winner> winners)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Winners:");
            foreach (var winnersGroup in winners.GroupBy(x => x.Prize.Name))
            {
                sb.AppendLine($"{winnersGroup.Key}: {string.Join(", ", winnersGroup.Select(x => $"{x.Participant.Moniker} ({x.Participant.Contact})"))}");
            }
            File.WriteAllText(_path, sb.ToString(), Encoding.UTF8);
        }
    }
}