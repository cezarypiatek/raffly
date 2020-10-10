using System;
using System.Linq;

namespace Raffly
{
    class SelectedPrizeAccessPolicy : IPrizeAccessPolicy
    {
        public bool HasAccessTo(Participant participant, Prize prize)
        {
            return participant.Preferences.TryGetValue("Prize", out var preference) &&
                   preference is PricePreference pricePreference && 
                   pricePreference.AnticipatedPrizes.Any(x => string.Equals(x, prize.Name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}