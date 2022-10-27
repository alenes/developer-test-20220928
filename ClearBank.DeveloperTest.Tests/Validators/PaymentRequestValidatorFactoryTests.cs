using System;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Validators;

public class PaymentRequestValidatorFactoryTests
{
    private readonly PaymentRequestValidatorFactory _sut;
    
    public PaymentRequestValidatorFactoryTests()
    {
        _sut = new PaymentRequestValidatorFactory();
    }

    [Theory]
    [InlineData(PaymentScheme.Bacs, typeof(BacsPaymentRequestValidator))]
    [InlineData(PaymentScheme.Chaps, typeof(ChapsPaymentRequestValidator))]
    [InlineData(PaymentScheme.FasterPayments, typeof(FasterPaymentRequestValidator))]
    public void ShouldCreateValidator_GivenPaymentScheme(PaymentScheme scheme, Type factoryType)
    {
        //arrange
        //act
        var result = _sut.CreatePaymentRequestValidator(scheme);

        //assert
        result.Should().NotBeNull();
        result.Should().BeOfType(factoryType);
    }
    
    [Fact]
    public void ShouldThrowException_WhenCreateValidator_GivenNotSupportedPaymentScheme()
    {
        //arrange
        //act
        var action = () => _sut.CreatePaymentRequestValidator((PaymentScheme)4);

        //assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }
}