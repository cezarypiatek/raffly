using System.Collections.Generic;

namespace Raffly
{
    interface IParticipantReader
    {
        IReadOnlyList<Participant> Load();
    }
}