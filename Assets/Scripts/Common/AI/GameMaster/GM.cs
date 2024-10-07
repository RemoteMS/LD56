using System;
using System.Collections.Generic;
using Common.AI.Points;
using Common.EventBus.Events.InGame;
using EventBus.Events;
using EventBus.Events.InGame;
using GenericEventBus;
using Helpers.Interfaces;
using Player;
using ServiceLocator;
using UnityEngine;
using IInitializable = Helpers.Interfaces.IInitializable;
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
            var points = new Vector3[8];

            var to = _settings.Level1.interestsPoints.Length - 1;

            Debug.Log($"test to - {to} m points - {_settings.Level1.interestsPoints.Length}");
            points[0] = _settings.Level1.interestsPoints[Random.Range(0, to)].transform.position;
            points[1] = _settings.Level2.interestsPoints[Random.Range(0, to)].transform.position;
            points[2] = _settings.Level1.interestsPoints[Random.Range(0, to)].transform.position;
            points[3] = _settings.Level3.interestsPoints[Random.Range(0, to)].transform.position;
            points[4] = _settings.Level1.interestsPoints[Random.Range(0, to)].transform.position;
            points[5] = _settings.Level3.interestsPoints[Random.Range(0, to)].transform.position;
            points[6] = _settings.Level2.interestsPoints[Random.Range(0, to)].transform.position;
            points[7] = _settings.Level1.interestsPoints[Random.Range(0, to)].transform.position;

            return points;
        }

        private PointMover _pointMover;

        public void Init()
        {
            _eventBus = SL.Current.Get<GenericEventBus<TBaseEvent>>();
            _playerSettings = SL.Current.Get<PlayerSettings>();
            _settings = SL.Current.Get<GMSettings>();

            _points = GeneratePoints();
            _pointMover = new PointMover(_settings.target, this);
            _pointMover.SetPoints(_points);

            SubscribeToEvents();
        }

        public void SubscribeToEvents()
        {
            _eventBus.SubscribeTo<TryGetEvent>(HandleInteractEvent);
            _eventBus.SubscribeTo<RaycastStarted>(HandleRaycastStartedEvent);
            _eventBus.SubscribeTo<RaycastEnded>(HandleRaycastEndedEvent);
        }


        public void UnsubscribeFromEvents()
        {
            _eventBus.UnsubscribeFrom<TryGetEvent>(HandleInteractEvent);
            _eventBus.UnsubscribeFrom<RaycastStarted>(HandleRaycastStartedEvent);
            _eventBus.UnsubscribeFrom<RaycastEnded>(HandleRaycastEndedEvent);
        }


        public void Dispose()
        {
            UnsubscribeFromEvents();
        }


        //todo: warning
        private bool CatTake = false;

        private void HandleRaycastEndedEvent(ref RaycastEnded eventdata)
        {
            CatTake = false;
        }

        private void HandleRaycastStartedEvent(ref RaycastStarted eventdata)
        {
            CatTake = true;
        }


        private void HandleInteractEvent(ref TryGetEvent eventdata)
        {
            Debug.Log($"HandleInteractEvent - {CatTake}");
            if (CatTake)
            {
                // cj,hfnm mov

                var takenIndex = _pointMover.CurrentPointIndex;

                Debug.LogWarning($"Test HandleInteractEvent - {CatTake}, {takenIndex}");
                _eventBus.Raise<TakenEvent>(new() { Value = takenIndex });
                _pointMover.MoveToNextPoint();
            }
        }

        public void Instance()
        {
            // spawner.SpawnObject(Vector3.zero, Quaternion.identity);
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

        public int CurrentPointIndex => _pointMover.CurrentPointIndex;
    }

    public class PointMover
    {
        [SerializeField] private GameObject objectToMove;
        private Vector3[] points;
        public int CurrentPointIndex { get; private set; } = 0;

        public PointMover(GameObject objectToMove, GM gameManager)
        {
            this.objectToMove = objectToMove;
            this.gameManager = gameManager;
        }

        public void SetPoints(Vector3[] newPoints)
        {
            Debug.Log($"Test newPoints - [{newPoints}]");

            points = newPoints;
            CurrentPointIndex = 0;
        }


        public void MoveToNextPoint()
        {
            Debug.LogWarning(
                $"Test MoveToNextPoint - {CurrentPointIndex} - {points != null && CurrentPointIndex < points.Length}");

            var a = "";
            foreach (var point in points)
            {
                a += $" {point}";
            }

            Debug.LogWarning($"Test - ppints - {a}");
            Debug.LogWarning($"Test - CurrentPointIndex {CurrentPointIndex} < points.Length - {points.Length}");


            if (points != null && CurrentPointIndex < points.Length)
            {
                Debug.LogWarning($"Test Moved");

                objectToMove.transform.position = points[CurrentPointIndex];
                CurrentPointIndex++;


                if (CurrentPointIndex >= points.Length)
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