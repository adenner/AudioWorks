﻿using System;
using System.Collections.Concurrent;
using System.Management.Automation;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;

namespace AudioWorks.Commands
{
    sealed class CmdletLogger : ILogger
    {
        [CanBeNull]
        internal BlockingCollection<object> MessageQueue { get; set; }

        public void Log<TState>(LogLevel logLevel, EventId eventId, [CanBeNull] TState state, [CanBeNull] Exception exception, [NotNull] Func<TState, Exception, string> formatter)
        {
            if (MessageQueue == null) return;

            var message = formatter(state, exception);

            switch (logLevel)
            {
                case LogLevel.Debug:
                    MessageQueue.Add(new DebugRecord(message));
                    break;
                case LogLevel.Information:
                    MessageQueue.Add(new InformationRecord(message, null));
                    break;
                case LogLevel.Warning:
                    MessageQueue.Add(new WarningRecord(message));
                    break;
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                case LogLevel.Information:
                case LogLevel.Warning:
                    return true;
                default:
                    return false;
            }
        }

        [NotNull]
        public IDisposable BeginScope<TState>([CanBeNull] TState state) => NullScope.Instance;
    }
}