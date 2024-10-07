using System;
using System.Collections.Generic;
using Common.AI.Points;
using Common.EventBus.Events.InGame;
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

        private Vector3[] _points;

        public Vector3 GetPlayerPoint()
        {
            var r = Random.Range(1, 10);

            if (r == 1)
            {
                return _playerSettings.playerTransform.position;
            }

            var level = GetClosestLevel(_playerSettings.playerTransform.position.y);

            var a = ReturnRandomPoint(level);

            return a;
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

        private Vector3[] GeneratePoints()
        {
            var uniquePoints = new HashSet<Vector3>();

            foreach (var point in _settings.Level1.interestsPoints)
            {
                uniquePoints.Add(point.transform.position);
            }

            foreach (var point in _settings.Level2.interestsPoints)
            {
                uniquePoints.Add(point.transform.position);
            }

            foreach (var point in _settings.Level3.interestsPoints)
            {
                uniquePoints.Add(point.transform.position);
            }

            return new List<Vector3>(uniquePoints).ToArray();
        }

        private PointMover _pointMover;

        public void Init()
        {
            _eventBus = SL.Current.Get<GenericEventBus<TBaseEvent>>();
            _playerSettings = SL.Current.Get<PlayerSettings>();
            _settings = SL.Current.Get<GMSettings>();

            _points = GeneratePoints();
            _pointMover = new PointMover(_settings.target.transform, this);
            _pointMover.SetPoints(_points);

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


        public void OnEndReached()
        {
            _eventBus.Raise<WinGameEvent>(new());
        }
    }

    public class PointMover
    {
        [SerializeField] private Transform objectToMove;
        private Vector3[] points;
        private int currentPointIndex = 0;

        public PointMover(Transform objectToMove, GM gameManager)
        {
            this.objectToMove = objectToMove;
            this.gameManager = gameManager;
        }

        public void SetPoints(Vector3[] newPoints)
        {
            points = newPoints;
            currentPointIndex = 0;
        }


        public void MoveToNextPoint()
        {
            if (points != null && currentPointIndex < points.Length)
            {
                objectToMove.position = points[currentPointIndex];
                currentPointIndex++;


                if (currentPointIndex >= points.Length)
                {
                    OnEndReached();
                }
            }
        }

        GM gameManager;

        // Метод, который вызывается в конце пути
        private void OnEndReached()
        {
            gameManager.OnEndReached();
            Debug.Log("Объект прошел все точки!");
        }
    }
}