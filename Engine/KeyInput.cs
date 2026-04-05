using STG.Engine.Debug;
using STG.Engine.Helper;
using Microsoft.Xna.Framework.Input;

namespace STG.Engine {
    public static class KeyInput {
        //static Game game;

        public enum Mouses { 
            RightMouse,
            LeftMouse,
        }
        

        public static void Initialize() {
           // KeyInput.game = game;
        }

        public static KeyboardState CurrentKeyboard { get; set; }
        public static KeyboardState OldKeyboard { get; set; }


        public static MouseState CurrentMouseState { get; set; }
        public static MouseState OldMouseState { get; set; }

        static MouseState MouseState(bool isUseCurrent) =>
            isUseCurrent ? CurrentMouseState : OldMouseState;

        /// <summary>
        /// マウスが押されているか押されてないかを判定
        /// </summary>
        /// <param name="mouse">マウスのボタン</param>
        /// <param name="state">押されているか、いないか</param>
        /// <param name="isUseCurrent">規定値 TRUE</param>
        /// <returns></returns>
        static bool GetMouseState(Mouses mouse, ButtonState state,bool isUseCurrent=true) {
            switch (mouse) {
                case Mouses.RightMouse: return MouseState(isUseCurrent).RightButton == state;
                case Mouses.LeftMouse : return MouseState(isUseCurrent).LeftButton  == state;
                default:
                    Debug.Debug.Log($"{mouse.ToString()}はswitch case defaultです");
                    return false;
            }
        }

        public static bool MouseHeld(Mouses mouse) => 
            GetMouseState(mouse, ButtonState.Pressed);

        public static bool MouseJustReleased(Mouses mouse) => 
            GetMouseState(mouse, ButtonState.Released) && 
            GetMouseState(mouse,ButtonState.Pressed,false);

        public static bool MouseJustPressed(Mouses mouse) =>
            MouseHeld(mouse) && GetMouseState(mouse,ButtonState.Released,false);


        /// <summary>
        /// Use this to determine if the key is being held
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>returns true if the key is held.</returns>
        public static bool IsHeld(Keys key) =>
             CurrentKeyboard.IsKeyDown(key);

        public static bool IsHeld(string keyStr) =>
            IsHeld(EnumHelper<Keys>.ToEnum(keyStr));

        /// <summary>
        /// Use this to check if the key was just released
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>Returns true if the key was released</returns>
        public static bool IsReleased(Keys key) =>
            CurrentKeyboard.IsKeyUp(key) && OldKeyboard.IsKeyDown(key);
        
        public static bool IsReleased(string keyStr) =>
            IsReleased(EnumHelper<Keys>.ToEnum(keyStr));

        /// <summary>
        /// Use this to see if the key was just pressed
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>Returns true if the key was just pressed</returns>
        public static bool JustPressed(Keys key) =>
            CurrentKeyboard.IsKeyDown(key) && OldKeyboard.IsKeyUp(key);

        public static bool JustPressed(string keyStr) =>
            JustPressed(EnumHelper<Keys>.ToEnum(keyStr));
    }
}