using AutoFixture.Xunit2;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using FluentAssertions;
using Xunit;

// ReSharper disable IdentifierTypo

namespace ClearBank.DeveloperTest.Tests.Validators;

public class BacsPaymentRequestValidatorTests
{
    private readonly BacsPaymentRequestValidator _sut;
    
    public BacsPaymentRequestValidatorTests()
    {
        _sut = new BacsPaymentRequestValidator();
    }

    [Theory, AutoData]
    public void ShouldValidatePaymentMethodWithSuccess_GivenAccountWithAllowedBacsPayments(MakePaymentRequest request,
        Account account)
    {
        //arrange
        account.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs;

        //act
        var result = _sut.Validate(request, account);

        //assert
        result.Should().BeTrue();
    }
    
    [Theory, AutoData]
    public void ShouldInvalidatedPaymentMethod_GivenAccountNotAllowedBacsPayments(MakePaymentRequest request,
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
}