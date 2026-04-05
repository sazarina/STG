using STG.Engine.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STG.Engine.Debugging.Scripts {
    class Player : Behaviour {
        override public void Start() {
            gameObject.texture = LoadTexture("Content/Player.png", "Player");

        }

        public override void Update() {

        }
    }
}
