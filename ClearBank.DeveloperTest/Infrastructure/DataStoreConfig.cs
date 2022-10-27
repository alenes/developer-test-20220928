using System.Configuration;

namespace ClearBank.DeveloperTest.Infrastructure;

public class DataStoreConfig : IDataStoreConfig
{
    public string DataStoreType => ConfigurationManager.AppSettings["DataStoreType"];
}