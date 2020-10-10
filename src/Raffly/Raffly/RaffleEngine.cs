using System.Collections.Generic;
using System.Linq;

namespace Raffly
{
    class RaffleEngine
    {
        private readonly IPrizeAccessPolicy _policy;
        private readonly IWinnerPolicy _winnerPolicy;

        public RaffleEngine(IPrizeAccessPolicy policy, IWinnerPolicy winnerPolicy)
        {
            _policy = policy;
            _winnerPolicy = winnerPolicy;
        }

        public IReadOnlyList<Winner> Draw(IReadOnlyList<Prize> prizes, IReadOnlyList<Participant> participants)
        {
            var nextTourParticipants = participants.ToHashSet();
            return EnumerateWinners(prizes, nextTourParticipants).ToList();
        }

        private IEnumerable<Winner> EnumerateWinners(IReadOnlyList<Prize> prizes, HashSet<Participant> participants)
        {
            foreach (var prize in prizes)
            {
                var thisPrizeParticipants = participants.Where(p => _policy.HasAccessTo(p, prize)).ToList();
                if (thisPrizeParticipants.Count == 0)
                {
                    continue;
                }
                var luckyOne = _winnerPolicy.DrawTheLuckyOne(thisPrizeParticipants);
                participants.Remove(luckyOne);
                yield return new Winner
                {
                    Participant = luckyOne,
                    Prize = prize
                };
            }
        }
    }
}