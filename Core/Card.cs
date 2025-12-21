using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core
{
    public class Card
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public required Guid DeckId { get; set; }
        public required Guid UserId { get; set; }

        public required string Title { get; set; }
        public string? Description { get; set; }

        public string? GeneratedSentencePair { get; set; } // JSON format
        
        public DateTimeOffset NextRepetitionTime { get; set; } = DateTimeOffset.MinValue;

        public double? Stability { get; set; }
        public double? Difficulty { get; set; }
        public int SuccessfulRepetitions { get; set; } = 0;
        public int Lapses { get; set; } = 0;

        public string FamiliarLanguage { get; set; } = "Russian";
        public string LearningLanguage { get; set; } = "English";

        public double KeyForRandomChoice { get; set; } = Random.Shared.NextDouble();
    }
}
