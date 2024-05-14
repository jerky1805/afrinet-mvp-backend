//u/sing Afrinet.Services;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Afrinet.Web;

public class BlobStorageService : IBlobStorageService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<BlobStorageService> _logger;

    string blobStorageconnection = string.Empty;
    string blobContainerName = string.Empty;
    public BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        blobStorageconnection = _configuration.GetValue<string>("Azure_Storage_Conn_String");
        blobContainerName = _configuration.GetValue<string>("Azure_Storage_Container");
    }

    public async Task<string> UploadFileToBlobAsync(string strFileName, string contecntType, Stream fileStream)
    {
        try
        {
            var container = new BlobContainerClient(blobStorageconnection, blobContainerName);
            var createResponse = await container.CreateIfNotExistsAsync();
            if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            var blob = container.GetBlobClient(strFileName);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
            await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contecntType });
            var urlString = blob.Uri.ToString();
            return urlString;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex.ToString());
            throw;
        }
    }

    public async Task<bool> DeleteFileToBlobAsync(string strFileName)
    {
        try
        {
            var container = new BlobContainerClient(blobStorageconnection, blobContainerName);
            var createResponse = await container.CreateIfNotExistsAsync();
            if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            var blob = container.GetBlobClient(strFileName);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex.ToString());
            throw;
        }
    }
}
