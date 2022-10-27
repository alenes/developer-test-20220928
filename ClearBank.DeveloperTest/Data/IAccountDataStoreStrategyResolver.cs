namespace ClearBank.DeveloperTest.Data;

public interface IAccountDataStoreStrategyResolver
{
    public IAccountDataStore Resolve();
}