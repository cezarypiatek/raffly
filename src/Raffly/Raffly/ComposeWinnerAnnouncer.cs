using System.Collections.Generic;

namespace Raffly
{
    class ComposeWinnerAnnouncer : IWinnerAnnouncer
    {
        private readonly IWinnerAnnouncer[] _announcers;

        public ComposeWinnerAnnouncer(params IWinnerAnnouncer[] announcers)
        {
            _announcers = announcers;
        }

        public void Announce(IReadOnlyList<Winner> winners)
        {
            foreach (var announcer in _announcers)
            {
                announcer.Announce(winners);
            }
        }
    }
}