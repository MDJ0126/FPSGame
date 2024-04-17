using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace FPSGame.Character
{
    [RequireComponent(typeof(Character))]
    public class AnimatorController : MonoBehaviour
    {
        public TwoBoneIKConstraint LeftHand { get; private set; } = null;

        private Character _owner = null;
        private Animator _animator = null;
        private AnimatorReceiver _receiver = null;
        private Dictionary<eCharacterState, AnimatorStateMachineBehaviour> _states = new();
        private AnimatorStateMachineBehaviour _currentState = null;
        private List<eCharacterState> _fireList = new();
        private Rig[] _rigs = null;

        private void Awake()
        {
            _owner = GetComponent<Character>();
            _animator = GetComponentInChildren<Animator>(true);
            _receiver = GetComponentInChildren<AnimatorReceiver>(true);
            _rigs = GetComponentsInChildren<Rig>(true);
            FindHands();
        }

        private void OnEnable()
        {
            if (_rigs != null)
            {
                foreach (var rig in _rigs)
                {
                    rig.weight = 1f;
                }
            }
            _animator.Rebind();
            _states.Clear();
            _fireList.Clear();
            var states = _animator.GetBehaviours<AnimatorStateMachineBehaviour>();
            foreach (var state in states)
            {
                _states.Add(state.characterState, state);
                if (state is FireStateMachineBehaviour)
                {
                    _fireList.Add(state.characterState);
                }
            }
        }

        private void OnDisable()
        {
            _currentState = null;
        }

        public void Idle()
        {
            _animator.SetBool(AnimHash.IsWalk, false);
        }

        public void Walk(float directionX, float dicrectionY)
        {
            _animator.SetBool(AnimHash.IsWalk, true);
            _animator.SetFloat(AnimHash.VelocityX, Mathf.Lerp(_animator.GetFloat(AnimHash.VelocityX), directionX, 0.1f));
            _animator.SetFloat(AnimHash.VelocityZ, Mathf.Lerp(_animator.GetFloat(AnimHash.VelocityZ), dicrectionY, 0.1f));
        }

        private void FindHands()
        {
            var childs = this.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < childs.Length; i++)
            {
                Transform child = childs[i];
                if (child.name.Equals("Left Hand"))
                {
                    LeftHand = child.GetComponent<TwoBoneIKConstraint>();
                }
            }
        }

        /// <summary>
        /// 공격
        /// </summary>
        /// <param name="onFire"></param>
        /// <param name="onFinished"></param>
        public void Fire(Action onFire = null, Action onFinished = null)
        {
            if (_receiver)
            {
                _receiver.onFire = onFire;
                if (_fireList.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, _fireList.Count);
                    SetState(_fireList[randomIndex], onFinished);
                }
            }
        }

        public void SetState(eCharacterState state, Action onFinished = null)
        {
            _currentState?.OnFinishedEvent();
            _states.TryGetValue(state, out _currentState);
            _currentState.OnFinished = onFinished;

            switch (state)
            {
                case eCharacterState.Fire1:
                    _animator.SetTrigger(AnimHash.Fire1);
                    break;
                case eCharacterState.Fire2:
                    _animator.SetTrigger(AnimHash.Fire2);
                    break;
                case eCharacterState.Fire3:
                    _animator.SetTrigger(AnimHash.Fire3);
                    break;
                case eCharacterState.Dead:
                    {
                        _animator.SetTrigger(AnimHash.Dead);
                        if (_rigs != null)
                        {
                            foreach (var rig in _rigs)
                            {
                                rig.weight = 0f;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}