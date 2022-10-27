using System;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStoreStrategyResolver _dataStoreStrategyResolver;
        private readonly IPaymentRequestValidatorFactory _validatorFactory;

        public PaymentService(
            IAccountDataStoreStrategyResolver dataStoreStrategyResolver,
            IPaymentRequestValidatorFactory validatorFactory)
        {
            _dataStoreStrategyResolver = dataStoreStrategyResolver ?? throw new ArgumentNullException(nameof(dataStoreStrategyResolver));
            _validatorFactory = validatorFactory ?? throw new ArgumentNullException(nameof(validatorFactory));
        }
        
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var accountDataStore = _dataStoreStrategyResolver.Resolve();
            var account = accountDataStore.GetAccount(request.DebtorAccountNumber);
            
            var paymentRequestValidator = _validatorFactory.CreatePaymentRequestValidator(request.PaymentScheme);

            var result = new MakePaymentResult
            {
                Success = paymentRequestValidator.Validate(request, account)
            };

            if (result.Success)
            {
                account.Balance -= request.Amount;
                accountDataStore.UpdateAccount(account);
            }

            return result;
        }
    }
}
