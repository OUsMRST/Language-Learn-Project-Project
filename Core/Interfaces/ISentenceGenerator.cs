using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface ISentenceGenerator
    {
        public Task<string> GenerateSentencePair(Card card);
    }
}
