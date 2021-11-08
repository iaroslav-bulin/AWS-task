using Amazon.SQS;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace SqsLogger
{
    public class Logger : ILogger
    {
        private readonly string _name;
        private readonly LoggerConfig _config;
        private readonly SqsService _sqsService;

        public Logger(string name, LoggerConfig config, SqsService sqsService) 
        {
            _name = name;
            _config = config;
            _sqsService = sqsService;
        }

        public IDisposable BeginScope<TState>(TState state) => default;

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == _config.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            Console.WriteLine($"{logLevel} - {eventId.Id} - {_name} - {formatter(state, exception)}");
            // uncomment
            _sqsService.SendMessage($"{logLevel} - {eventId.Id} - {_name} - {formatter(state, exception)}").Wait();
        }
    }

    public class LoggerConfig
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Information;

    }

    public class LoggerProvider : ILoggerProvider
    {
        private bool disposedValue;
        private readonly LoggerConfig _config;
        private readonly ConcurrentDictionary<string, Logger> _loggers = new ConcurrentDictionary<string, Logger>();
        private readonly SqsService _sqsService;

        public LoggerProvider(LoggerConfig config, string queueUrl, IAmazonSQS sqsClient)
        {
            _config = config;
            _sqsService = new SqsService(queueUrl, sqsClient);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new Logger(name, _config, _sqsService));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _loggers.Clear();
                    _sqsService.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SqsLoggerProvider()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
