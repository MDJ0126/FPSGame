using System.Collections;
using System.Collections.Generic;
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

        private Character _owner = null;
        private SphereCollider _sphereCollider = null;
        private float radius = 10f;
        private List<Character> _detectedCharacters = new List<Character>();

        private void Start()
        {
            _sphereCollider = GetComponent<SphereCollider>();
            _sphereCollider.radius = radius;
        }

        private void OnTriggerEnter(Collider other)
        {
            var character = other.gameObject.GetComponent<Character>();
            if (character)
            {
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
                // 살아있는 적 캐릭터를 반환
                var enemy = _detectedCharacters.Find(character => !character.IsDead && _owner.TeamNember != character.TeamNember);
                if (enemy) 
                    return enemy;

                // 예외
                return _detectedCharacters[0];
            }
            return null;
        }
    }
}