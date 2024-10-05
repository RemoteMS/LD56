using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Helpers.Interfaces;
using UnityEngine;

namespace ServiceLocator.Reflection
{
    public class ServicesControllerBase : MonoBehaviour, IDisposable
    {
        #region Interfaces Invoking Lists
        private List<IGameUpdatable> _gameUpdatables = new List<IGameUpdatable>();
        private List<IGameFixedUpdatable> _gameFixedUpdatables = new List<IGameFixedUpdatable>();
        private List<IGameLateUpdatable> _gameLateUpdatables = new List<IGameLateUpdatable>();
        private List<IBeforeDisposable> _beforeDisposables = new List<IBeforeDisposable>();
        private List<IDisposable> _disposables = new List<IDisposable>();
        #endregion


        protected virtual void RegisterToCollection(object instance)
        {
            GryAttributes(instance);
        }

        private void GryAttributes(object instance)
        {
            var fields = instance
                .GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(field => field.GetCustomAttribute<RegisterFieldAttribute>() != null);

            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<RegisterFieldAttribute>();
                var value = field.GetValue(instance);

                foreach (var interfaceType in attribute.InterfaceTypes)
                {
                    AddToCollection(interfaceType, value);
                }
            }

#if UNITY_EDITOR
            Debug.Log($"_disposables.Count - {_disposables.Count}");
            Debug.Log($"_gameUpdatables.Count - {_gameUpdatables.Count}");
            Debug.Log($"_gameFixedUpdatables.Count - {_gameFixedUpdatables.Count}");
            Debug.Log($"_gameLateUpdatables.Count - {_gameLateUpdatables.Count}");
#endif
        }

        private void AddToCollection(Type interfaceType, object value)
        {
            if (value == null)
            {
                Debug.LogWarning($"Field with {interfaceType} Type is null.");
                return;
            }

            Type valueType = value.GetType();

            if (interfaceType.IsAssignableFrom(valueType))
            {
                if (interfaceType == typeof(IDisposable))
                {
                    _disposables.Add((IDisposable)value);
                }
                else if (interfaceType == typeof(IGameUpdatable))
                {
                    _gameUpdatables.Add((IGameUpdatable)value);
                }
                else if (interfaceType == typeof(IGameFixedUpdatable))
                {
                    _gameFixedUpdatables.Add((IGameFixedUpdatable)value);
                }
                else if (interfaceType == typeof(IGameLateUpdatable))
                {
                    _gameLateUpdatables.Add((IGameLateUpdatable)value);
                }
            }
            else
            {
                Debug.LogWarning(
                    $"Object of type {valueType.FullName} doesn't inherit {interfaceType.FullName}"
                );
            }
        }

        private void Update()
        {
            foreach (var updatable in _gameUpdatables)
            {
                updatable.OnUpdateTrigger();
            }
        }

        public void LateUpdate()
        {
            foreach (var updatable in _gameLateUpdatables)
            {
                updatable.OnLateUpdateTrigger();
            }
        }

        public void FixedUpdate()
        {
            foreach (var updatable in _gameFixedUpdatables)
            {
                updatable.OnFixedUpdateTrigger();
            }
        }

        public void BeforeDispose()
        {
            foreach (var beforeDisposable in _beforeDisposables)
            {
                beforeDisposable.BeforeDispose();
            }
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable?.Dispose();
            }
        }

        private void OnDestroy()
        {
            BeforeDispose();
            Dispose();
        }
    }
}
