using System;

namespace STG.Engine.Component {
    public class Component {
        public GameObject gameObject { get; internal set; }
        public Transform transform => gameObject.transform;

        public string Name {
            get {
                if (gameObject != null) {
                    return gameObject.name;
                } else {
                    throw new InvalidOperationException("GameObjectにアタッチされていません");
                }
            }
            set {
                if (gameObject != null) {
                    gameObject.name = value;
                } else { 
                    throw new InvalidOperationException("GameObjectにアタッチされていません");
                }
            }
        }

        public bool isActive { get; internal set; }
        public virtual void Initialize() {

        } 

        public virtual void  Update() { 
        
        }

        public Action OnInitialize;
        public Action OnDestroy;
    }
}
