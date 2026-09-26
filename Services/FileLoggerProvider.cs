using Microsoft.Extensions.Logging;

namespace Routify.Services;

public class FileLoggerProvider : ILoggerProvider
{
    private readonly string _path;
    private readonly object _lock = new();

    public FileLoggerProvider(string path)
    {
        _path = path;
    }

    public ILogger CreateLogger(string categoryName) => new FileLogger(categoryName, _path, _lock);

    public void Dispose() { }

    private class FileLogger : ILogger
    {
        private readonly string _category;
        private readonly string _path;
        private readonly object _lock;

        public FileLogger(string category, string path, object lockObj)
        {
            _category = category;
            _path = path;
            _lock = lockObj;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var linea = $"{DateTime.Now:HH:mm:ss} [{logLevel}] {_category}: {formatter(state, exception)}";
            if (exception is not null)
            {
                linea += Environment.NewLine + exception;
            }

            lock (_lock)
            {
                File.AppendAllText(_path, linea + Environment.NewLine + Environment.NewLine);
            }
        }
    }
}