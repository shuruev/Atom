using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Atom.Util
{
    /// <summary>
    /// Allows to run an external process getting output/error contents properly and without deadlocks.
    /// </summary>
    public static class RunProcess
    {
        /// <summary>
        /// Runs external process and returns result object.
        /// </summary>
        public static RunProcessResult Run(
            string fileName,
            string arguments = null,
            string workingDirectory = null,
            Action<string> handleOutputBlock = null,
            Action<string> handleErrorBlock = null)
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    WorkingDirectory = workingDirectory,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                var output = new StringBuilder();
                var error = new StringBuilder();
                process.Start();

                Task.WaitAll(
                    Task.Run(() =>
                    {
                        while (true)
                        {
                            var symbol = process.StandardOutput.Read();
                            if (symbol < 0)
                                break;

                            var block = ReadAvailableBlock(process.StandardOutput, symbol);
                            output.Append(block);
                            handleOutputBlock?.Invoke(block);
                        }
                    }),
                    Task.Run(() =>
                    {
                        while (true)
                        {
                            var symbol = process.StandardError.Read();
                            if (symbol < 0)
                                break;

                            var block = ReadAvailableBlock(process.StandardError, symbol);
                            error.Append(block);
                            handleErrorBlock?.Invoke(block);
                        }
                    }));

                process.WaitForExit();
                return new RunProcessResult(process, output, error);
            }
        }

        private static string ReadAvailableBlock(TextReader reader, int first)
        {
            var sb = new StringBuilder();
            sb.Append((char)first);

            while (reader.Peek() >= 0)
            {
                var c = reader.Read();
                sb.Append((char)c);
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// Represents the result of external process run.
    /// </summary>
    public class RunProcessResult
    {
        /// <summary>
        /// Process file name.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Process execution arguments.
        /// </summary>
        public string Arguments { get; }

        /// <summary>
        /// Process working directory.
        /// </summary>
        public string WorkingDirectory { get; }

        /// <summary>
        /// Local time when the process execution started.
        /// </summary>
        public DateTime StartTime { get; }

        /// <summary>
        /// Local time when the process execution ended.
        /// </summary>
        public DateTime EndTime { get; }

        /// <summary>
        /// Total execution duration.
        /// </summary>
        public TimeSpan RunDuration { get; }

        /// <summary>
        /// Process exit code.
        /// </summary>
        public int ExitCode { get; }

        /// <summary>
        /// Gets full standard output received during process execution.
        /// </summary>
        public string StandardOutput { get; }

        /// <summary>
        /// Gets full standard error received during process execution.
        /// </summary>
        public string StandardError { get; }

        internal RunProcessResult(Process process, StringBuilder output, StringBuilder error)
        {
            FileName = process.StartInfo.FileName;
            Arguments = process.StartInfo.Arguments;
            WorkingDirectory = process.StartInfo.WorkingDirectory;
            StartTime = process.StartTime;
            EndTime = process.ExitTime;
            RunDuration = EndTime.Subtract(StartTime);
            ExitCode = process.ExitCode;
            StandardOutput = output.ToString();
            StandardError = error.ToString();
        }
    }
}
