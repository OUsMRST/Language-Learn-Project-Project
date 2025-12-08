using Core;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Llm
{
    public class SentenceGenerator : ISentenceGenerator
    {
        public Task<Dictionary<string, string>> GenerateSentence(Card card)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["foreign"] = "This is first *word* you'll learn!";
            output["familiar"] = "Это первое *слово*, которое ты выучишь!";

            return Task.FromResult(output);
        }
    }
}
