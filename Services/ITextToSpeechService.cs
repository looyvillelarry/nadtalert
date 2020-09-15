using System;
using System.Threading.Tasks;
using NADT.Models;

namespace NADT.Services
{
    public interface ITextToSpeechService
    {
        Task<HttpSpeechResponse> GetSpeech(string body, string from);
    }
}
