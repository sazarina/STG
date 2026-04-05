using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace STG.Engine.Graphics {
    public static class LoadHelper {
        public static ContentManager Content;
        public static GraphicsDevice Device;
        public static void Initialize(ContentManager contentManager, GraphicsDevice graphicsDevice) {
            Content = contentManager;
            Device = graphicsDevice;
        }
    }
}
