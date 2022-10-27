using AutoFixture.Xunit2;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using FluentAssertions;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Services;

public class PaymentServiceTests
{
    private readonly Mock<IPaymentRequestValidator> _paymentRequestValidator;
    private readonly Mock<IAccountDataStore> _accountDataStore;
    private readonly Mock<IPaymentRequestValidatorFactory> _validatorFactory;
    private readonly Mock<IAccountDataStoreStrategyResolver> _dataStoreStrategyResolver;
    private readonly PaymentService _sut;

    public PaymentServiceTests()
    {
        _paymentRequestValidator = new Mock<IPaymentRequestValidator>();
        _accountDataStore = new Mock<IAccountDataStore>();
        
        _validatorFactory = new Mock<IPaymentRequestValidatorFactory>();
        _validatorFactory.Setup(f => f.CreatePaymentRequestValidator(It.IsAny<PaymentScheme>()))
            .Returns(_paymentRequestValidator.Object);
        
        _dataStoreStrategyResolver = new Mock<IAccountDataStoreStrategyResolver>();
        _dataStoreStrategyResolver.Setup(r => r.Resolve()).Returns(_accountDataStore.Object);
        
        _sut = new PaymentService(_dataStoreStrategyResolver.Object, _validatorFactory.Object);
    }

    [Theory, AutoData]
    public void ShouldUpdateAccountBalance_WhenMakePayment_GivenValidPaymentRequest(MakePaymentRequest paymentRequest, Account account)
    {
        //arrange
        account.Balance += paymentRequest.Amount;
        account.AccountNumber = paymentRequest.DebtorAccountNumber;
        
        var initialBalance = account.Balance;
        decimal updatedBalance = 0;
        
        _paymentRequestValidator.Setup(v => v.Validate(paymentRequest, account)).Returns(true);
        _accountDataStore.Setup(d => d.GetAccount(paymentRequest.DebtorAccountNumber)).Returns(account);
        _accountDataStore.Setup(d =>
                d.UpdateAccount(It.Is<Account>(a => a.AccountNumber == paymentRequest.DebtorAccountNumber)))
            .Callback((Account a) => updatedBalance = a.Balance);
        
        //act
        var result = _sut.MakePayment(paymentRequest);
        
        //assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        updatedBalance.Should().Be(initialBalance - paymentRequest.Amount);
        
        _validatorFactory.Verify(
            f => f.CreatePaymentRequestValidator(It.Is<PaymentScheme>(s => s == paymentRequest.PaymentScheme)),
            Times.Once);
        _paymentRequestValidator.VerifyAll();
        _accountDataStore.VerifyAll();
        _validatorFactory.VerifyAll();
        _dataStoreStrategyResolver.VerifyAll();
    }
    
    [Theory, AutoData]
    public void ShouldNotUpdateAccountBalance_AndReturnFailed_WhenMakePayment_GivenInvalidPaymentRequest(MakePaymentRequest paymentRequest, Account account)
    {
        //arrange
        account.AccountNumber = paymentRequest.DebtorAccountNumber;
        var initialBalance = account.Balance;
        
        _paymentRequestValidator.Setup(v => v.Validate(paymentRequest, account)).Returns(false);
        _accountDataStore.Setup(d => d.GetAccount(paymentRequest.DebtorAccountNumber)).Returns(account);
        
        //act
        var result = _sut.MakePayment(paymentRequest);
        
        //assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        account.Balance.Should().Be(initialBalance);
        
        _accountDataStore.Verify(d => d.UpdateAccount(It.IsAny<Account>()), Times.Never);
        _paymentRequestValidator.VerifyAll();
        _accountDataStore.VerifyAll();
    }
}