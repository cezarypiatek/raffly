using System;
using System.Collections.Generic;

namespace Raffly
{
    class FairRandomWinnerPolicy : IWinnerPolicy
    {
        private readonly Random _random;

        public FairRandomWinnerPolicy()
        {
            this._random = new Random();
        }

        public Participant DrawTheLuckyOne(IReadOnlyList<Participant> participants)
        {
            var luckyIndex =  _random.Next(0, participants.Count - 1);
            return participants[luckyIndex];
        }
    }
}