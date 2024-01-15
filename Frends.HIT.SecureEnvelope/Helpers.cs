using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.IO.Compression;

namespace Frends.HIT.SecureEnvelope {

    /// <summary>
    /// Helper classes for various operations
    /// </summary>
    internal class Helpers {

        /// <summary>
        /// Get an X509Certificate from cert and privkey string
        /// </summary>
        /// <param name="certificate">The Base64-encoded PEM Certificate</param>
        /// <param name="privatekey">The Base64-encoded PEM Private Key</param>
        /// <returns></returns>
        internal static X509Certificate2 GetX509Certificate(string certificate, string privatekey) {
            if (string.IsNullOrEmpty(certificate)) {
                throw new InvalidDataException("Certificate is not specified");
            }
            
            byte[] certData = Convert.FromBase64String(certificate);
            string decodedCert = Encoding.UTF8.GetString(certData);

            byte[] privkeyData = Convert.FromBase64String(privatekey);
            string decodedPrivkey = Encoding.UTF8.GetString(privkeyData);

            var xcert = X509Certificate2.CreateFromPem(decodedCert, decodedPrivkey);

            if (xcert.NotAfter < DateTime.Now) {
                throw new InvalidDataException("Certificate has expired, please replace");
            }
            
            return xcert;
        }

        /// <summary>
        /// Base64-encode a string
        /// </summary>
        /// <param name="input">The string</param>
        /// <returns>Base64 Result</returns>
        internal static string GetBase64String(string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        }

        /// <summary>
        /// Get a UTF8 string from a Base64-encoded string
        /// </summary>
        /// <param name="base64">The base64-encoded string</param>
        /// <returns>Cleartext UTF8 String</returns>
        internal static string GetUtf8StringFromBase64(string base64)
        {
            return string.IsNullOrEmpty(base64) ? string.Empty : Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        }

        /// <summary>
        /// Get content from Base64-encoded string
        /// </summary>
        /// <param name="input">The input data, base64-encoded</param>
        /// <param name="compressedWithGzip">Whether the data is compressed with Gzip</param>
        /// <param name="fileEncoding">The relevant encoding</param>
        /// <returns>String with decoded data</returns>
        internal static string GetContentFromBase64(string input, bool compressedWithGzip, Encoding fileEncoding)
        {
            var input64 = Convert.FromBase64String(input);

            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(input64, 0, input64.Length);
                memoryStream.Position = 0L;

                if (compressedWithGzip)
                {
                    using (var resultStream = new MemoryStream())
                    {
                        using (var gzip = new GZipStream(memoryStream, CompressionMode.Decompress))
                        {
                            gzip.CopyTo(resultStream);
                            // Here the memoryStream is disposed with gzip
                            return ReadStream(resultStream, fileEncoding);
                        }
                    }
                }
                // Here we dispose memoryStream again, but this should not create issues
                return ReadStream(memoryStream, fileEncoding);
            }
        }

        /// <summary>
        /// Base64-encode a string
        /// </summary>
        /// <param name="input">The string</param>
        /// <returns>The base64-encoded string</returns>
        internal static string Base64Encode(string input) {
            var inputByteArray = Encoding.UTF8.GetBytes(input);

            return Convert.ToBase64String(inputByteArray, Base64FormattingOptions.None);
        }

        internal static string Base64Decode(string input) {
            var inputByteArray = Convert.FromBase64String(input);

            return Encoding.UTF8.GetString(inputByteArray);
        }

        /// <summary>
        /// Compress with Gzip and base64-encode the result
        /// </summary>
        /// <param name="input">The original string</param>
        /// <returns>The gzipped and base64-encoded result</returns>
        internal static string GzipAndBase64Encode(string input)
        {
            var inputByteArray = Encoding.UTF8.GetBytes(input);

            using (var resultStream = new MemoryStream())
            {
                using (var gzip = new GZipStream(resultStream, CompressionMode.Compress))
                {
                    // Disposing of gzip also disposes resultStream
                    gzip.Write(inputByteArray, 0, inputByteArray.Length);
                }

                var resultBytes = resultStream.ToArray();
                
                // Here we dispose resultStream again, but this should not create issues
                return Convert.ToBase64String(resultBytes, Base64FormattingOptions.None);
            }
        }

        /// <summary>
        /// Read a full stream and return the contents
        /// </summary>
        /// <param name="stream">The stream to read</param>
        /// <param name="fileEncoding">The encoding to use for the resulting data</param>
        /// <returns>A string with the stream contents</returns>
        internal static string ReadStream(Stream stream, Encoding fileEncoding)
        {
            stream.Position = 0L;

            using (var reader = new StreamReader(stream, fileEncoding))
            {
                return reader.ReadToEnd();
            }
        }
    }
}