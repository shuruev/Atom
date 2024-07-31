using System;
using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;

namespace Atom.AWS
{
    /// <summary>
    /// Describe one of the possible ways to connect to AWS:
    /// 1. Have Key and Secret populated (Region can optionally be populated as well)
    /// 2. Specify UseProfile to use local profile (such as ".aws" folder)
    /// 3. Will try to partially use credentials or region from both directly specified and profile
    /// 4. Will use default settings if nothing was directly specified
    /// </summary>
    /// <remarks>
    /// The primary use-case is to have these settings stored in your config, when developing AWS-based apps.
    /// Then when running locally, or outside AWS these config values will be used.
    /// When running in AWS, remove this config section (or leave the values empty) so the default AWS client would be used.
    /// </remarks>
    public class AWSSettings
    {
        /// <summary>
        /// AWS access key ID.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// AWS secret key value.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// AWS region code.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Name of local profile to use, when building AWS client.
        /// </summary>
        public string UseProfile { get; set; }
    }

    /// <summary>
    /// Helper factory for creating desired AWS client based on specified settings.
    /// </summary>
    public static class AWSClient
    {
        /// <summary>
        /// Helps creating AWS client based on specified settings.
        /// Provide AWS constructors for each case, and have specific client initialized based on which
        /// settings components are provided.
        /// </summary>
        /// <example><![CDATA[
        /// public class MyQueue
        /// {
        ///     private readonly AmazonSQSClient _client;
        ///     private readonly Lazy<string> _queueUrl;
        ///
        ///     public MyQueue(AWSSettings settings, string queueName)
        ///     {
        ///         _client = AWSClient.Create(
        ///             settings,
        ///             (creds, reg) => new AmazonSQSClient(creds, reg),
        ///             creds => new AmazonSQSClient(creds),
        ///             reg => new AmazonSQSClient(reg),
        ///             () => new AmazonSQSClient());
        ///
        ///         _queueUrl = new Lazy<string>(() => _client.GetQueueUrlAsync(queueName)
        ///             .ConfigureAwait(false).GetAwaiter().GetResult().QueueUrl);
        ///     }
        /// }
        /// ]]></example>
        public static T Create<T>(
            AWSSettings settings,
            Func<AWSCredentials, RegionEndpoint, T> createByCredentialsAndRegion,
            Func<AWSCredentials, T> createByCredentials,
            Func<RegionEndpoint, T> createByRegion,
            Func<T> createByDefault)
        {
            SharedCredentialsFile file = null;
            CredentialProfile profile = null;
            AWSCredentials credentials = null;
            RegionEndpoint region = null;

            var hasKey = !String.IsNullOrWhiteSpace(settings?.Key);
            var hasSecret = !String.IsNullOrWhiteSpace(settings?.Secret);
            var hasRegion = !String.IsNullOrWhiteSpace(settings?.Region);
            var hasProfile = !String.IsNullOrWhiteSpace(settings?.UseProfile);

            if (hasProfile)
            {
                file = new SharedCredentialsFile();
                if (!file.TryGetProfile(settings.UseProfile, out profile))
                    throw new InvalidOperationException($"Cannot read AWS profile '{settings.UseProfile}'");
            }

            if (hasKey && hasSecret)
            {
                credentials = new BasicAWSCredentials(settings.Key, settings.Secret);
            }
            else if (hasProfile)
            {
                if (!AWSCredentialsFactory.TryGetAWSCredentials(profile, file, out credentials))
                    throw new InvalidOperationException($"Cannot get AWS credentials from profile '{profile.Name}'");
            }

            if (hasRegion)
            {
                region = RegionEndpoint.GetBySystemName(settings.Region);
            }
            else if (hasProfile)
            {
                region = profile.Region;
            }

            if (credentials != null && region != null)
                return createByCredentialsAndRegion(credentials, region);

            if (credentials != null)
                return createByCredentials(credentials);

            if (region != null)
                return createByRegion(region);

            return createByDefault();
        }
    }
}
