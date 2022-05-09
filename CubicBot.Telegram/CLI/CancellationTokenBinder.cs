using System.CommandLine.Binding;
using System.Threading;

namespace CubicBot.Telegram.CLI;

internal class CancellationTokenBinder : BinderBase<CancellationToken>
{
    protected override CancellationToken GetBoundValue(BindingContext bindingContext) =>
        (CancellationToken)bindingContext.GetService(typeof(CancellationToken))!;
}
