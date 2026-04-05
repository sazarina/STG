using STG.Engine.Component;
using STG.Engine.Debug;
using System;
using System.Collections.Generic;

namespace STG.Engine.Component {
    public partial class GameObject {
        #region Component 

        public T AddComponent<T>() where T : Component, new() {
            Type type = typeof(T);

            if (!IsRegisteredComponent<T>()) {
                if (type == typeof(Transform)) {
                    return (T)(object)(CreateTransformFunc());
                    //else if(type == typeof())

                } else {
                    throw new NotImplementedException($"{type.Name}型のは実装されていません");
                }

            } else {
                throw new ArgumentException($"{type.Name}は既にコンポーネントが登録されています");
            }
        }

        public T GetComponent<T>() {
            Type baseType = typeof(T).BaseType;

            if (baseType == typeof(ScriptBase)) {
                if (IsRegisteredComponent<T>()) {
                    return (T)(object)AttachedScripts[typeof(T)];
                } else {
                    Debug.Debug.Log($"{typeof(T).Name}型のスクリプトはアタッチされていません");
                    return default;
                }

            } else if (baseType == typeof(Component)) {
                if (IsRegisteredComponent<T>()) {
                    return (T)(object)ComponentList[typeof(T)];

                } else {
                    Debug.Debug.Log($"{typeof(T).Name}型のコンポーネントはアタッチされていません");
                    return default;
                }

            } else {
                Debug.Debug.Log($"{baseType.Name}型の親を持つコンポーネントは見つかりません");
                return default;
            }
        }
        public Dictionary<Type, Component> GetComponents()
            => ComponentList;

        bool IsRegisteredComponent<T>() {
            Type baseType = typeof(T).BaseType;
            if (baseType == typeof(ScriptBase)) {
                return AttachedScripts.ContainsKey(typeof(T));

            } else if (baseType == typeof(Component)) {
                return ComponentList.ContainsKey(typeof(T));

            } else {
                Debug.Debug.Log($"{baseType.Name}型の親を持つコンポーネントは見つかりません");
                return false;
            }
        }

        public T GetComponentInParent<T>() where T : Component, new() =>
            transform.Parent.gameObject.GetComponent<T>();

        public T[] GetComponentInChildren<T>() {
            List<T> components = new List<T>();

            if (transform.Children == null) {
                return null;
            }

            foreach (var child in transform.Children.Values) {
                if (child.IsRegisteredComponent<T>()) {
                    T t = child.GetComponent<T>();
                    if (t != null) {
                        components.Add(t);
                    }
                }
            }

            return components.ToArray();
        }

        #endregion


        #region AttachScript

        public static T CreateScirptInstance<T>(GameObject gameObject) where T: ScriptBase,new(){
            T t = new T();
            t.Initialize(ScriptController.self,gameObject);
            t.Start();
            return t;
        }
        
        public T AttachScript<T>() where T : ScriptBase, new() {
            T t = CreateScirptInstance<T>(this);

            AttachedScripts.Add(t.GetType(), t);
            return t;
        }

        public void AttachScript<T>(T t) where T : ScriptBase, new() {
            Debug.Debug.Log(typeof(T).BaseType);
            AttachedScripts.Add(t.GetType(), t);
        }
        #endregion
        public void Destroy() {
            GameObjectManager.Instance().Destroy(this);
        }
    }
}