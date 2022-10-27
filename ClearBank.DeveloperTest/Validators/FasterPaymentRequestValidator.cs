using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators;

public class FasterPaymentRequestValidator: IPaymentRequestValidator
{
    public bool Validate(MakePaymentRequest paymentRequest, Account account) =>
        account != null && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) &&
        account.Balance >= paymentRequest.Amount;
}