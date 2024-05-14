namespace Afrinet.Web;
public interface IBlobStorageService
{
    Task<string> UploadFileToBlobAsync(string strFileName, string contentType, Stream fileStream);
    Task<bool> DeleteFileToBlobAsync(string strFileName);
}