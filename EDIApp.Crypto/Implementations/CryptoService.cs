using System.Text;
using System.Security.Cryptography;

using EDIApp.Crypto.Interfaces;
using EDIApp.Common.Enums;
using System.IO;

namespace EDIApp.Crypto.Implementations
{
	/// <summary>
	///  Realisation of <see cref="ICryptoService"/>.
	/// </summary>
	public class CryptoService : ICryptoService
	{
		#region Private fields

		/// <summary>
		/// Chars for generating random strings.
		/// </summary>
		private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

		/// <summary>
		/// Random.
		/// </summary>
		private readonly Random _random = new();

		/// <summary>
		/// Rsa key lenght.
		/// </summary>
		private readonly int _rsaKeyLenght = 2048;

		#endregion

		#region Public properties

		/// <summary>
		/// Key of crypto service.
		/// </summary>
		public readonly string Key;

		/// <summary>
		/// Init vector of crypto service.
		/// </summary>
		public readonly string IV;

		/// <summary>
		/// RSA public key.
		/// </summary>
		public readonly string RSAPublicKey;

		#endregion

		#region Constructor

		/// <summary>
		/// Inizialise instance of <see cref="CryptoService"/>.
		/// </summary>
		/// <param name="rsaPublicKey">RSA public key.</param>
		public CryptoService(string rsaPublicKey)
		{
			Key = Convert.ToBase64String(Encoding.UTF8.GetBytes(GenerateRandomString(8)));
			IV = Convert.ToBase64String(Encoding.UTF8.GetBytes(GenerateRandomString(8)));
			RSAPublicKey = rsaPublicKey;
		}

		#endregion

		#region Implementation of ICryptoService

		/// <inheritdoc/>
		public async Task<string> DecryptDataAsync(string data, CryptoAlgorithms algorithm)
			=> algorithm switch
				{
					CryptoAlgorithms.DES => await DESDecryptDataAsync(data),
					_ => throw new InvalidDataException($"There is no decryptor for algorithm '{algorithm}'!")
				};

		/// <inheritdoc/>
		public async Task<string> EncryptDataAsync(string data, CryptoAlgorithms algorithm)
			=> algorithm switch
				{
					CryptoAlgorithms.DES => await DESEncryptDataAsync(data),
					CryptoAlgorithms.RSA => await RSAEncryptDataAsync(data),
					_ => throw new InvalidDataException($"There is no encryptor algorithm '{algorithm}'!")
				};

		#endregion

		#region Encryptors

		/// <summary>
		/// Encrypt data using DES algorithm.
		/// </summary>
		/// <param name="data">Data to encrypt.</param>
		/// <returns>Base64 string of encrypted data.</returns>
		private async Task<string> DESEncryptDataAsync(string data)
		{
			using (var des = GetDES())
			{
				using (var stream = new MemoryStream())
				{
					using (var cryptoStream = new CryptoStream(stream, des.CreateEncryptor(), CryptoStreamMode.Write))
					{
						var dataBytes = Encoding.UTF8.GetBytes(data);

						cryptoStream.Write(dataBytes, 0, dataBytes.Length);
						await cryptoStream.FlushFinalBlockAsync();

						return Convert.ToBase64String(stream.ToArray());
					}
				}
			}
		}

		/// <summary>
		/// Encrypt data using RSA algorithm.
		/// </summary>
		/// <param name="data">Data to encrypt.</param>
		/// <returns>Base64 string of encrypted data.</returns>
		private Task<string> RSAEncryptDataAsync(string data)
		{
			using (var rsa = GetRSA())
			{
				var dataBytes = Encoding.UTF8.GetBytes(data);
				var ecryptedData = rsa.Encrypt(dataBytes, RSAEncryptionPadding.Pkcs1);

				return Task.FromResult(Convert.ToBase64String(ecryptedData));
			}
		}

		#endregion

		#region Decryptors

		/// <summary>
		/// Decrypt data using DES algorithm.
		/// </summary>
		/// <param name="data">Data to decrypt.</param>
		/// <returns>Decrypted data string.</returns>
		private async Task<string> DESDecryptDataAsync(string data)
		{
			using (var des = GetDES())
			{
				using (var stream = new MemoryStream())
				{
					using (var cryptoStream = new CryptoStream(stream, des.CreateDecryptor(), CryptoStreamMode.Write))
					{
						var dataBytes = Convert.FromBase64String(data);

						cryptoStream.Write(dataBytes, 0, dataBytes.Length);
						await cryptoStream.FlushFinalBlockAsync();

						return Convert.ToBase64String(stream.ToArray());
					}
				}
			}
		}

		#endregion

		#region Private fields

		/// <summary>
		/// Generate random string.
		/// </summary>
		/// <param name="lenght">String lenght.</param>
		/// <returns>Generated string.</returns>
		private string GenerateRandomString(int lenght)
			=> new(Enumerable.Repeat(_chars, lenght)
				.Select(s => s[_random.Next(s.Length)]).ToArray());

		/// <summary>
		/// Create and configure DES.
		/// </summary>
		/// <returns>DES.</returns>
		private DES GetDES()
		{
			var des = DES.Create();

			des.GenerateIV();

			des.Key = Convert.FromBase64String(Key);
			des.IV = Convert.FromBase64String(IV);

			return des;
		}

		private RSA GetRSA()
		{
			RSA rsa = RSA.Create(_rsaKeyLenght);

			rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(RSAPublicKey), out _);

			return rsa;
		}

		#endregion
	}
}
