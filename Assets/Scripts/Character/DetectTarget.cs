﻿using System.Collections.Generic;
using UnityEngine;

namespace FPSGame.Character
{
    public class DetectTarget : MonoBehaviour
    {
        public static DetectTarget AddComponent(Character character)
        {
            GameObject go = new GameObject("DetectTarget");
            go.layer = (int)eLayer.IgnoreRaycast;
            SphereCollider sphereCollider = go.AddComponent<SphereCollider>();
            sphereCollider.center = Vector3.zero;
            sphereCollider.isTrigger = true;
            go.transform.SetParent(character.transform);
            go.transform.Initialize();
            var detectTarget = go.AddComponent<DetectTarget>();
            detectTarget._owner = character;
            return detectTarget;
        }

        public float DetectDistance { get; private set; } = 15f;
        private Character _owner = null;
        private SphereCollider _sphereCollider = null;
        private List<Character> _detectedCharacters = new List<Character>();
        private Character _currentTarget = null;

        private void Start()
        {
            _sphereCollider = GetComponent<SphereCollider>();
            _sphereCollider.radius = DetectDistance;
        }

        private void OnTriggerEnter(Collider other)
        {
            var character = other.gameObject.GetComponent<Character>();
            if (character)
            {
                if (!_detectedCharacters.Exists(c => c.Equals(character)))
                    _detectedCharacters.Add(character);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var character = other.gameObject.GetComponent<Character>();
            if (character)
            {
                _detectedCharacters.Remove(character);
            }
        }

        private void OnGUI()
        {
            var detectedTarget = GetDectedCharacter();
            if (detectedTarget)
            {
                Debug.DrawRay(_owner.MyTransform.position, detectedTarget.MyTransform.position, Color.red);
            }
        }

        /// <summary>
        /// 탐지한 캐릭터를 반환
        /// </summary>
        /// <returns></returns>
        public Character GetDectedCharacter()
        {
            if (_detectedCharacters.Count == 0) return null;

            // 현재 타겟이 범위 안에 있는지 판단
            if (_currentTarget != null)
            {
                if (_currentTarget.TeamNember == _owner.TeamNember)
                {
                    _currentTarget = null;
                }
                else if (_currentTarget.IsDead)
                {
                    _currentTarget = null;
                }
                else
                { 
                    float distance = Vector3.Distance(_owner.MyTransform.position, _currentTarget.MyTransform.position);
                    if (DetectDistance < distance)
                    {
                        _currentTarget = null;
                    }
                }
            }

            // 타겟 검색
            if (_currentTarget == null)
            {
                // 가장 가까운 타겟 탐색
                Character minmumTarget = null;
                float minDistance = float.MaxValue;
                foreach (var target in _detectedCharacters)
                {
                    if (minmumTarget == null) minmumTarget = target;
                    // 죽지 않은 적 탐색
                    if (!minmumTarget.Equals(target) && !target.IsDead && _owner.TeamNember != target.TeamNember)
                    {
                        float distance = Mathf.Abs((_owner.MyTransform.position - target.MyTransform.position).sqrMagnitude);
                        if (minDistance > distance)
                        {
                            Vector3 aimCenter = _owner.MyTransform.position;
                            aimCenter.y = _owner.AimHeight;
                            Vector3 targetCenter = target.MyTransform.position;
                            targetCenter.y = Character.CHARACTER_HEIGHT_CENTER;

                            Vector3 direction = (targetCenter - aimCenter).normalized;
                            if (Physics.Raycast(aimCenter, direction, out var hit, DetectDistance))
                            {
                                if (hit.collider.gameObject.Equals(target.gameObject))
                                {
                                    minDistance = distance;
                                    minmumTarget = target;
                                }
                            }
                        }
                    }
                }

                if (!minmumTarget.IsDead)
                {
                    _currentTarget = minmumTarget;
                }
            }

            return _currentTarget;
        }
    }
}