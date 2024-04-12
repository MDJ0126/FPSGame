using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPSGame.Character
{
    public abstract class AICharacter : Character
    {
        /// <summary>
        /// AI 컨트롤러
        /// </summary>
        public AIComponent AI { get; protected set; } = null;
        public NavMeshAgent Agent { get; protected set; } = null;

        protected override void Awake()
        {
            base.Awake();
            this.Agent = GetComponent<NavMeshAgent>();
        }
    }
}