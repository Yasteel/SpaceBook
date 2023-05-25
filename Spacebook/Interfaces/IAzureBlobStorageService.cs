using Azure.Storage.Blobs;

namespace Spacebook.Interfaces
{
    public interface IAzureBlobStorageService
    {

        BlobContainerClient CreateContainer(string containerName);

        string UploadBlob(IFormFile file, string containerName);

        void DeleteContainer(string containerName);

        void GetContainer(string containerName);

        void GetAllContainers();

        void GetBlob();

        List<string> GetAllBlobs(string containerName);

        BlobServiceClient CreateConnection();
    }
}
