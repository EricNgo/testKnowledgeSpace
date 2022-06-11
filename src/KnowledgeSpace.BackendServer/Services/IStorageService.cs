using System.IO;
using System.Threading.Tasks;

namespace KnowledgeSpace.BackendServer.Services
{
    public interface IStorageService
    {
        string GetFileUrl(string Filename);

        Task SaveFileAsync(Stream mediaBinaryStream, string Filename);

        Task DeleteFileAsync(string Filename);

    }
}
