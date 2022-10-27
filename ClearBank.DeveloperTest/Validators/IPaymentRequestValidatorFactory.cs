using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators;

public interface IPaymentRequestValidatorFactory
{
    public IPaymentRequestValidator CreatePaymentRequestValidator(PaymentScheme paymentScheme);
}