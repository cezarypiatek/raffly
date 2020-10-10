using System.Collections.Generic;

namespace Raffly
{
    class PrizeFactory
    {
        public static IEnumerable<Prize> Create(uint quantity, string name)
        {
            for (int i = 0; i < quantity; i++)
            {
                yield return new Prize(name);
            }
        }
    }
}