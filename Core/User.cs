using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public ICollection<Deck> Decks { get; set; } = new List<Deck>();
        public ICollection<Card> Cards { get; set; } = new List<Card>();

        public string FamiliarLanguage { get; set; } = "Russian";
        public string LearningLanguage { get; set; } = "English";
    }
}
