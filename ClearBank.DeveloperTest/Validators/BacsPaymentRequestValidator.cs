using ClearBank.DeveloperTest.Types;
// ReSharper disable IdentifierTypo

namespace ClearBank.DeveloperTest.Validators;

public class BacsPaymentRequestValidator: IPaymentRequestValidator
{
    public bool Validate(MakePaymentRequest paymentRequest, Account account) =>
        account != null && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs);
}