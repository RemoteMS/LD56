using System;
using UnityEngine;
using TMPro;
using System.Collections;
using Common.EventBus.Events;
using Common.EventBus.Events.InGame;
using EventBus.Events;
using EventBus.Events.InGame;
using GenericEventBus;
using Helpers.Interfaces;
using ServiceLocator;

namespace Common.TMP
{


    public class TextFadeController : MonoBehaviour, IGameEventListener
    {
        public TextMeshProUGUI tmpText;
        public CanvasGroup canvasGroup;

        public float fadeDuration = 1f;

        private GenericEventBus<TBaseEvent> _eventBus;

        public void ShowText(int value)
        {
            tmpText.text = $"{value} / 8";
            StopAllCoroutines();
            StartCoroutine(FadeTextInAndOut());
        }

        public void SubscribeToEvents()
        {
            _eventBus.SubscribeTo<TryGetEvent>(GetTryGetEvent);
            _eventBus.SubscribeTo<TakenEvent>(HandleTakenEvent);
            _eventBus.SubscribeTo<RaycastStarted>(HandleTRaycastStartedEvent);
            _eventBus.SubscribeTo<RaycastEnded>(HandleRaycastEndedEvent);
        }


        public CanvasGroup RaycastTextHelp;


        private void HandleRaycastEndedEvent(ref RaycastEnded eventdata)
        {
            RaycastTextHelp.alpha = 0;
        }

        private void HandleTRaycastStartedEvent(ref RaycastStarted eventdata)
        {
            RaycastTextHelp.alpha = 1;
        }


        private void GetTryGetEvent(ref TryGetEvent eventdata)
        {
        }

        public void UnsubscribeFromEvents()
        {
            _eventBus.UnsubscribeFrom<TryGetEvent>(GetTryGetEvent);
            _eventBus.UnsubscribeFrom<TakenEvent>(HandleTakenEvent);
            _eventBus.UnsubscribeFrom<RaycastStarted>(HandleTRaycastStartedEvent);
            _eventBus.UnsubscribeFrom<RaycastEnded>(HandleRaycastEndedEvent);
        }

        private void HandleTakenEvent(ref TakenEvent eventdata)
        {
            ShowText(eventdata.Value);
        }


        private void Start()
        {
            _eventBus = SL.Current.Get<GenericEventBus<TBaseEvent>>();

            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private IEnumerator FadeTextInAndOut()
        {
            yield return StartCoroutine(Fade(0f, 1f));


            yield return new WaitForSeconds(5f);


            yield return StartCoroutine(Fade(1f, 0f));
        }

        private IEnumerator Fade(float startAlpha, float endAlpha)
        {
            var elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }


            canvasGroup.alpha = endAlpha;
        }


    }
}