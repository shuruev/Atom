using System;
using Azure.Storage.Blobs;

namespace Atom.Azure
{
    /// <summary>
    /// Base class for connecting to Azure blob container.
    /// </summary>
    public abstract class AzureBlobContainer
    {
        protected readonly Lazy<BlobContainerClient> _client;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected AzureBlobContainer(AzureSettings settings, string containerName, bool createIfNotExists)
        {
            _client = new Lazy<BlobContainerClient>(() =>
            {
                var client = AzureClient.Create(
                    settings,
                    conn => new BlobContainerClient(conn, containerName),
                    (acc, cred) => new BlobContainerClient(new Uri($"https://{acc}.blob.core.windows.net/{containerName}"), cred),
                    () => throw new InvalidOperationException("Cannot create Azure blob container client without storage account name"));

                if (createIfNotExists)
                    client.CreateIfNotExists();

                return client;
            });
        }
    }
}
