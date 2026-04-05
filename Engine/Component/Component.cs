using System;

namespace STG.Engine.Component {
    public class Component {
        public GameObject gameObject { get; protected set; }

        public string Name {
            get {
                if (gameObject != null) {
                    return gameObject.name;
                } else {
                    return name;
                }
            }
            set {
                name = value;

                if (gameObject != null) {
                    gameObject.name = value;
                }
            }
        }

        string name;

        public bool isActive { get; protected set; }
        public virtual void Initialize() {

        } 

        public virtual void  Update() { 
        
        }

        public virtual void Draw() {

        }

        public Action OnInitialize;
        public Action OnDestroy;
    }
}
