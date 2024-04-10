using UnityEngine;


namespace FPSGame.Character
{
    [RequireComponent(typeof(Character))]
    public abstract class AIComponent : MonoBehaviour
    {
        protected Character owner = null;

        protected virtual void Awake()
        {
            owner = GetComponent<Character>();
        }
    }
}