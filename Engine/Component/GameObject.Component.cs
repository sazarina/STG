using System;
using System.Collections.Generic;
using Engine.Component;
using STG.Engine.Debugging;

namespace STG.Engine.Component {
    public partial class GameObject {
        #region Component 

        public T AddComponent<T>() where T : Component, new() {
            Type type = typeof(T);

            if (!IsRegisteredComponent<T>()) {
                if (typeof(Behavior).IsAssignableFrom(type)) {
                    var script = AttachScript(type);
                    ComponentList.Add(type, script);
                    return (T)script;
                } else if (type.BaseType == typeof(Component)) {
                    Component component = null;
                    if (type == typeof(SpriteRenderer)){ 
                        component  = new SpriteRenderer();
                        RenderManager.Instance().Register(component as SpriteRenderer);
                    }

                    component.gameObject = this;

                    ComponentList.Add(type, component);
                    return (T)component;
                } else {
                    throw new NotImplementedException($"{type.Name}型のは実装されていません");
                }

            } else {
                throw new ArgumentException($"{type.Name}は既にコンポーネントが登録されています");
            }
        }

        public T GetComponent<T>() {
            Type baseType = typeof(T).BaseType;

            if (baseType == typeof(Behavior)) {
                if (IsRegisteredComponent<T>()) {
                    return (T)(object)AttachedScripts[typeof(T)];
                } else {
                    Debug.Log($"{typeof(T).Name}型のスクリプトはアタッチされていません");
                    return default;
                }

            } else if (baseType == typeof(Component)) {
                if (IsRegisteredComponent<T>()) {
                    return (T)(object)ComponentList[typeof(T)];

                } else {
                    Debug.Log($"{typeof(T).Name}型のコンポーネントはアタッチされていません");
                    return default;
                }

            } else {
                Debug.Log($"{baseType.Name}型の親を持つコンポーネントは見つかりません");
                return default;
            }
        }

        public Dictionary<Type, Component> GetComponents()
            => ComponentList;

        public bool IsRegisteredComponent<T>() {
            Type baseType = typeof(T).BaseType;
            if (baseType == typeof(Component)  || baseType == typeof(Behavior)) {
                return ComponentList.ContainsKey(typeof(T));

            } else {
                Debug.Log($"{baseType.Name}型の親を持つコンポーネントは見つかりません");
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

        //internal static T CreateScirptInstance<T>(GameObject gameObject) where T : Behavior, new() {
        //    T t = new T();
        //    t.Initialize(ScriptController.Instance(), gameObject);
        //    t.Start();
        //    return t;
        //}

        
        //public T AttachScript<T>() where T : Behavior, new() {
        //    T t = CreateScirptInstance<T>(this);

        //    AttachedScripts.Add(t.GetType(), t);
        //    return t;
        //}

        Component AttachScript(Type type) {
           var script = (Behavior)Activator.CreateInstance(type);
            Debug.Log($"スクリプト:{type.Name}を{name}にアタッチします");
            script.Initialize(ScriptController.Instance(), this);
            script.Start();
            AttachedScripts.Add(type, script);
            return script;
        }

        #endregion
        public void Destroy() {
            GameObjectManager.Instance().Destroy(this);
        }
    }
}