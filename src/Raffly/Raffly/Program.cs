using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Raffly
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = LoadConfig();
            var participantLoader = ParticipantLoaderFactory.CreateParticipantLoader(configuration);
            var prizes = LoadPrizes(configuration);
            var participants = participantLoader.Load();
            var announcer = CreateAnnouncer();
            var raffleEngine = CreateRaffleEngine();
            Console.WriteLine("Press key to start the raffle....");
            Console.ReadKey();
            var winners = raffleEngine.Draw(prizes, participants);
            announcer.Announce(winners);
            Console.WriteLine("Congratulations!");
        }

        private static RaffleEngine CreateRaffleEngine()
        {
            return new RaffleEngine(new SelectedPrizeAccessPolicy(), new FairRandomWinnerPolicy());
        }

        private static IReadOnlyList<Prize> LoadPrizes(IConfiguration configurationRoot)
        {
            return configurationRoot.GetSection("Prizes").Get<List<Program.PrizeAvailabilityInfo>>()
                .SelectMany(x => PrizeFactory.Create(x.Quantity, x.Name)).ToList();
        }

        class PrizeAvailabilityInfo
        {
            public string Name { get; set; }
            public uint Quantity { get; set; }
        }

        private static ComposeWinnerAnnouncer CreateAnnouncer()
        {
            return new ComposeWinnerAnnouncer(new ConsoleWinnerAnnouncer(), new FileWinnerAnnouncer($"Winners_{DateTime.Now:yyyy-MM-dd}.txt"));
        }

        private static IConfigurationRoot LoadConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
        }
    }
}
