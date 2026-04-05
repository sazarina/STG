using Microsoft.Xna.Framework;
using System;

namespace STG.Engine.Helper {
    public static class FileLocation {
        public readonly static string ResourcesFolderDirectory = "../../Resources";
        public readonly static string ImageFolderDirectory = Environment.CurrentDirectory+"/Resources/Images";
        public readonly static string ScriptFolderDirectory = "../../Script";
    }

    public static class CustomExtension {
        public static Vector2 Add(this Vector2 vector, Vector2 add) {
            return vector + add;
        }

        public static Vector2 Add(this Vector2 vector, int x = 0, int y = 0) {
            return vector + new Vector2(x, y);
        }

        public static System.Drawing.Size CastToSize(this Point point) {
            return new System.Drawing.Size(point.X, point.Y);
        }

        public static System.Drawing.Point CastToPoint(this Point point) {
            return new System.Drawing.Point(point.X, point.Y);
        }

        public static Point CastToPoint(this System.Drawing.Size size) {
            return new Point(size.Width, size.Height);
        }
    }
}
