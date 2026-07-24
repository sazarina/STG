using System;

namespace STG.Engine.Component {
    public class Component {
        // GameObject.AddComponent<T>()で必ず設定されるため、ここではnull!で警告を抑制
        public GameObject gameObject { get; internal set; } = null!;
        public Transform transform {
            get {
                if (gameObject == null) {
                    throw new InvalidOperationException("GameObjectにアタッチされていません");
                }
                return gameObject.transform;
            }
        }

        public string name {
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

        internal Component() {
            OnInitialize += () => {

            };
            OnDestroy += () => {

            };
        }

        public virtual void Initialize() {

        } 

        public virtual void  Update() { 
        
        }

        public Action OnInitialize;
        public Action OnDestroy;
    }
}
