using System;
using Common.AI.Points;
using EventBus.Events;
using GenericEventBus;
using Helpers.Interfaces;
using Player;
using ServiceLocator;
using UnityEngine;
using Random = UnityEngine.Random;

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
        private GMSettings _settings;

        // sorry for hardcode
        private const float Fl3 = 12.39f;
        private const float Fl2 = 7.12f;
        private const float Fl1 = 1.83f;


        public Vector3 GetPlayerPoint()
        {
            Debug.Log("GetPlayerPoint");

            var level = GetClosestLevel(_playerSettings.playerTransform.position.y);

            var a = ReturnRandomPoint(level);

            return a;

            return _playerSettings.playerTransform.position;
        }

        private Vector3 ReturnRandomPoint(Room room)
        {
            var i = Random.Range(0, room.interestsPoints.Length);
            return room.interestsPoints[i].transform.position;
        }


        public void OnUpdateTrigger()
        {
            var y = _playerSettings.playerTransform.position.y;

            Floor = GetClosestLevel(y).Level;

            Debug.Log($"yyy - {Floor}");
        }

        public void Init()
        {
            _eventBus = SL.Current.Get<GenericEventBus<TBaseEvent>>();
            _playerSettings = SL.Current.Get<PlayerSettings>();
            _settings = SL.Current.Get<GMSettings>();

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

        private Room GetClosestLevel(float y)
        {
            var diff1 = Mathf.Abs(y - Fl1);
            var diff2 = Mathf.Abs(y - Fl2);
            var diff3 = Mathf.Abs(y - Fl3);

            if (diff1 < diff2 && diff1 < diff3)
            {
                return _settings.Level1;
            }

            if (diff2 < diff1 && diff2 < diff3)
            {
                return _settings.Level2;
            }

            return _settings.Level3;
        }
    }
}