using System;
using EventBus.Events;
using GenericEventBus;
using Helpers.Interfaces;
using ServiceLocator;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class InputReader
        : InputSystem_Actions.IPlayerActions,
            InputSystem_Actions.IUIActions,
            IService,
            IInitializable,
            IDisposable
    {
        private InputSystem_Actions _inputActions;

        private GenericEventBus<TBaseEvent> _eventBus;

        public InputReader() { }

        public void Init()
        {
            _eventBus = SL.Current.Get<GenericEventBus<TBaseEvent>>();

            _inputActions ??= new InputSystem_Actions();

            _inputActions.Player.SetCallbacks(this);
            _inputActions.UI.SetCallbacks(this);

            Debug.Log($"Enabling Gameplay Actions {_inputActions.bindings}");

            SetGameplay();
        }

        public void SetGameplay()
        {
            _inputActions.UI.Disable();
            _inputActions.Player.Enable();
        }

        public void SetUI()
        {
            _inputActions.Player.Disable();
            _inputActions.UI.Enable();
        }

        public void Dispose()
        {
            if (_inputActions != null)
                DisableAllInput();
        }

        private void DisableAllInput()
        {
            if (_inputActions == null)
                return;

            _inputActions.UI.Disable();
            _inputActions.Player.Disable();
        }

        #region Actions


        #endregion



        public void OnAttack(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnAttack - {context.phase}");
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnCancel - {context.phase}");
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnClick - {context.phase}");
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnCrouch after - {context.phase}");
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnInteract - {context.phase}");
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnJump - {context.phase}");
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnLook - {context.phase}");
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnMiddleClick - {context.phase}");
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnMove - {context.phase}, value - {context.ReadValue<Vector2>()}");
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnNavigate - {context.phase}");
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnNext - {context.phase}");
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnPoint - {context.phase}");
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnPrevious - {context.phase}");
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnRightClick - {context}");
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnScrollWheel - {context.phase}");
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnSprint - {context.phase}");
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnSubmit - {context.phase}");
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnTrackedDeviceOrientation - {context.phase}");
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnTrackedDevicePosition - {context.phase}");
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnPause - {context.phase}");
        }

        public void OnResume(InputAction.CallbackContext context)
        {
            Debug.Log($"- OnResume - {context.phase}");
        }
    }
}
