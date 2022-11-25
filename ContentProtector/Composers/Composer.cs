using ContentProtector.Components;
using ContentProtector.Controllers;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace ContentProtector.Composers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class Composer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<InitializePlan>();
            composition.Components().Append<ActionBlocker>();
            composition.Register<ContentProtectorApiController>();
        }
    }
}