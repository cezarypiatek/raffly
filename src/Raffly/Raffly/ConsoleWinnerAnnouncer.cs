using System;
using System.Collections.Generic;
using System.Linq;

namespace Raffly
{
    class ConsoleWinnerAnnouncer : IWinnerAnnouncer
    {
        public void Announce(IReadOnlyList<Winner> winners)
        {
            Console.WriteLine("Winners:");
            foreach (var winnersGroup in winners.GroupBy(x=>x.Prize.Name))
            {
                Console.WriteLine($"{winnersGroup.Key}: {string.Join(", ", winnersGroup.Select(x=>x.Participant.Moniker))}");
            }
        }
    }
}