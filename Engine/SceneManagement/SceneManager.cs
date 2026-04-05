using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STG.Engine.SceneManagement {
    class SceneManager {
        public static Scene Current { get; private set; }

        public void LoadScene(string sceneName) {
            Current = Scene.CreateScene(sceneName);
        }

        public async void LoadSceneAsync() {
            
        }

        public void Update() {
            Current.Update();
        }

        public void Draw() {
            Current.Draw();

        }


    }
}
