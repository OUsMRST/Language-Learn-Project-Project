using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Core;
using DataAccess;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BusinessLogic
{
    [ApiController]
    [Route("[controller]")]
    public class DecksController : ControllerBase
    {
        private readonly DatabaseContext _db;


        public DecksController(DatabaseContext db)
        {
            _db = db;
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Deck>> GetById(Guid id)
        {
            Deck? deck = await _db.Decks.FindAsync(id);

            if (deck == null) return NotFound();
            return Ok(deck);
        }


        [HttpGet("{id:guid}/cards")]
        public async Task<ActionResult<ICollection<Card>>> GetCards(Guid id)
        {
            Deck? deck = await _db.Decks.Include(d => d.Cards).FirstOrDefaultAsync(x => x.Id == id);
            
            if (deck == null) return NotFound();
            return Ok(deck.Cards);
        }


        [HttpPost()]
        public async Task<ActionResult<Deck>> Post(Deck newDeck)
        {
            _db.Decks.Add(newDeck);
            await _db.SaveChangesAsync();

            return Created($"/decks/{newDeck.Id}", newDeck);
        }


        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Deck>> Put(Deck updatedDeck)
        {
            Deck? deckToUpdate = await _db.Decks.FindAsync(updatedDeck.Id);
            if (deckToUpdate == null) return NotFound();

            deckToUpdate.Name = updatedDeck.Name;
            deckToUpdate.Description = updatedDeck.Description;

            await _db.SaveChangesAsync();

            return Ok(deckToUpdate);
        }
    }
}
