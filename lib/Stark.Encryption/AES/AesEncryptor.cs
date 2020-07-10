using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Stark.Encryption.AES
{
    internal class AesEncryptor : IEncryptObjects, IDisposable
    {
        private readonly Aes _aes;
        private bool _disposedValue;

        public AesEncryptor(IOptions<AesEncryptorOptions> options)
        {
            _aes = Aes.Create();
            _aes.Key = options.Value.Key;
            _aes.IV = options.Value.IV;
        }

        public async Task<T> DecryptAsync<T>(byte[] source)
        {
            var decryptor = _aes.CreateDecryptor();
            using var memoryStream = new MemoryStream(source);
            using var cryptostream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var reader = new StreamReader(cryptostream);
            var encrypted = await reader.ReadToEndAsync();
            return JsonConvert.DeserializeObject<T>(encrypted);
        }

        public async Task<byte[]> EncryptAsync<T>(T source)
        {
            var encryptor = _aes.CreateEncryptor();
            using var memoryStream = new MemoryStream();
            using (var cryptostream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            using (var streamWriter = new StreamWriter(cryptostream))
            {
                await streamWriter.WriteAsync(JsonConvert.SerializeObject(source));
            }
            return memoryStream.ToArray();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _aes.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
