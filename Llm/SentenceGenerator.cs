using Core;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using OllamaSharp;
using OllamaSharp.Models;
using System.Text.Json;

namespace Llm
{
    public class SentenceGenerator : ISentenceGenerator
    {
        private readonly OllamaApiClient _ollamaClient;
        private readonly string _model;
        private readonly JsonElement _format = JsonDocument.Parse("""
            {
                "type": "object",
                "properties": {
                    "Foreign": {"type": "string"},
                    "Familiar": {"type": "string"}
                },
                "required": ["Foreign", "Familiar"],
                "additionalProperties": false
            }
            """).RootElement.Clone();



        public SentenceGenerator(string uri = "http://127.0.0.1:11434", string model = "hf.co/Vikhrmodels/Vistral-24B-Instruct-GGUF:Q4_K_M")
        {
            _ollamaClient = new OllamaApiClient(uri, model);
            _model = model;
        }



        public async Task<string> GenerateSentencePair(Card card)
        {
            GenerateRequest request = new GenerateRequest()
            {
                Model = _model,
                Stream = false,
                Format = _format,

                Prompt = card.Description == null ? $"""
                Generate sentence using {card.Title} in {card.LearningLanguage} (it is foreign language). Then translate it to {card.FamiliarLanguage} (it is familiar language). Return ONLY a valid JSON object with exactly the following fields: Foreign, Familiar. Any other text or fields are forbidden.
                The response must start with '{'{'}' and end with '{'}'}'.
                """ 
                : $"""
                Generate sentence using {card.Title} in {card.LearningLanguage} with the meaning {card.Description} (it is foreign language). Then translate it to {card.FamiliarLanguage} (it is familiar language). Return ONLY a valid JSON object with exactly the following fields: Foreign, Familiar. Any other text or fields are forbidden.
                The response must start with '{'{'}' and end with '{'}'}'.
                """
            };


            IAsyncEnumerable<GenerateResponseStream?> response = _ollamaClient.GenerateAsync(request);
            StringBuilder futureOutput = new StringBuilder();

            await foreach (GenerateResponseStream? token in response) futureOutput.Append(token!.Response);
            return futureOutput.ToString();
        }
    }
}
