using System;
using Common.EventBus.Events;
using Common.EventBus.Events.InGame;
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

        public InputReader()
        {
        }

        public void Init()
        {
            _eventBus = SL.Current.Get<GenericEventBus<TBaseEvent>>();

            _inputActions ??= new InputSystem_Actions();

            _inputActions.Player.SetCallbacks(this);
            _inputActions.UI.SetCallbacks(this);


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

        private MoveEvent _moveEvent = new();
        private SprintEvent _sprintEvent = new();
        private CancelSprintEvent _cancelSprintEven = new();
        private LookEvent _lookEvent = new();
        private JumpEvent _jumpEvent = new();

        #endregion


        public void OnAttack(InputAction.CallbackContext context)
        {
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
        }

        public void OnClick(InputAction.CallbackContext context)
        {
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                _eventBus.Raise<TryGetEvent>(new());
            Debug.Log($"- Test OnInteract - {context.phase}");
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                _eventBus.Raise<JumpEvent>(_jumpEvent);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _lookEvent.LookDirection = context.ReadValue<Vector2>();
            _eventBus.Raise<LookEvent>(_lookEvent);
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                _moveEvent.Direction = context.ReadValue<Vector2>();
                _eventBus.Raise<MoveEvent>(_moveEvent);
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                _moveEvent.Direction = context.ReadValue<Vector2>();
                _eventBus.Raise<MoveEvent>(_moveEvent);
            }
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
        }

        public void OnNext(InputAction.CallbackContext context)
        {
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                _eventBus.Raise<SprintEvent>(_sprintEvent);
            else if (context.phase == InputActionPhase.Canceled)
            {
                _eventBus.Raise<CancelSprintEvent>(_cancelSprintEven);
            }
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            _eventBus.Raise<PauseEvent>(new());
        }

        public void OnResume(InputAction.CallbackContext context)
        {
            _eventBus.Raise<ResumeEvent>(new());
        }
    }
}