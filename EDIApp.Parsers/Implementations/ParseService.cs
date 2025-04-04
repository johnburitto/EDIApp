using System.Text.RegularExpressions;

using EDIApp.Common.Dtos;
using EDIApp.Parsers.Interfaces;

namespace EDIApp.Parsers.Implementations
{
	/// <summary>
	/// Realisation of <see cref="IParseService"/>.
	/// </summary>
	public class ParseService : IParseService
	{
		#region Regexes

		/// <summary>
		/// Payment segemnt regex template.
		/// </summary>
		private const string _paymentSegmentTemplate = @"{0}(\s*.*?\n?)+?\{1}";

		/// <summary>
		/// Line regex template.
		/// </summary>
		private const string _lineTemplate = @"{0}(.*?)[;.:]";

		#endregion

		#region Implementation of IParseService

		/// <inheritdoc/>
		public InvoiceDto ParseInvoiceData(string data)
			=> new()
			{
				Header = RegexSegment(data, string.Format(_paymentSegmentTemplate, "Поставка товару", ";")),
				Buyer = TrimData(RegexSegment(data, string.Format(_lineTemplate, "Платник")), "Платник - "),
				Receiver = TrimData(RegexSegment(data, string.Format(_lineTemplate, "Отримувач")), "Отримувач - "),
				Goods = TrimData(RegexSegment(data, string.Format(_lineTemplate, "Товар")), "Товар - "),
				Amount = TrimData(RegexSegment(data, string.Format(_lineTemplate, "Кількість")), "Кількість - "),
				OrderPrice = TrimData(RegexSegment(data, string.Format(_lineTemplate, "Ціна")), "Ціна - "),
				MeasurementUnit = TrimData(RegexSegment(data, string.Format(_lineTemplate, "Одиниця виміру")), "Одиниця виміру - "),
				Sum = TrimData(RegexSegment(data, string.Format(_lineTemplate, "Сума")), "Сума - ")
			};

		/// <inheritdoc/>
		public PaymentDto ParsePaymentData(string data)
		{
			var dto = new PaymentDto()
			{
				Header = RegexSegment(data, string.Format(_paymentSegmentTemplate, "Оплата за товар", ";"))
			};
			var senderSegment = RegexSegment(data, string.Format(_paymentSegmentTemplate, "Платник", "."));
			var receiverSegment = RegexSegment(data, string.Format(_paymentSegmentTemplate, "Отримувач", "."));
			var amountSegment = RegexSegment(data, string.Format(_paymentSegmentTemplate, "Переказ", "."));

			ParsePaymentSenderData(dto, senderSegment);
			ParsePaymentReceiverData(dto, receiverSegment);
			ParsePaymentAmountData(dto, amountSegment);

			return dto;
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Parse payment sender data.
		/// </summary>
		/// <param name="data">Data to parse.</param>
		private static void ParsePaymentSenderData(PaymentDto dto, string data)
		{
			dto.Sender = TrimData(RegexSegment(data, string.Format(_lineTemplate, "Платник")), "Платник - ");
			dto.SenderAccount = TrimData(RegexSegment(data, string.Format(_lineTemplate, "Рахунок")), "Рахунок - ");
			dto.SenderBank = TrimData(RegexSegment(data, string.Format(_lineTemplate, "Банк")), "Банк - ");
			dto.SenderMFI = TrimData(RegexSegment(data, string.Format(_lineTemplate, "МФО")), "МФО - ");
		}

		/// <summary>
		/// Parse payment receiver data.
		/// </summary>
		/// <param name="data">Data to parse.</param>
		private static void ParsePaymentReceiverData(PaymentDto dto, string data)
		{
			dto.Receiver = TrimData(RegexSegment(data, string.Format(_lineTemplate, "Отримувач")), "Отримувач - ");
			dto.ReceiverAccount = TrimData(RegexSegment(data, string.Format(_lineTemplate, "Рахунок")), "Рахунок - ");
			dto.ReceiverBank = TrimData(RegexSegment(data, string.Format(_lineTemplate, "Банк")), "Банк - ");
			dto.ReceiverMFI = TrimData(RegexSegment(data, string.Format(_lineTemplate, "МФО")), "МФО - ");
		}

		/// <summary>
		/// Parse payment amount data.
		/// </summary>
		/// <param name="data">Data to parse.</param>
		private static void ParsePaymentAmountData(PaymentDto dto, string data)
		{
			dto.Amount = TrimData(RegexSegment(data, string.Format(_lineTemplate, "Сума")), "Сума - ");
			dto.Currency = TrimData(RegexSegment(data, string.Format(_lineTemplate, "Валюта")), "Валюта - ");
		}

		/// <summary>
		/// Get segemnt from string data using regex
		/// </summary>
		/// <param name="data">String data.</param>
		/// <param name="regex">Regex.</param>
		/// <returns>String segment.</returns>
		private static string RegexSegment(string data, string regex)
			=> Regex.Match(data, regex).Value;

		/// <summary>
		/// Trim data from not necessary data.
		/// </summary>
		/// <param name="data">Data to trim.</param>
		/// <param name="start">Start of data to remove.</param>
		/// <returns>Trimed data.</returns>
		private static string TrimData(string data, string start)
			=> data.Replace(start, "")
				.Replace(":", "")
				.Replace(";", "")
				.Replace(".", "");

		#endregion
	}
}
