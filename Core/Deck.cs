using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core
{
    public class Deck
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }
        
        public ICollection<Card> Cards { get; set; } = new List<Card>();

        public string FamiliarLanguage { get; set; } = "Russian";
        public string LearningLanguage { get; set; } = "English";
    }
}
