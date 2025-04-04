using System.Text;
using System.Net.WebSockets;

using EDIApp.Common.Dtos;
using EDIApp.Common.Enums;
using EDIApp.Common.Logging;
using EDIApp.Socket.Interfaces;
using EDIApp.Parsers.Interfaces;
using EDIApp.Generators.Interfaces;
using EDIApp.Crypto.Implementations;
using EDIApp.Parsers.Implementations;
using EDIApp.Generators.Implementations;
using System.Diagnostics;

namespace EDIApp.Socket.Implementations
{
	/// <summary>
	/// Realisation of <see cref="ICustomWebSocket"/>.
	/// </summary>
	public class CustomWebSocket : ICustomWebSocket
	{
		#region Private fields

		/// <summary>
		/// Crypto service.
		/// </summary>
		private CryptoService? _cryptoService;

		/// <summary>
		/// Parse service.
		/// </summary>
		private readonly IParseService _parseService;

		/// <summary>
		/// EDI generator.
		/// </summary>
		private readonly IEDIGenerator _ediGenerator;

		/// <summary>
		/// Buffer size.
		/// </summary>
		private readonly int _bufferSize = 10 * 1024 * 1024;

		/// <summary>
		/// Web socket byffer.
		/// </summary>
		private readonly byte[] _buffer;

		/// <summary>
		/// Cancellation token
		/// </summary>
		private readonly CancellationToken _cancellationToken = CancellationToken.None;

		#endregion

		#region Constructor

		/// <summary>
		/// Inizialise instance of <see cref="CustomWebSocket"/>.
		/// </summary>
		public CustomWebSocket()
		{
			_parseService = new ParseService();
			_ediGenerator = new EDIGenerator();
			_buffer = new byte[_bufferSize];
		}

		#endregion

		#region Implementation of ICustomWebSocket

		/// <inheritdoc/>
		public async Task StartWebsocketAsync(Func<ClientWebSocket, string, Task<bool>>? exitAction = null)
		{
			Console.InputEncoding = Encoding.UTF8;
			Console.OutputEncoding = Encoding.UTF8;

			using (var socket = new ClientWebSocket())
			{
				try
				{
					await HandshakeAsync(socket);

					while (true)
					{
						Logger.UserInput(out string input);

						if (await exitAction!.Invoke(socket, input))
						{
							return;
						}

						(var dto, var type) = GetEDIDto(input);
						var message = await _cryptoService!.EncryptDataAsync(GenerateEDIMessage(dto, type), CryptoAlgorithms.DES);

						await SendMessageAsync(socket, message, type);
						await ProcessResposeAsync(socket, type);
					}
				}
				catch (Exception e)
				{
					Logger.Error(e.Message);
				}
			}
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Handshake.
		/// </summary>
		/// <param name="socket">Web socket.</param>
		private async Task HandshakeAsync(ClientWebSocket socket)
		{
			await socket.ConnectAsync(new Uri("ws://localhost:8080/ws"), _cancellationToken);

			Logger.Information("WebSocket connect successfully");

			var result = await socket.ReceiveAsync(new ArraySegment<byte>(_buffer), _cancellationToken);
			var stringResult = Encoding.UTF8.GetString(_buffer, 0, result.Count);

			if (!string.IsNullOrEmpty(stringResult))
			{
				_cryptoService = new CryptoService(stringResult);
				
				Logger.Debug("Crypto service created");

				var passwordMessage = _ediGenerator.GeneratePasswordMessage(_cryptoService.Key, _cryptoService.IV);
				var encryptyedMessage = await _cryptoService.EncryptDataAsync(passwordMessage, CryptoAlgorithms.RSA);

				await SendMessageAsync(socket, encryptyedMessage, EDIType.Password);
			}

			Logger.Information("Handshake executed successfully");
		}

		/// <summary>
		/// Sends message via web socket.
		/// </summary>
		/// <param name="socket">Web socket.</param>
		/// <param name="message">Message.</param>
		private async Task SendMessageAsync(ClientWebSocket socket, string? message, EDIType type)
		{
			var messageBytes = Encoding.UTF8.GetBytes(message ?? throw new ArgumentNullException(nameof(message)));

			await socket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, _cancellationToken);

			Logger.Debug($"{type} message send successfully");
		}

		/// <summary>
		/// Process web socket response.
		/// </summary>
		/// <param name="socket"></param>
		/// <returns></returns>
		private async Task ProcessResposeAsync(ClientWebSocket socket, EDIType type)
		{
			var response = await socket.ReceiveAsync(new ArraySegment<byte>(_buffer), _cancellationToken);
			var encryptedMessage = Encoding.UTF8.GetString(_buffer, 0, response.Count);
			var message = await _cryptoService!.DecryptDataAsync(encryptedMessage, CryptoAlgorithms.DES);
			var path = $"{Directory.GetCurrentDirectory()}/Files";
			var filePath = $"{path}/{type}-{DateTime.UtcNow.ToFileTimeUtc()}.pdf";

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			File.WriteAllBytes(filePath, Convert.FromBase64String(message));
			Process.Start(new ProcessStartInfo(filePath)
			{
				UseShellExecute = true
			});

			Logger.Debug($"{type} message server response was processed successfully");
		}

		/// <summary>
		/// Gets EDI data dto from input.
		/// </summary>
		/// <param name="data">Data.</param>
		/// <returns>EDI data dto.</returns>
		private (object, EDIType) GetEDIDto(string data)
			=> data.Contains("Оплата за товар") 
				? (_parseService.ParsePaymentData(data), EDIType.Payment) 
				: (_parseService.ParseInvoiceData(data), EDIType.Invoice);

		/// <summary>
		/// Generates EDI message.
		/// </summary>
		/// <param name="data">Data to generate from.</param>
		/// <param name="type">EDI message type.</param>
		/// <returns>EDI message.</returns>
		private string GenerateEDIMessage(object data, EDIType type)
			=> type switch
				{
					EDIType.Payment => _ediGenerator.GeneratePaymentMessage((PaymentDto)data),
					EDIType.Invoice => _ediGenerator.GenerateInvoiceMessage((InvoiceDto)data),
					_ => throw new InvalidDataException($"There is no EDI type '{type}'!")
				};

		#endregion
	}
}
