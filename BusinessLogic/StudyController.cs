using Core;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace BusinessLogic
{
    [ApiController]
    [Route("[controller]")]
    public class StudyController : ControllerBase
    {
        private readonly ICardsQueue _cardsQueue;
        private readonly IDatabaseContext _db;
        private readonly IScheduler _scheduler;

        public StudyController(ICardsQueue queue, IDatabaseContext db, IScheduler scheduler)
        { 
            _cardsQueue = queue;
            _db = db;
            _scheduler = scheduler;
        }
        


        [HttpGet("{deckId:guid}")]
        public async Task<ActionResult<Card>> GetRandomCardToRepeatFromDeck(Guid deckId)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            double randomNumberToCompare = Random.Shared.NextDouble();

            Card? cardToRepeat = await _db.Cards.FirstOrDefaultAsync(c => c.DeckId == deckId && c.NextRepetitionTime >= now && c.KeyForRandomChoice >= randomNumberToCompare);
            
            if (cardToRepeat == null)
            {
                cardToRepeat = await _db.Cards.FirstOrDefaultAsync(c => c.DeckId == deckId && c.NextRepetitionTime >= now);
                
                if (cardToRepeat == null) return NotFound();
            }
            return Ok(cardToRepeat); // Некрасиво и костыльно :(
        }


        [HttpPatch("{cardId:guid}/{assessment:int}")]
        public async Task<IActionResult> RepeatCard(int assessment, Guid cardId, CancellationToken cancellationToken)
        {
            Card? repeatedCard = await _db.Cards.FindAsync(cardId);
            if (repeatedCard == null) return NotFound();

            repeatedCard = _scheduler.Schedule(assessment, repeatedCard);
            await _db.SaveChangesAsync(cancellationToken);
            _cardsQueue.EnquequeAsync(cardId);

            return Ok(repeatedCard);
        }
    }
}