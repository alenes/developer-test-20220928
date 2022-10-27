using System;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators;

public class PaymentRequestValidatorFactory : IPaymentRequestValidatorFactory
{
    public IPaymentRequestValidator CreatePaymentRequestValidator(PaymentScheme paymentScheme) =>
        paymentScheme switch
        {
            PaymentScheme.Bacs => new BacsPaymentRequestValidator(),
            PaymentScheme.FasterPayments => new FasterPaymentRequestValidator(),
            PaymentScheme.Chaps => new ChapsPaymentRequestValidator(),
            _ => throw new ArgumentOutOfRangeException(nameof(paymentScheme), paymentScheme,
                $"Payment scheme {paymentScheme} is not supported")
        };
}