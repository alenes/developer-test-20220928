using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators;

public class ChapsPaymentRequestValidator: IPaymentRequestValidator
{
    public bool Validate(MakePaymentRequest paymentRequest, Account account) =>
        account != null && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) &&
        account.Status == AccountStatus.Live;
}