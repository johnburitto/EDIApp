using System.Net.WebSockets;

namespace EDIApp.Socket.Interfaces
{
	/// <summary>
	/// Describe all methods to process and send data via web socket.
	/// </summary>
	public interface ICustomWebSocket
	{
		/// <summary>
		/// Starts web socket.
		/// </summary>
		/// <param name="exitAction">Actiopn to close the app.</param>
		public Task StartWebsocketAsync(Func<ClientWebSocket, string, Task<bool>>? exitAction = null);
	}
}
