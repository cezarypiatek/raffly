using System.Collections.Generic;

namespace Raffly
{
    class PricePreference: ParticipantPreference
    {
        public IReadOnlyList<string> AnticipatedPrizes { get; set; }
    }
}