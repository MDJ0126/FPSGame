﻿using UnityEngine;

namespace FPSGame.Character
{
    public class Zombie : AICharacter
    {
        protected override void Awake()
        {
            base.Awake();
            this.AI = this.gameObject.AddComponent<ZombieAI>();
        }

        private void Start()
        {
            this.MoveController.MoveTo(Vector3.zero);
        }
    }
}