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

namespace Nadt.Alert.Functions
{
    public static class AlertAll
    {
     
  
       
        [FunctionName("AlertAll")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
                log.LogInformation("Alert All function called");
            string messageText = req.Query["message"];
            if(string.IsNullOrWhiteSpace(messageText))
            {
                messageText =  "NADT ALERT : CHECK ZELLO !";
            }
                var twilioFrom = "+15022896151";   
    
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            var storageCreds = new StorageCreds{Account = "cs710033fff871335fa", Key="AqH/kXfWKQ5ZCgAvnf+2x2zjVQoeNwPn0vYgB0A2CDvEiXymWfhppV4x03iyY1efFMT4xRun9SRBq1BsKExIHw==" };
            var blobService = new BlobService(storageCreds.ConnectionString);
            var twilioAccount = new TwilioAccount{AccountSid = "AC3c6e72c47e6c954f3eaa6afc18ab4627", AuthToken ="4110e02d7828ff6fe71f5355564895fa"};
            var authenticationService = new AuthenticationService("1582863b46ca4401ba40d1611a2508ce");
            var textToSpeechService = new TextToSpeechService(authenticationService,storageCreds);


           

            TwilioClient.Init(twilioAccount.AccountSid,twilioAccount.AuthToken);

            int counter=1; 
            /*
            foreach( var person in await blobService.GetPhoneNumbersAsync())
            {
            var message = MessageResource.Create(
                        body: messageText,
                        from: new Twilio.Types.PhoneNumber(twilioFrom),
                        to: new Twilio.Types.PhoneNumber(person)
                    );
                    counter++;
            }
            */
            string responseMessage =  $"A message of {messageText} was sent to {counter} people.";
            
            return new OkObjectResult(responseMessage);
        }

      

    }
}
