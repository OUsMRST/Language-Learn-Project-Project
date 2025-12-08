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
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _db;

        public UsersController(DatabaseContext db)
        {
            _db = db;
        }


        
        [HttpGet()]
        public async Task<IEnumerable<User>> Get()
        {
            List<User> users = await _db.Users.ToListAsync();
            return users;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<User>> GetById(Guid id)
        {
            User? user = await _db.Users.FindAsync(id);

            if (user == null) return NotFound();
            return Ok(user);
        }


        [HttpGet("{id:guid}/decks")]
        public async Task<ActionResult<ICollection<Deck>>> GetDecks(Guid id)
        {
            User? user = await _db.Users.Include(u => u.Decks).ThenInclude(d => d.Cards).FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();
            return Ok(user.Decks);
        }


        [HttpPost()]
        public async Task<ActionResult<User>> Post()
        {
            User newUser = new User();
            _db.Add(newUser);
            await _db.SaveChangesAsync();

            return Created($"/users/{newUser.Id}", newUser);
        }
    }
}
