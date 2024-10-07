using System;
using EventBus.Events;
using GenericEventBus;
using Helpers.Interfaces;
using Player;
using ServiceLocator;
using UnityEngine;

namespace Common.AI.GameMaster
{
    public class GM : IService, IInitializable, IDisposable, IGameEventListener, IGameUpdatable
    {
        public int Floor { get; private set; }

        public GM()
        {
        }

        private GenericEventBus<TBaseEvent> _eventBus;
        private PlayerSettings _playerSettings;


        // sorry for hardcode
        private const float Fl3 = 12.39f;
        private const float Fl2 = 7.12f;
        private const float Fl1 = 1.83f;


        public Vector3 GetPlayerPoint()
        {
            return _playerSettings.playerTransform.position;
        }

        public void OnUpdateTrigger()
        {
            var y = _playerSettings.playerTransform.position.y;

            Floor = GetClosest(y);

            Debug.Log($"yyy - {Floor}");
        }

        public void Init()
        {
            _eventBus = SL.Current.Get<GenericEventBus<TBaseEvent>>();
            _playerSettings = SL.Current.Get<PlayerSettings>();

            SubscribeToEvents();
        }

        public void SubscribeToEvents()
        {
        }

        public void UnsubscribeFromEvents()
        {
        }

        public void Dispose()
        {
            UnsubscribeFromEvents();
        }

        private static int GetClosest(float y)
        {
            var diff1 = Mathf.Abs(y - Fl1);
            var diff2 = Mathf.Abs(y - Fl2);
            var diff3 = Mathf.Abs(y - Fl3);

            if (diff1 < diff2 && diff1 < diff3)
            {
                return 1;
            }

            if (diff2 < diff1 && diff2 < diff3)
            {
                return 2;
            }

            return 3;
        }
    }
}