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
            Console.WriteLine($"Worker started at {DateTimeOffset.UtcNow}. Cancellation requested: {cancellationToken.IsCancellationRequested}");

            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Waiting for card ID...");
                Guid cardId = await _cardQueue.DequequeAsync(cancellationToken); // О... Так оказывается компилятор оптимизирует и автоматически выносит переменные за пределы цикла... Интересненько :)
                using IServiceScope scope = _services.CreateScope();
                IDatabaseContext database = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();

                try
                {
                    Card? card = await database.Cards.FindAsync(cardId);
                    Console.WriteLine("Card dequeqed.");

                    if (card == null)
                    {
                        Console.WriteLine("Card == null!");
                        continue;
                    }


                    string sentencePair = await _sentenceGenerator.GenerateSentencePair(card);
                    card.GeneratedSentencePair = sentencePair;

                    await database.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                }
            }
        }
    }
}
