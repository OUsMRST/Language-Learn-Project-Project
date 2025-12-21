using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace Core.Interfaces
{
    public interface ICardsQueue
    {
        public void EnquequeAsync(Guid cardId);
        public ValueTask<Guid> DequequeAsync(CancellationToken cancellationToken);
    }
}
