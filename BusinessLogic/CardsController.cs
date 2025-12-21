using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Core;
using DataAccess;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Core.Interfaces;

namespace BusinessLogic
{
    [ApiController]
    [Route("[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly DatabaseContext _db;
        private readonly ICardsQueue _cardsQueue;

        public CardsController(DatabaseContext db, ICardsQueue cardQueue)
        {
            _db = db;
            _cardsQueue = cardQueue;
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Card>> GetById(Guid id)
        {
            Card? card = await _db.Cards.FindAsync(id);
            if (card == null) return NotFound();
            return Ok(card);
        }


        [HttpPost()]
        public async Task<ActionResult<Card>> Post(Card newCard, CancellationToken cancellationToken)
        {
            _db.Cards.Add(newCard);
            await _db.SaveChangesAsync(cancellationToken);
            _cardsQueue.Enqueque(newCard.Id);

            return Created($"/cards/{newCard.Id}", newCard);
        }


        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Card>> Put(Card updatedCard, CancellationToken cancellationToken)
        {
            Card? cardToUpdate = await _db.Cards.FindAsync(updatedCard.Id);
            if (cardToUpdate == null) return NotFound();

            cardToUpdate.Title = updatedCard.Title;
            cardToUpdate.Description = updatedCard.Description;
            cardToUpdate.NextRepetitionTime = updatedCard.NextRepetitionTime;

            await _db.SaveChangesAsync(cancellationToken);

            return Ok(cardToUpdate);
        }
    }
}
