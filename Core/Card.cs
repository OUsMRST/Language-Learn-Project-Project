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
        public Guid UserId { get; set; }
        public Guid DeckId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTimeOffset nextRepeatition { get; set; } = DateTimeOffset.MinValue;
    }
}
