using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Spacebook.Interfaces;

namespace Spacebook.Services
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {


        public BlobContainerClient CreateContainer(string containerName)
        {
            var blobServiceClient = CreateConnection();
            return blobServiceClient.CreateBlobContainer(containerName);
        }

        /* 
         Returns the filename of the file uploaded so that filename
         can be referenced in the database
         */
        public string UploadBlob(IFormFile file, string containerName) 
        {
            var blobServiceClient = CreateConnection();
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            string fileName = containerName + Guid.NewGuid().ToString();
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

           
            using (var stream = file.OpenReadStream())
            {
                blobClient.Upload(stream, true);
            }

            BlobHttpHeaders headers = new BlobHttpHeaders();
            headers.ContentType = file.ContentType;
            blobClient.SetHttpHeaders(headers);

            return "https://payconnect.blob.core.windows.net" + containerName + "/" + fileName;
        }

        public void DeleteContainer(string containerName)
        {
            throw new NotImplementedException();
        }

        public List<string> GetAllBlobs(string containerName)
        {
            var blobServiceClient = CreateConnection();
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            List<string> blobs = new List<string>();
            foreach (BlobItem blobItem in containerClient.GetBlobs())
            {
                blobs.Add("https://payconnect.blob.core.windows.net" + containerName + "/" + blobItem.Name);
            }

            return blobs;
        }

        public void GetAllContainers()
        {
            throw new NotImplementedException();
        }

        public void GetBlob()
        {
            throw new NotImplementedException();
        }

        public void GetContainer(string containerName)
        {
            throw new NotImplementedException();
        }

        public BlobServiceClient CreateConnection()
        {
            string? connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Cannot connect to Azure Blob Storage");
            }

            return new BlobServiceClient(connectionString);
        }
    }
}
