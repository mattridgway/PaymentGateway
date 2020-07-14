using System.Threading.Tasks;

namespace Stark.Encryption
{
    public interface IEncryptObjects
    {
        Task<T> DecryptAsync<T>(byte[] source);
        Task<byte[]> EncryptAsync<T>(T source);
    }
}
