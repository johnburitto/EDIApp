using EDIApp.Common.Enums;

namespace EDIApp.Crypto.Interfaces
{
	/// <summary>
	/// Describe all methods to process crypto operations.
	/// </summary>
	public interface ICryptoService
	{
		/// <summary>
		/// Ecrypt data.
		/// </summary>
		/// <param name="data">Data to encrypt.</param>
		/// <returns>Encrypted data.</returns>
		Task<string> EncryptDataAsync(string data, CryptoAlgorithms algorithm);

		/// <summary>
		/// Decrypt data.
		/// </summary>
		/// <param name="data">Data to decrypt.</param>
		/// <returns>Decrypted data.</returns>
		Task<string> DecryptDataAsync(string data, CryptoAlgorithms algorithm);
	}
}
