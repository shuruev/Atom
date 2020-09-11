using System;

namespace Atom.Azure
{
    /// <summary>
    /// Describe one of the possible ways to connect to Azure:
    /// 1. Use storage connection string
    /// 2. Use storage account name and key
    /// 3. Use storage account and default credentials
    /// 4. Use storage account and specific managed identity (not implemented yet)
    /// </summary>
    /// <remarks>
    /// The primary use-case is to have these settings stored in your config, when developing Azure-based apps.
    /// Then when running locally, or in Azure without using identities these config values will be used.
    /// When running in Azure with managed identities, settings could be changed (e.g. to start using default credentials instead).
    /// </remarks>
    public class AzureSettings
    {
        /// <summary>
        /// Connection string for accessing Azure storage.
        /// </summary>
        public string StorageConnectionString { get; set; }

        /// <summary>
        /// Azure storage account name.
        /// </summary>
        public string StorageAccountName { get; set; }

        /// <summary>
        /// Azure storage account key.
        /// </summary>
        public string StorageAccountKey { get; set; }
    }

    /// <summary>
    /// Helper factory for creating desired Azure client based on specified settings.
    /// </summary>
    public static class AzureClient
    {
        /// <summary>
        /// Helps creating Azure client based on specified settings.
        /// Provide Azure constructors for each case, and have specific client initialized based on which
        /// settings components are provided.
        /// </summary>
        /// <example><![CDATA[
        /// public class MyQueue
        /// {
        ///     private readonly Lazy<QueueClient> _client;
        ///
        ///     public MyQueue(AzureSettings settings, string queueName, bool createIfNotExists)
        ///     {
        ///         _client = new Lazy<QueueClient>(() =>
        ///         {
        ///             var client = AzureClient.Create(
        ///                 settings,
        ///                 conn => new QueueClient(conn, queueName),
        ///                 acc => new QueueClient(new Uri($"https://{acc}.queue.core.windows.net/{queueName}"), new DefaultAzureCredential()),
        ///                 () => throw new InvalidOperationException("Cannot create Azure queue client without storage account name"));
        ///
        ///             if (createIfNotExists)
        ///                 client.CreateIfNotExists();
        ///
        ///             return client;
        ///         });
        /// }
        /// ]]></example>
        public static T Create<T>(
            AzureSettings settings,
            Func<string, T> createByStorageConnectionString,
            Func<string, T> createWithDefaultCredentials,
            Func<T> createByDefault)
        {
            var hasStorageConnectionString = !String.IsNullOrWhiteSpace(settings?.StorageConnectionString);
            var hasStorageAccountName = !String.IsNullOrWhiteSpace(settings?.StorageAccountName);
            var hasStorageAccountKey = !String.IsNullOrWhiteSpace(settings?.StorageAccountKey);

            if (hasStorageConnectionString)
                return createByStorageConnectionString(settings.StorageConnectionString);

            if (hasStorageAccountName && hasStorageAccountKey)
                return createByStorageConnectionString($"DefaultEndpointsProtocol=https;AccountName={settings.StorageAccountName};AccountKey={settings.StorageAccountKey};EndpointSuffix=core.windows.net");

            if (hasStorageAccountName)
                return createWithDefaultCredentials(settings.StorageAccountName);

            return createByDefault();
        }
    }
}
