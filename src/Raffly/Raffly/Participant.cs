using System.Collections.Generic;

namespace Raffly
{
    class Participant
    {
        public string Moniker { get; set; }
        public Contact Contact { get; set; }
        public Dictionary<string, ParticipantPreference> Preferences { get; set; }

        public override string ToString() => $"{Moniker} ({Contact})";
    }
}