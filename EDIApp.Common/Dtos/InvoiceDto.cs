namespace EDIApp.Common.Dtos
{
	/// <summary>
	/// Holds all needed information for creation invoice EDI message.
	/// </summary>
	public class InvoiceDto
	{
		/// <summary>
		/// Provides inforamtion about sender.
		/// </summary>
		public string? Header { get; set; }

		/// <summary>
		/// Provides inforamtion about sender.
		/// </summary>
		public string? Buyer { get; set; }

		/// <summary>
		/// Provides inforamtion about receiver.
		/// </summary>
		public string? Receiver { get; set; }

		/// <summary>
		/// Provides inforamtion about goods.
		/// </summary>
		public string? Goods { get; set; }

		/// <summary>
		/// Provides inforamtion about amount.
		/// </summary>
		public string? Amount { get; set; }

		/// <summary>
		/// Provides inforamtion about order price.
		/// </summary>
		public string? OrderPrice { get; set; }

		/// <summary>
		/// Provides inforamtion about sender.
		/// </summary>
		public string? MeasurementUnit { get; set; }

		/// <summary>
		/// Provides inforamtion about sum.
		/// </summary>
		public string? Sum { get; set; }
	}
}
