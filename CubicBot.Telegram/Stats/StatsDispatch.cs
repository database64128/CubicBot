using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CubicBot.Telegram.Stats
{
    public class StatsDispatch : IDispatch
    {
        private readonly Data _data;
        private readonly List<IStatsCollector> _collectors = new();

        public StatsDispatch(StatsConfig config, Data data)
        {
            _data = data;

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

            if (config.EnableParenthesisEnclosure)
            {
                var parenthesisEnclosure = new ParenthesisEnclosure();
                _collectors.Add(parenthesisEnclosure);
            }
        }

        public Task HandleAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken = default)
        {
            var tasks = _collectors.Select(collector => collector.CollectAsync(botClient, message, _data, cancellationToken));
            return Task.WhenAll(tasks);
        }
    }
}
