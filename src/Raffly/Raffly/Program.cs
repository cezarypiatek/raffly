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

            ShowIntro(prizes, participants);

            var announcer = CreateAnnouncer();
            var raffleEngine = CreateRaffleEngine();
            Console.WriteLine("Press key to start the raffle....");
            Console.ReadKey();
            var winners = raffleEngine.Draw(prizes, participants);
            announcer.Announce(winners);
            Console.WriteLine("Congratulations!");
        }

        private static void ShowIntro(IReadOnlyList<Prize> prizes, IReadOnlyList<Participant> participants)
        {
            Console.WriteLine("Prizes:");
            foreach (var prize in prizes.GroupBy(x=>x.Name))
            {
                Console.WriteLine($"\t{prize.Key} x{prize.Count()}");
            }

            Console.WriteLine("----------------------------------");

            Console.WriteLine("Participants:");
            foreach (var (participant, index) in participants.Select((p,i)=>(p,i+1)))
            {
                Console.WriteLine($"\t#{index} {participant}");
            }
            Console.WriteLine("----------------------------------");
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
