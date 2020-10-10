namespace Raffly
{
    internal interface IPrizeAccessPolicy
    {
        bool HasAccessTo(Participant participant, Prize prize);
    }
}