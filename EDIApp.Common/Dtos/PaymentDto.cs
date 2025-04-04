namespace EDIApp.Common.Dtos
{
	/// <summary>
	/// Holds all needed information for creation payment EDI message.
	/// </summary>
	public class PaymentDto
	{
		/// <summary>
		/// Provides information about payment header.
		/// </summary>
		public string? Header { get; set; }

		/// <summary>
		/// Provides information about sender.
		/// </summary>
		public string? Sender { get; set; }

		/// <summary>
		/// Provides information about sender account.
		/// </summary>
		public string? SenderAccount { get; set; }

		/// <summary>
		/// Provides information about sender bank.
		/// </summary>
		public string? SenderBank { get; set; }

		/// <summary>
		/// Provides information about sender mfo.
		/// </summary>
		public string? SenderMFI { get; set; }

		/// <summary>
		/// Provides information about receiver.
		/// </summary>
		public string? Receiver { get; set; }

		/// <summary>
		/// Provides information about receiver account.
		/// </summary>
		public string? ReceiverAccount { get; set; }

		/// <summary>
		/// Provides information about receiver bank.
		/// </summary>
		public string? ReceiverBank { get; set; }

		/// <summary>
		/// Provides information about receiver mfo.
		/// </summary>
		public string? ReceiverMFI { get; set; }

		/// <summary>
		/// Provides information about payment amount.
		/// </summary>
		public string? Amount { get; set; }

		/// <summary>
		/// Provides information about payment currency.
		/// </summary>
		public string? Currency { get; set; }
	}
}
