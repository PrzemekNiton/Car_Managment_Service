using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radzen;
using Radzen.Blazor;
using OpenAI_API;
using OpenAI_API.Completions;
using System.Security.Cryptography;

namespace SimpleCarManagment.Pages
{
    public partial class OpenAiComponent
    {
        public async Task<List<ResponseAPI>> GetOpenAI()
        {
            var carArray = car.Name.Split(" ");

            var userInput = $"Dla samochodu marki {carArray[0]} modelu {carArray[1]} rocznika {car.Production.ToString("yyyy")} o przebiegu {car.Mileage} km, proszę podać terminy wymiany oleju, filtrów oleju, filtrów powietrza, wymiany rozrządu, wymiany płynu hamulcowego, wymiany płynu chłodniczego, regularnej kontroli lub wymiany świateł przednich i tylnych oraz regularnej kontroli lub wymiany klocków i tarcz hamulcowych.";
            var api = new OpenAIAPI(new APIAuthentication("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"));

            var response = await api.Completions.CreateCompletionAsync(new CompletionRequest
            {
                Prompt = userInput,
                MaxTokens = 300
            });

            if (response != null && response.Completions != null && response.Completions.Any())
            {
                var asd = response.Completions[0].Text;
                var answer = asd.Split("\n");

                return answer.Where(x => x.Length > 0).Select(x => new ResponseAPI { Response = x }).ToList();
            }

            return new List<ResponseAPI>();
        }
    }

    public class ResponseAPI
    {
        public string Response { get; set; }
    }
}

