using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using NADT.Models;

namespace NADT.Services
{
    //private string blobConnectionString ="DefaultEndpointsProtocol=https;AccountName=cs710033fff871335fa;AccountKey=AqH/kXfWKQ5ZCgAvnf+2x2zjVQoeNwPn0vYgB0A2CDvEiXymWfhppV4x03iyY1efFMT4xRun9SRBq1BsKExIHw==;EndpointSuffix=core.windows.net";
    public class BlobService
    {        private readonly StorageCreds creds;

        public BlobService( StorageCreds creds)
        {
            this.creds = creds ?? throw new System.ArgumentNullException(nameof(creds));
        }

        public async Task<List<string>> GetPhoneNumbersAsync ()
        {
            
 
            const string fileName = "alertcells.csv";
            const string containerName = "nadt";
                // Setup the connection to the storage account
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(creds.ConnectionString);
                
                // Connect to the blob storage
                CloudBlobClient serviceClient = storageAccount.CreateCloudBlobClient();
                // Connect to the blob container
                CloudBlobContainer container = serviceClient.GetContainerReference($"{containerName}");
                // Connect to the blob file
                CloudBlockBlob blob = container.GetBlockBlobReference($"{fileName}");
                // Get the blob file as text
                string contents = await blob.DownloadTextAsync();
                
                return GetListFromString(contents);
        } 

        private List<string> GetListFromString( string content)
        {
            string[] array = content.Split("\r\n");
            var result = new List<string>();
            result.AddRange(array); 
            return result;

        }
        public async Task<string> StoreSoundbite(byte[] soundBite)
        { 
                var blobPath = "https://cs710033fff871335fa.blob.core.windows.net/nadt/";
                var name = Path.GetRandomFileName();
                var filename = Path.ChangeExtension(name, ".mp3");
                var urlString = blobPath + filename;

                var creds = new StorageCredentials( this.creds.Account, this.creds.Key);
                var blob = new CloudBlockBlob(new Uri(urlString), creds);
                blob.Properties.ContentType = "audio/mpeg";

                if (!(await blob.ExistsAsync().ConfigureAwait(false)))
                {
                    await blob
                            .UploadFromByteArrayAsync(soundBite, 0, soundBite.Length)
                            .ConfigureAwait(false);
                    }

                return urlString;
        }


    }
}