using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CubicBot.Telegram.Stats;

public sealed class StatsDispatch : IDispatch
{
    private readonly List<IStatsCollector> _collectors = [];

    public StatsDispatch(StatsConfig config)
    {
        if (config.EnableGrass)
        {
            var grass = new Grass();
            _collectors.Add(grass);
        }

        if (config.EnableMessageCounter)
        {
            var messageCounter = new MessageCounter();
            _collectors.Add(messageCounter);
        }

        if (config.EnableTwoTripleThree)
        {
            var twoTripleThree = new TwoTripleThree();
            _collectors.Add(twoTripleThree);
        }

        if (config.EnableParenthesisEnclosure)
        {
            var parenthesisEnclosure = new ParenthesisEnclosure();
            _collectors.Add(parenthesisEnclosure);
        }
    }

    public Task HandleAsync(MessageContext messageContext, CancellationToken cancellationToken = default)
    {
        var tasks = _collectors.Select(collector => collector.CollectAsync(messageContext, cancellationToken));
        return Task.WhenAll(tasks);
    }
}
