using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI.Routing
{
    public abstract class BaseActivator : IActivator
    {
        protected abstract object InstantiateObject(ActivationParams parameters);

        protected virtual async Task<object> ActivateCoreAsync(ActivationParams parameters)
        {
            var obj = InstantiateObject(parameters);
            var activatable = obj as IActivatable;
            if (activatable != null)
            {
                await activatable.InitAsync(parameters.Params);
            }
            return obj;
        }

        public async Task<object> ActivateAsync(ActivationParams parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            var result = await ActivateCoreAsync(parameters);
            if (result == null) throw new InvalidReturnValueException($"{nameof(ActivateCoreAsync)} must not return a null value. It must either return a new object or throw an exception.");
            return result;
        }

        public async Task DeactivateAsync(object activated)
        {
            if (activated == null) throw new ArgumentNullException(nameof(activated));
            await DeactivateCoreAsync(activated);
        }

        protected virtual async Task DeactivateCoreAsync(object activated)
        {
            var deactivatable = activated as IActivatable;
            if (deactivatable != null)
            {
                await deactivatable.DestroyAsync();
            }
        }
    }
}
