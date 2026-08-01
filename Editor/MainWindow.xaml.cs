using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using STG.Engine;
using STG.Engine.Debugging;

namespace Editor {
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window {
        [DllImport("user32.dll")]
        private static extern uint GetDpiForSystem();

        public MainWindow() {
            InitializeComponent();

            // gameControl.Width/HeightはMyGame.Initialize()内でScreen.Width/Height
            // (=バックバッファ解像度)としてそのまま使われるため1200x800固定のまま触らない。
            // 表示上の物理サイズだけをDPIスケールの逆数でLayoutTransformにより補正し、
            // Debugging(WinForms, game1: 1200x800)と物理ピクセルで一致させる。
            double dpiScale = GetDpiForSystem() / 96.0;
            dpiCompensateTransform.ScaleX = 1.0 / dpiScale;
            dpiCompensateTransform.ScaleY = 1.0 / dpiScale;

            Loaded += (s, e) => {
                Dispatcher.BeginInvoke(new Action(() => {
                    gameControl.Focus();
                    Keyboard.Focus(gameControl);
                }), DispatcherPriority.ApplicationIdle);
            };

            gameControl.MouseEnter += (s, e) => {
                gameControl.Focus();
                Keyboard.Focus(gameControl);
            };

            SizeChanged += (s, e) => {
                gameControl.Focus();
            };
        }
    }
}
