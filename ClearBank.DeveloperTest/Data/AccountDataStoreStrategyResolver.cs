using System;
using ClearBank.DeveloperTest.Infrastructure;

namespace ClearBank.DeveloperTest.Data;

public class AccountDataStoreStrategyResolver: IAccountDataStoreStrategyResolver
{
    private readonly IDataStoreConfig _dataStoreConfig;

    public AccountDataStoreStrategyResolver(IDataStoreConfig dataStoreConfig)
    {
        _dataStoreConfig = dataStoreConfig ?? throw new ArgumentNullException(nameof(dataStoreConfig));
    }
    
    public IAccountDataStore Resolve() => _dataStoreConfig.DataStoreType == "Backup"
        ? new BackupAccountDataStore()
        : new AccountDataStore();
}