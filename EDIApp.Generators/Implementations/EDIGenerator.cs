using EDIApp.Common.Constants;
using EDIApp.Common.Dtos;
using EDIApp.Generators.Interfaces;

namespace EDIApp.Generators.Implementations
{
	/// <summary>
	/// Realisation of <see cref="IEDIGenerator"/>.
	/// </summary>
	public class EDIGenerator : IEDIGenerator
	{
		#region Private fields
		
		/// <summary>
		/// Message number.
		/// </summary>
		private int _messageNumber = 1;
		
		#endregion

		#region Implementation of IEDIGenerator

		/// <inheritdoc/>
		public string GeneratePasswordMessage(string key = "", string iv = "")
		{
			var message = $"{EDIBlocksNames.Header}{EDIBlocksNames.Delimiter}{_messageNumber.ToString().PadLeft(4, '0')}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.MessageStart}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.Date}{EDIBlocksNames.Delimiter}{DateTime.UtcNow.ToString("yyyyMMdd:HHmm")}:24{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.ItemDescription}{EDIBlocksNames.Delimiter}{(_messageNumber + 1).ToString().PadLeft(4, '0')}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.CryptoAlgorithm}{EDIBlocksNames.Delimiter}DES{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.CryptoKey}{EDIBlocksNames.Delimiter}{key}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.CryptoIV}{EDIBlocksNames.Delimiter}{iv}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.MessageEnd}{EDIBlocksNames.SegmentEnd}";

			message += $"{EDIBlocksNames.PacketEnd}{EDIBlocksNames.Delimiter}{message.Count(s => s.Equals('\'')).ToString().PadLeft(4, '0')}{EDIBlocksNames.SegmentEnd}";

			_messageNumber++;

			return message;
		}

		/// <inheritdoc/>
		public string GeneratePaymentMessage(PaymentDto dto)
		{
			var message = $"{EDIBlocksNames.Header}{EDIBlocksNames.Delimiter}{_messageNumber.ToString().PadLeft(4, '0')}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.MessageStart}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.Date}{EDIBlocksNames.Delimiter}{DateTime.UtcNow.ToString("yyyyMMdd:HHmm")}:24{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.DocumentName}{EDIBlocksNames.Delimiter}{EDIBlocksNames.Payment}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.Payer}{EDIBlocksNames.Delimiter}{dto.Sender}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.SenderBank}{EDIBlocksNames.Delimiter}{dto.SenderBank}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.SenderMFI}{EDIBlocksNames.Delimiter}{dto.SenderMFI}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.SenderAccount}{EDIBlocksNames.Delimiter}{dto.SenderAccount}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.Receiver}{EDIBlocksNames.Delimiter}{dto.Receiver}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.ReceiverBank}{EDIBlocksNames.Delimiter}{dto.ReceiverBank}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.ReceiverMFI}{EDIBlocksNames.Delimiter}{dto.ReceiverMFI}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.ReceiverAccount}{EDIBlocksNames.Delimiter}{dto.ReceiverAccount}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.ItemDescription}{EDIBlocksNames.Delimiter}{dto.Header}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.Currency}{EDIBlocksNames.Delimiter}{dto.Currency}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.Sum}{EDIBlocksNames.Delimiter}{dto.Amount}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.MessageEnd}{EDIBlocksNames.SegmentEnd}";

			message += $"{EDIBlocksNames.PacketEnd}{EDIBlocksNames.Delimiter}{message.Count(s => s.Equals('\'')).ToString().PadLeft(4, '0')}{EDIBlocksNames.SegmentEnd}";

			_messageNumber++;

			return message;
		}

		/// <inheritdoc/>
		public string GenerateInvoiceMessage(InvoiceDto dto)
		{
			var message = $"{EDIBlocksNames.Header}{EDIBlocksNames.Delimiter}{_messageNumber.ToString().PadLeft(4, '0')}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.MessageStart}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.Date}{EDIBlocksNames.Delimiter}{DateTime.UtcNow.ToString("yyyyMMdd:HHmm")}:24{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.DocumentName}{EDIBlocksNames.Delimiter}{EDIBlocksNames.Invoice}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.Buyer}{EDIBlocksNames.Delimiter}{dto.Buyer}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.Receiver}{EDIBlocksNames.Delimiter}{dto.Receiver}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.Goods}{EDIBlocksNames.Delimiter}{dto.Goods}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.Amount}{EDIBlocksNames.Delimiter}{dto.Amount}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.OrderPrice}{EDIBlocksNames.Delimiter}{dto.OrderPrice}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.MeasurementUnit}{EDIBlocksNames.Delimiter}{dto.MeasurementUnit}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.Sum}{EDIBlocksNames.Delimiter}{dto.Sum}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.ItemDescription}{EDIBlocksNames.Delimiter}{dto.Header}{EDIBlocksNames.SegmentEnd}" +
				$"{EDIBlocksNames.MessageEnd}{EDIBlocksNames.SegmentEnd}";

			message += $"{EDIBlocksNames.PacketEnd}{EDIBlocksNames.Delimiter}{message.Count(s => s.Equals('\'')).ToString().PadLeft(4, '0')}{EDIBlocksNames.SegmentEnd}";

			_messageNumber++;

			return message;
		}

		#endregion
	}
}
