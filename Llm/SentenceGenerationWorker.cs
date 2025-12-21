using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Core;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Llm
{
    public class SentenceGenerationWorker : BackgroundService
    {
        private readonly ICardsQueue _cardQueue;
        private readonly IServiceProvider _services;
        private readonly ISentenceGenerator _sentenceGenerator;

        public SentenceGenerationWorker(ICardsQueue cardQueue, IServiceProvider services, ISentenceGenerator sentenceGenerator)
        {
            _cardQueue = cardQueue;
            _services = services;
            _sentenceGenerator = sentenceGenerator;
        }


        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Guid cardId = await _cardQueue.Dequeque(cancellationToken); // О... Так оказывается компилятор оптимизирует и автоматически выносит переменные за пределы цикла... Интересненько :)

                using IServiceScope scope = _services.CreateScope();

                IDatabaseContext database = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();

                Card? card = await database.Cards.FindAsync(cardId, cancellationToken);
                if (card == null) continue;

                string sentencePair = await _sentenceGenerator.GenerateSentencePair(card);
                card.GeneratedSentencePair = sentencePair;

                await database.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
