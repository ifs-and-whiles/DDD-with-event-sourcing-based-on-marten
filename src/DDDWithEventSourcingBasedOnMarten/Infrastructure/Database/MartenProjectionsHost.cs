using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Baseline.Dates;
using Marten;
using Marten.Events.Projections.Async;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace DDDWithEventSourcingBasedOnMarten.Infrastructure.Database
{
     // ReSharper disable once ClassNeverInstantiated.Global
    public class MartenProjectionsHost : IHostedService
    {
        private static readonly ILogger Logger = Log.ForContext<MartenProjectionsHost>();

        private readonly IDocumentStore _store;
        private IDaemon _daemon;

        public MartenProjectionsHost(IDocumentStore store) => _store = store;

        public Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            Logger.Information("Projections starting");

            var settings = new DaemonSettings {LeadingEdgeBuffer = 0.Seconds()};

            _daemon = _store.BuildProjectionDaemon(
                logger: new SerilogDaemonLogger(),
                settings: settings
            );
            _daemon.StartAll();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            Logger.Information("Projections stopping");
            return _daemon?.StopAll();
        }

        public async Task WaitForNonStaleResults() => await _daemon.WaitForNonStaleResults();
        

        private class SerilogDaemonLogger : IDaemonLogger
        {
            private static readonly ILogger Logger =
                Log.ForContext(Constants.SourceContextPropertyName, "MartenAsyncDeamon");

            private IEnumerable<string> ToNames(IEnumerable<IProjectionTrack> projectionTracks) =>
                projectionTracks.Select(ToName);

            private string ToName(IProjectionTrack track) => track.ViewType.Name;

            private object ToPageSummary(EventPage page) => new {page.From, page.To, page.Count};

            public void BeginStartAll(IEnumerable<IProjectionTrack> values)
            {
                Logger.Information("Starting all projections: {@Projections}", ToNames(values));
            }

            public void DeterminedStartingPosition(IProjectionTrack track)
            {
                if (Logger.IsEnabled(LogEventLevel.Debug))
                    Logger.Debug("Determined starting position for projection {Projection} is {Position}",
                        ToName(track),
                        track.LastEncountered);
            }

            public void FinishedStartingAll()
            {
                Logger.Information("Finished starting projection daemon");
            }

            public void BeginRebuildAll(IEnumerable<IProjectionTrack> values)
            {
                Logger.Information("Beginning a rebuild of {@Projections}", ToNames(values));
            }

            public void FinishRebuildAll(TaskStatus status, AggregateException exception)
            {
                if (exception != null)
                    Logger.Error(exception, "Error while rebuilding projections");
                else
                    Logger.Information("Rebuilding of projections completed");
            }

            public void BeginStopAll()
            {
                Logger.Information("Beginning to stop projection daemon");
            }

            public void AllStopped()
            {
                Logger.Information("projection daemon stopped");
            }

            public void PausingFetching(IProjectionTrack track, long lastEncountered)
            {
                if (Logger.IsEnabled(LogEventLevel.Verbose))
                    Logger.Verbose("Pausing fetching for projection {Projection}, last encountered {LastEncountered}",
                        ToName(track), lastEncountered);
            }

            public void FetchStarted(IProjectionTrack track)
            {
                if (Logger.IsEnabled(LogEventLevel.Verbose))
                    Logger.Verbose("Fetching started for projection {Projection}", ToName(track));
            }

            public void FetchingIsAtEndOfEvents(IProjectionTrack track)
            {
                if (Logger.IsEnabled(LogEventLevel.Verbose))
                    Logger.Verbose("Fetching is at the end of events for projection {Projection}", ToName(track));
            }

            public void FetchingStopped(IProjectionTrack track)
            {
                if (Logger.IsEnabled(LogEventLevel.Verbose))
                    Logger.Verbose("Fetching stopped for projection {Projection}", ToName(track));
            }

            public void PageExecuted(EventPage page, IProjectionTrack track)
            {
                if (Logger.IsEnabled(LogEventLevel.Debug))
                    Logger.Debug("Page {Page} executed for projection {Projection}", ToPageSummary(page),
                        ToName(track));
            }

            public void FetchingFinished(IProjectionTrack track, long lastEncountered)
            {
                if (Logger.IsEnabled(LogEventLevel.Verbose))
                    Logger.Verbose(
                        "Fetching finished for projection {Projection} at last encountered {LastEncountered}",
                        ToName(track), lastEncountered);
            }

            public void StartingProjection(IProjectionTrack track, DaemonLifecycle lifecycle)
            {
                Logger.Information("Starting projection {Projection} with lifecycle {Lifecycle}", ToName(track),
                    lifecycle);
            }

            public void Stopping(IProjectionTrack track)
            {
                Logger.Information("Stopping projection {Projection}", ToName(track));
            }

            public void Stopped(IProjectionTrack track)
            {
                Logger.Information("Projection {Projection} stopped", ToName(track));
            }

            public void ProjectionBackedUp(IProjectionTrack track, int cachedEventCount, EventPage page)
            {
                if (Logger.IsEnabled(LogEventLevel.Debug))
                    Logger.Debug(
                        "Projection {Projection} is backed up with {CachedEventCount}, last page fetched {Page}",
                        ToName(track), cachedEventCount, ToPageSummary(page));
            }

            public void ClearingExistingState(IProjectionTrack track)
            {
                Logger.Information("Clearing existing state for projection {Projection}", ToName(track));
            }

            public void Error(Exception exception)
            {
                Logger.Error(exception, "Error in projection daemon");
            }
        }
    }
}