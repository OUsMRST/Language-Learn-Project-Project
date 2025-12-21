using Core;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheluding
{
    public class Scheduler : IScheduler
    {
        public void Schedule(int assessment, ref Card card)
        {
            if (card.Difficulty == null) card.Difficulty = 1;    
            switch (assessment)
            {
                case 1:
                    card.Lapses++;
                    card.Difficulty = 1;
                    card.NextRepetitionTime.AddSeconds(30);
                    break;
                case 2:
                    card.SuccessfulRepetitions++;
                    card.NextRepetitionTime = DateTimeOffset.UtcNow.AddMinutes(10 / ((double)card.Difficulty + 0.01));
                    break;
                case 3:
                    card.SuccessfulRepetitions++;
                    card.NextRepetitionTime = DateTimeOffset.UtcNow.AddMinutes(100 / ((double)card.Difficulty + 0.01));
                    break;
                case 4:
                    card.SuccessfulRepetitions++;
                    card.NextRepetitionTime = DateTimeOffset.UtcNow.AddMinutes(500 / ((double)card.Difficulty + 0.01));
                    break;
            }
        }
    }
}
