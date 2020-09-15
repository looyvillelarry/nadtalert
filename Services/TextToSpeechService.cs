using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NADT.Models;

namespace NADT.Services
{
    public class TextToSpeechService : ITextToSpeechService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly StorageCreds _storageCreds;

        //I am using Constructor Injection for the services we need.  This was set up in the Startup.cs file
        public TextToSpeechService(IAuthenticationService authenticationService, StorageCreds storageCreds)
        {
            _authenticationService =
                authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));

            _storageCreds =
                storageCreds ?? throw new ArgumentNullException(nameof(storageCreds));

        }

        public async Task<HttpSpeechResponse> GetSpeech(string body, string from)
        {
        var response = new HttpSpeechResponse();
        //below is the endpoint I was given when I added Speech Services, you can substitute it 
        //for the one for your region: 
        //https://eastasia.tts.speech.microsoft.com/cognitiveservices/v1 
        //https://northeurope.tts.speech.microsoft.com/cognitiveservices/v1
        var endpoint = "https://westus.tts.speech.microsoft.com/cognitiveservices/v1";
        var token = await _authenticationService.FetchTokenAsync();
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("X-Microsoft-OutputFormat", "audio-16khz-128kbitrate-mono-mp3");
            client.DefaultRequestHeaders.Add("User-Agent", "autotexter");

            client.DefaultRequestHeaders.Add("Authorization", token);

            var uriBuilder = new UriBuilder(endpoint);

            var text = $@"
                    <speak version='1.0' xmlns=""http://www.w3.org/2001/10/synthesis"" xml:lang='en-US'>
                        <voice  name='Microsoft Server Speech Text to Speech Voice (en-GB, Susan, Apollo)'>
                        You had a text message from {from}
                            <break time = ""100ms"" /> The message was
                            <break time=""100ms""/> {body}
                        </voice> 
                    </speak>
            ";

            var content = new StringContent(text, Encoding.UTF8, "application/ssml+xml");

            var result = await client
                            .PostAsync(uriBuilder.Uri.AbsoluteUri, content)
                            .ConfigureAwait(false);

            response.Code = result.StatusCode;
            if (result.IsSuccessStatusCode)
            {
                // add code to save the soundbite here
            }
            return response;
        }
        }

    }
}
