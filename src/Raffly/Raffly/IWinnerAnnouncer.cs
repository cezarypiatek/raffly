using System.Collections.Generic;

namespace Raffly
{
    interface IWinnerAnnouncer
    {
        void Announce(IReadOnlyList<Winner> winners);
    }
}