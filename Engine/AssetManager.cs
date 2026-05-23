using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STG.Engine {
    public class AssetManager {
        static AssetManager self = null;
        ContentManager content = null;

        Dictionary<string,Texture2D> assets = new Dictionary<string, Texture2D>();

        internal static AssetManager Instance {
            get {
                if (self == null) {
                    self = new AssetManager();
                }

                return self;
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
                var texture = Instance.content.Load<Texture2D>(assetName);
                Instance.assets[assetName] = texture;
                return texture;
            }
        }
    }
}
