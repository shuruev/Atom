using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atom.Util;
using Azure.Storage.Queues;

namespace Atom.Azure
{
    /// <summary>
    /// Base class for connecting to Azure queue.
    /// </summary>
    public abstract class AzureQueue
    {
        private const int MAX_PARALLEL_TASKS = 25;

        protected readonly Lazy<QueueClient> _client;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected AzureQueue(string connectionString, string queueName, bool createIfNotExists)
        {
            _client = new Lazy<QueueClient>(() =>
            {
                var client = new QueueClient(connectionString, queueName);

                if (createIfNotExists)
                    client.CreateIfNotExists();

                return client;
            });
        }

        /// <summary>
        /// Sends a single message with optional postpone delay.
        /// </summary>
        protected async Task SendMessageAsync(string message, TimeSpan? delay = null)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            var base64 = Convert.ToBase64String(bytes);
            await _client.Value.SendMessageAsync(base64, delay);
        }

        /// <summary>
        /// Sends several messages with optional postpone delay.
        /// Unlike native AWS client, this does not have any limitation on how many messages are being sent.
        /// </summary>
        protected async Task SendMessagesAsync(IEnumerable<string> messages, TimeSpan? delay = null)
            => await SendMessagesAsync(messages.Select(msg => (msg, delay)));

        /// <summary>
        /// Sends several messages with optional postpone delay, which might be different for every message.
        /// Unlike native Azure client, this does not have any limitation on how many messages are being sent.
        /// </summary>
        protected async Task SendMessagesAsync(IEnumerable<(string message, TimeSpan? delay)> messages)
        {
            await AsyncUtil.ForEachAsync(
                messages,
                MAX_PARALLEL_TASKS,
                item => SendMessageAsync(item.message, item.delay));
        }

        /// <summary>
        /// Returns the approximate number of messages available for retrieval from the queue.
        /// </summary>
        protected async Task<int> GetApproximateMessagesCountAsync()
        {
            var response = await _client.Value.GetPropertiesAsync();
            var props = response.Value;
            return props.ApproximateMessagesCount;
        }
    }
}
