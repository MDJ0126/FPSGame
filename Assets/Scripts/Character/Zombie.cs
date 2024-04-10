using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSGame.Character
{
    public class Zombie : Character
    {
        protected override void Awake()
        {
            base.Awake();
            this.AI = this.gameObject.AddComponent<ZombieAI>();
        }
    }
}