using System;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Infrastructure;
using FluentAssertions;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Data;

public class AccountDataStoreStrategyResolverTests
{
    private readonly AccountDataStoreStrategyResolver _sut;
    private readonly Mock<IDataStoreConfig> _dataStoreConfig;

    public AccountDataStoreStrategyResolverTests()
    {
        _dataStoreConfig = new Mock<IDataStoreConfig>();
        _sut = new AccountDataStoreStrategyResolver(_dataStoreConfig.Object);
    }

    [Theory]
    [InlineData("Backup", typeof(BackupAccountDataStore))]
    [InlineData("Account", typeof(AccountDataStore))]
    public void ShouldResolveDataStoreStrategy_GivenStorageConfig(string storageOption, Type dataStoreType)
    {
        //arrange
        _dataStoreConfig.SetupGet(c => c.DataStoreType).Returns(storageOption);
        
        //act
        var result = _sut.Resolve();
        
        //assert
        result.Should().NotBeNull();
        result.Should().BeOfType(dataStoreType);
    }
}