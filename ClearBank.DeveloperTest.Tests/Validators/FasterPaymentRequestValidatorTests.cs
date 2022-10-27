using AutoFixture.Xunit2;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Validators;

public class FasterPaymentRequestValidatorTests
{
    private readonly FasterPaymentRequestValidator _sut;
    
    public FasterPaymentRequestValidatorTests()
    {
        _sut = new FasterPaymentRequestValidator();
    }

    [Theory, AutoData]
    public void ShouldValidatePaymentMethodWithSuccess_GivenValidFasterPaymentRequest(MakePaymentRequest request,
        Account account)
    {
        //arrange
        account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;
        account.Balance += request.Amount;

        //act
        var result = _sut.Validate(request, account);

        //assert
        result.Should().BeTrue();
    }
    
    [Theory, AutoData]
    public void ShouldInvalidatedPaymentMethod_GivenAccountNotAllowedFasterPayments(MakePaymentRequest request,
        Account account)
    {
        //arrange
        account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;

        //act
        var result = _sut.Validate(request, account);

        //assert
        result.Should().BeFalse();
    }
    
    [Theory, AutoData]
    public void ShouldInvalidatedPaymentMethod_GivenNullAsAccount(MakePaymentRequest request)
    {
        //arrange
        //act
        var result = _sut.Validate(request, null);

        //assert
        result.Should().BeFalse();
    }
    
    [Theory, AutoData]
    public void ShouldInvalidatedPaymentMethod_WhenAccountBalanceLowerThanPaymentAmount(MakePaymentRequest request,
        Account account)
    {
        //arrange
        account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;
        account.Balance = request.Amount - 1;

        //act
        var result = _sut.Validate(request, account);

        //assert
        result.Should().BeFalse();
    }
}