using STG.Engine.Debugging;
using STG.Engine.Graphics;
using STG.Engine.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenon.Core;

namespace STG.Engine.SceneManagement {
    class Scene {
        string name;
        
        Dictionary<GameObject, LayerGroup> Layers = new Dictionary<GameObject, LayerGroup>();

        public static Scene CreateScene(string name) {
            return new Scene { name = name };
        }

        public void AddLayer(LayerGroup layerGroup) {
            if (!Layers.ContainsKey(layerGroup.gameObject)) {
                Layers.Add(layerGroup.gameObject, layerGroup);
            } else {
                Debug.Log($"{layerGroup.gameObject}は既に登録しています");
            }
        }


        public void Update() {
            Layers.Values.
                Where(x => x.gameObject.active).
                Select(x => x.gameObject).ForEach(gameObject => {
                    gameObject.Update();
                });
        }

        public void Draw() {
            var groupList = Layers.Values.GroupBy(x => x.layer.layerOrder).OrderBy(x => x.Key);
            groupList.ForEach(group => {
                var sorted = group.OrderBy(x => x.orderInLayer);
                sorted.Select(x => x.gameObject).Where(x => x.active).ForEach(gameObject => {
                    gameObject.Draw();
                    gameObject.GetComponents().Values.
                    ForEach(component => component.Draw());
                });
            });
        }
    }
}

//UnityEngineみたいにクライアント中で実行できるようにしたい
    