﻿using System;
using System.Diagnostics.Tracing;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Sinks;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging
{
    /// <summary>
    /// Factories and helpers for using the <see cref="ElasticSearchSink"/>.
    /// </summary>
    public static class ElasticSearchLog
    {
        public static SinkSubscription<ElasticSearchSink> LogToElasticSearch(this IObservable<EventEntry> eventStream,
            string instanceName, string hostName, int portNumber = 9200, TimeSpan? bufferingInterval = null,
            TimeSpan? onCompletedTimeout = null,
            int maxBufferSize = Buffering.DefaultMaxBufferSize)
        {
            var sink = new ElasticSearchSink(instanceName, hostName, portNumber,
                bufferingInterval ?? Buffering.DefaultBufferingInterval,
                maxBufferSize,
                onCompletedTimeout ?? Timeout.InfiniteTimeSpan);

            var subscription = eventStream.SubscribeWithConversion(sink);
            return new SinkSubscription<ElasticSearchSink>(subscription, sink);
        }

        public static EventListener CreateListener(string instanceName, string hostName, int portNumber = 9200,
            TimeSpan? bufferingInterval = null, TimeSpan? listenerDisposeTimeout = null, int maxBufferSize = Buffering.DefaultMaxBufferSize)
        {
            var listener = new ObservableEventListener();
            listener.LogToElasticSearch(instanceName, hostName, portNumber, bufferingInterval,
                listenerDisposeTimeout, maxBufferSize);
            return listener;
        }
    }
}