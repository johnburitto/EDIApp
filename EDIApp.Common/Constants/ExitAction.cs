using System.Net.WebSockets;

namespace EDIApp.Common.Constants
{
	/// <summary>
	/// Generate exit action for web socket.
	/// </summary>
	public static class ExitAction
	{
		#region Private fields

		/// <summary>
		/// Cancellation token
		/// </summary>
		private static CancellationToken _cancellationToken = CancellationToken.None;

		#endregion

		#region Public methods

		public static Func<ClientWebSocket, string, Task<bool>> GenerateExitAction(string exitMessage)
			=>  async (socket, input) =>
			{
				if (input == exitMessage)
				{
					await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bye", _cancellationToken);

					return true;
				}

				return false;
			};

		#endregion
	}
}
