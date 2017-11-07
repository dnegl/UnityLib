using DukeLabs.Core.UI.Component.Interface;
using Zenject;

namespace DukeLabs.Core.UI.Component
{
    public class UpdatableComponent : UIComponent, IUpdatable
    {
        [Inject] private UpdateManager _updateManager;

        public virtual bool IsFixed
        {
            get { return false; }
        }

        public virtual bool IsLate
        {
            get { return false; }
        }

        public virtual void Awake()
        {
            _updateManager.AddComponent(this);
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnFixedUpdate()
        {
        }

        public virtual void OnLateUpdate()
        {
        }

        public virtual void OnDestroy()
        {
            _updateManager.RemoveComponent(this);
        }
    }
}