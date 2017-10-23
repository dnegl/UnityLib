using System.Collections.Generic;
using DukeLabs.Core.UI.Component.Interface;
using UnityEngine;

namespace DukeLabs.Core.UI
{
    public sealed class UpdateManager : UnitySingleton<UpdateManager>
    {
        private readonly List<IUpdatable> _updatableComponents = new List<IUpdatable>();
        private readonly List<IUpdatable> _fixedUpdatableComponents = new List<IUpdatable>();
        private readonly List<IUpdatable> _lateUpdatableComponents = new List<IUpdatable>();

        public void AddComponent(IUpdatable updatableComponent)
        {
            _updatableComponents.Add(updatableComponent);
            if(updatableComponent.IsFixed)
                _fixedUpdatableComponents.Add(updatableComponent);
            if(updatableComponent.IsLate)
                _lateUpdatableComponents.Add(updatableComponent);
        }

        public void RemoveComponent(IUpdatable updatableComponent)
        {
            _updatableComponents.Remove(updatableComponent);
            _fixedUpdatableComponents.Remove(updatableComponent);
            _lateUpdatableComponents.Remove(updatableComponent);
        }

        private void Update()
        {
            for (int i = 0; i < _updatableComponents.Count; i++)
            {
                if (_updatableComponents[i] != null)
                {
                    _updatableComponents[i].OnUpdate();
                }
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _fixedUpdatableComponents.Count; i++)
            {
                if (_fixedUpdatableComponents[i] != null)
                {
                    _fixedUpdatableComponents[i].OnFixedUpdate();
                }
            }
        }
        
        private void LateUpdate()
        {
            for (int i = 0; i < _lateUpdatableComponents.Count; i++)
            {
                if (_lateUpdatableComponents[i] != null)
                {
                    _lateUpdatableComponents[i].OnLateUpdate();
                }
            }
        }
        
    }
}