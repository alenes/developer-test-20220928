using AutoFixture.Xunit2;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Validators;

public class ChapsPaymentRequestValidatorTests
{
    private readonly ChapsPaymentRequestValidator _sut;
    
    public ChapsPaymentRequestValidatorTests()
    {
        _sut = new ChapsPaymentRequestValidator();
    }

    [Theory, AutoData]
    public void ShouldValidatePaymentMethodWithSuccess_GivenValidChapsPaymentRequest(MakePaymentRequest request,
        Account account)
    {
        //arrange
        account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;
        account.Status = AccountStatus.Live;

        //act
        var result = _sut.Validate(request, account);

        //assert
        result.Should().BeTrue();
    }
    
    [Theory, AutoData]
    public void ShouldInvalidatedPaymentMethod_GivenAccountNotAllowedChapsPayments(MakePaymentRequest request,
        Account account)
    {
        //arrange
        account.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs;

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
    public void ShouldInvalidatedPaymentMethod_WhenAccountStatusIsNotLive(MakePaymentRequest request,
        Account account)
    {
        //arrange
        account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;
        account.Status = AccountStatus.Disabled;
        //act
        var result = _sut.Validate(request, account);

        //assert
        result.Should().BeFalse();
    }
}