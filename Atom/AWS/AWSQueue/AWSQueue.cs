using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Atom.Util;

namespace Atom.AWS
{
    /// <summary>
    /// Base class for connecting to AWS queue.
    /// </summary>
    public abstract class AWSQueue
    {
        private const int MAX_SENDING_BATCH = 10;
        private const int MAX_PARALLEL_TASKS = 25;

        protected readonly AmazonSQSClient _client;
        protected readonly Lazy<string> _queueUrl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected AWSQueue(AWSSettings settings, string queueName)
        {
            _client = AWSClient.Create(
                settings,
                (creds, reg) => new AmazonSQSClient(creds, reg),
                creds => new AmazonSQSClient(creds),
                reg => new AmazonSQSClient(reg),
                () => new AmazonSQSClient());

            _queueUrl = new Lazy<string>(() => _client.GetQueueUrlAsync(queueName)
                .ConfigureAwait(false).GetAwaiter().GetResult().QueueUrl);
        }

        /// <summary>
        /// Sends a single message with optional postpone delay.
        /// </summary>
        protected async Task SendMessageAsync(string message, TimeSpan? delay = null)
        {
            var request = new SendMessageRequest(_queueUrl.Value, message);
            if (delay.HasValue)
                request.DelaySeconds = Convert.ToInt32(delay.Value.TotalSeconds);

            await _client.SendMessageAsync(request);
        }

        /// <summary>
        /// Sends several messages with optional postpone delay.
        /// Unlike native AWS client, this does not have any limitation on how many messages are being sent.
        /// </summary>
        protected async Task SendMessagesAsync(IEnumerable<string> messages, TimeSpan? delay = null)
            => await SendMessagesAsync(messages.Select(msg => (msg, delay)));

        /// <summary>
        /// Sends several messages with optional postpone delay, which might be different for every message.
        /// Unlike native AWS client, this does not have any limitation on how many messages are being sent.
        /// </summary>
        protected async Task SendMessagesAsync(IEnumerable<(string message, TimeSpan? delay)> messages)
        {
            await AsyncUtil.ForEachAsync(
                messages.Batch(MAX_SENDING_BATCH),
                MAX_PARALLEL_TASKS,
                batch => _client.SendMessageBatchAsync(
                    _queueUrl.Value,
                    batch.Select(item =>
                    {
                        var entry = new SendMessageBatchRequestEntry(Guid.NewGuid().ToString(), item.message);
                        if (item.delay.HasValue)
                            entry.DelaySeconds = Convert.ToInt32(item.delay.Value.TotalSeconds);

                        return entry;
                    })
                    .ToList()));
        }

        /// <summary>
        /// Returns the approximate number of messages available for retrieval from the queue.
        /// </summary>
        protected async Task<int> GetApproximateNumberOfMessagesAsync()
        {
            var props = await _client.GetQueueAttributesAsync(_queueUrl.Value, new List<string> { "ApproximateNumberOfMessages" });
            return props.ApproximateNumberOfMessages;
        }
    }
}
