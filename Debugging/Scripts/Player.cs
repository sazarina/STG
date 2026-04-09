using STG.Engine.Component;
using STG.Engine.Graphics;

namespace STG.Engine.Debugging.Scripts {
    class Player : Behavior {
        override public void Start() {
            var sr = AddComponent<SpriteRenderer>();
            sr.SortingLayer = Layer("Chacter");
            sr.SortingOrder = 0;
            //sr.texture = LoadTexture("Content/Player.png", "Player");
        }

        public override void Update() {
        }
    }
}
