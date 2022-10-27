using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators;

public interface IPaymentRequestValidator
{
    public bool Validate(MakePaymentRequest paymentRequest, Account account);
}