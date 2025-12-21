using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using Core;
using Core.Interfaces;

namespace Llm
{
    public class CardQueue : ICardsQueue
    {
        private readonly Channel<Guid> _channel = Channel.CreateUnbounded<Guid>();

        private readonly ConcurrentDictionary<Guid, byte> _forDeduplication = new ConcurrentDictionary<Guid, byte>();
        public void Enqueque(Guid cardId)
        {
            if (!_forDeduplication.TryAdd(cardId, 0))
            _channel.Writer.WriteAsync(cardId);
        }
        public async ValueTask<Guid> Dequeque(CancellationToken cancellationToken)
        {
            Guid cardId = await _channel.Reader.ReadAsync(cancellationToken);
            _forDeduplication.TryRemove(cardId, out _);
            return cardId;
        }
    }
}
