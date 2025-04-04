using EDIApp.Common.Dtos;

namespace EDIApp.Generators.Interfaces
{
	/// <summary>
	/// Describe all methods to generate EDI messages.
	/// </summary>
	public interface IEDIGenerator
	{
		/// <summary>
		/// Generate password EDI message.
		/// </summary>
		/// <returns>Password EDI message.</returns>
		string GeneratePasswordMessage(string key = "", string iv = "");

		/// <summary>
		/// Generate payment EDI message.
		/// </summary>
		/// <param name="dto">Payment dto.</param>
		/// <returns>Payment EDI message.</returns>
		string GeneratePaymentMessage(PaymentDto dto);

		/// <summary>
		/// Generate invoice EDI message.
		/// </summary>
		/// <param name="dto">Invoice dto.</param>
		/// <returns>Invoice EDI message.</returns>
		string GenerateInvoiceMessage(InvoiceDto dto);
	}
}
