    using Assets.Sources.Game.ECS.Features;
using Entitas.VisualDebugging.Unity;
using UnityEngine;

namespace DukeLabs.Core
{
	//Start point for the project 
    public class StartPoint : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private Transform _scoreParent;
        private Entitas.Systems _systems;

        void Awake()
        {
            var contexts = Contexts.sharedInstance;
            _systems = new GameFeature(contexts, _parent, _scoreParent);
        }

        void Start()
        {
            _systems.Initialize();
        }

        void Update()
        {
            _systems.Execute();
            _systems.Cleanup();
        }

        void OnDestroy()
        {
            _systems.TearDown();
            _systems.DeactivateReactiveSystems();
            Contexts.sharedInstance.Reset();
            Contexts.sharedInstance = null;
        }
    }
}