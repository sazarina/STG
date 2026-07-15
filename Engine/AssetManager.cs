using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STG.Engine {
    public class AssetManager {
        ContentManager content = null;
        static AssetManager instance = null;

        Dictionary<string,Texture2D> assets = new Dictionary<string, Texture2D>();

        internal static AssetManager Instance {
            get {
                if (instance == null) {
                    instance = new AssetManager();
                }

                return instance;
            }
        }

        AssetManager() {

        }

        public void Initialize(ContentManager content) {
            this.content = content;
        }

        public static Texture2D Load(string assetName) {
            if (Instance.assets.ContainsKey(assetName)) {
                return Instance.assets[assetName];
            } else {
                if (Instance.content == null) {
                    throw new InvalidOperationException("AssetManager が初期化されていません。Initialize() を先に呼び出してください。");
                }

                var texture = Instance.content.Load<Texture2D>(assetName);
                Instance.assets[assetName] = texture;
                return texture;
            }
        }
    }
}
