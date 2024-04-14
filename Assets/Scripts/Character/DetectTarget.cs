using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

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

        private Character _owner = null;
        private SphereCollider _sphereCollider = null;
        private float radius = 10f;
        private List<Character> _detectedCharacters = new List<Character>();

        private void Start()
        {
            _sphereCollider = GetComponent<SphereCollider>();
            _sphereCollider.radius = radius;
        }

        private void Update()
        {
            for (int i = _detectedCharacters.Count - 1; i > 0 ; i--)
            {
                var character = _detectedCharacters[i];
                if (character.Collider == null || character.Collider.enabled == false)
                {
                    _detectedCharacters.Remove(character);
                }
            }
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
            if (_detectedCharacters.Count > 0)
            {
                Character minmumTarget = _detectedCharacters[0];
                float minDistance = float.MaxValue;
                foreach (var target in _detectedCharacters)
                {
                    if (!minmumTarget.Equals(target) && !target.IsDead && _owner.TeamNember != target.TeamNember)
                    {
                        if (minmumTarget == null) minmumTarget = target;
                        float distance = (_owner.MyTransform.position - target.MyTransform.position).sqrMagnitude;
                        if (minDistance > distance)
                        {
                            Vector3 aimCenter = _owner.MyTransform.position;
                            aimCenter.y = _owner.AimHeight;
                            Vector3 targetCenter = target.MyTransform.position;
                            targetCenter.y = Character.CHARACTER_HEIGHT_CENTER;

                            Vector3 direction = (targetCenter - aimCenter).normalized;
                            if (Physics.Raycast(aimCenter, direction, out var hit, _sphereCollider.radius))
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
                return minmumTarget;
            }
            return null;
        }
    }
}