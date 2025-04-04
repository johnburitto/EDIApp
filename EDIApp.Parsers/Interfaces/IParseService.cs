using EDIApp.Common.Dtos;

namespace EDIApp.Parsers.Interfaces
{
	/// <summary>
	/// Describe all methods to parse data for EDI messages.
	/// </summary>
	public interface IParseService
	{
		/// <summary>
		/// Parse payment data from user input.
		/// </summary>
		/// <param name="data">Data to parse from.</param>
		/// <returns>Payment dto.</returns>
		public PaymentDto ParsePaymentData(string data);

		/// <summary>
		/// Parse invoice data from user input.
		/// </summary>
		/// <param name="data">Data to parse from.</param>
		/// <returns>Invoice dto.</returns>
		public InvoiceDto ParseInvoiceData(string data);
	}
}
