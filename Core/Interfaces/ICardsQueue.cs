using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace Core.Interfaces
{
    public interface ICardsQueue
    {
        public void Enqueque(Guid cardId);
        public ValueTask<Guid> Dequeque(CancellationToken cancellationToken);
    }
}
