using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Net.Http;
using NADT.Models;
using NADT.Services;
using Twilio.Types;
using Twilio.AspNet.Core;
using Twilio.TwiML;

namespace Nadt.Alert.Functions
{
    public static class AlertAll
    {
        private static string twilioAccountSid = Environment.GetEnvironmentVariable("TwilioAccountSid");
        private static string twilioAccountKey = Environment.GetEnvironmentVariable("TwilioAccountKey");
        private static string twilioAuthServiceKey = Environment.GetEnvironmentVariable("TwilioAuthenticationServiceKey");        
        private static string twilioFromNumber = Environment.GetEnvironmentVariable("TwilioFromNumber");
        private static string storageAccount = Environment.GetEnvironmentVariable("StorageAccount");
        private static string storageAccountKey = Environment.GetEnvironmentVariable("StorageAccountKey");
        

        [FunctionName("AlertAll")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
                //log.LogInformation("Alert All function called");
            string messageText = req.Query["message"];
            if(string.IsNullOrWhiteSpace(messageText))
            {
                messageText =  "PRN ALERT : CHECK ZELLO !";
            }
            
    
            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);

            

            var storageCreds = new StorageCreds{Account =storageAccount, Key=storageAccountKey};
            var blobService = new BlobService(storageCreds);
            var twilioAccount = new TwilioAccount{AccountSid = twilioAccountSid, AuthToken =twilioAccountKey};
            //var authenticationService = new AuthenticationService(twilioAuthServiceKey);
            //var textToSpeechService = new TextToSpeechService(authenticationService,storageCreds);
            TwilioClient.Init(twilioAccount.AccountSid,twilioAccount.AuthToken);

            int counter=1; 
            /* TwilioResponse twilioResponse = new TwilioResponse();
            await CallResource.CreateAsync( 
                to: new PhoneNumber  ("+15027516471"),
                from: new PhoneNumber(twilioFrom),
                 url: new Uri($"{siteUrl}/api/speech/call/{twilioResponse.MessageSid}"),
                method: "GET");
            )
            await this.Call()
            */
            foreach( var person in await blobService.GetPhoneNumbersAsync())
            {
            var message = MessageResource.Create(
                        body: messageText,
                        from: new Twilio.Types.PhoneNumber(twilioFromNumber),
                        to: new Twilio.Types.PhoneNumber(person)
                    );
                    counter++;
            }
            
            string responseMessage =  $"A message of {messageText} was sent to {counter-1} people.";
            
            return new OkObjectResult(responseMessage);
        }
/*
        public static async Task<TwiMLResult> Call(string messageSid, ITextToSpeechService speechService)
        {
            var message = await MessageResource.FetchAsync(pathSid: messageSid);
            var response = await speechService
                        .GetSpeech(message.Body, message.From.ToString());
            var twiml = new VoiceResponse();
            twiml.Play(new Uri(response.Path));
            var result = new TwiMLResult(twiml);
            return result; //TwiML(twiml);
        }   
*/
    }

}
