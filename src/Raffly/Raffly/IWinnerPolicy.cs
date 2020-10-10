using System.Collections.Generic;

namespace Raffly
{
    interface IWinnerPolicy
    {
        Participant DrawTheLuckyOne(IReadOnlyList<Participant> participants);
    }
}