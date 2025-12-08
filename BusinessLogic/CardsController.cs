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
        private readonly ISentenceGenerator _sentenceGenerator;

        public CardsController(DatabaseContext db, ISentenceGenerator sentenceGenerator)
        {
            _db = db;
            _sentenceGenerator = sentenceGenerator;
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Card>> GetById(Guid id)
        {
            Card? card = await _db.Cards.FindAsync(id);
            if (card == null) return NotFound();
            return Ok(card);
        }


        [HttpPost()]
        public async Task<ActionResult<Card>> Post(Card newCard)
        {
            _db.Cards.Add(newCard);
            await _db.SaveChangesAsync();

            return Created($"/cards/{newCard.Id}", newCard);
        }


        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Card>> Put(Card updatedCard)
        {
            Card? cardToUpdate = await _db.Cards.FindAsync(updatedCard.Id);
            if (cardToUpdate == null) return NotFound();

            cardToUpdate.Title = updatedCard.Title;
            cardToUpdate.Description = updatedCard.Description;
            cardToUpdate.nextRepeatition = updatedCard.nextRepeatition;

            await _db.SaveChangesAsync();

            return Ok(cardToUpdate);
        }


        [HttpGet("{id:guid}/generate-sentense")]
        public async Task<ActionResult<Dictionary<string, string>>> GenerateSentense(Card card)
        {
            Dictionary<string, string> sentence = await _sentenceGenerator.GenerateSentence(card);
            return Ok(sentence);
        }
    }
}
