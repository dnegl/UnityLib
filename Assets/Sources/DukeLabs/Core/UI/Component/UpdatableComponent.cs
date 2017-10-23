using DukeLabs.Core.UI.Component.Interface;
using UnityEngine;

namespace DukeLabs.Core.UI.Component
{
    public class UpdatableComponent : UIComponent, IUpdatable
    {
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
            UpdateManager.Instance.AddComponent(this);
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
            if(UpdateManager.IsInitialized)
                UpdateManager.Instance.RemoveComponent(this);
        }
    }
}