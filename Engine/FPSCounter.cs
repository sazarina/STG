using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using STG.Engine;
using STG.Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STG.Engine {
    class FPSCounter {
        public float FPS;    // 実際のFPS
        float m_interval; // FPSの更新速度（殆どの場合は１秒）
        float m_updateTimer; // 更新するまでを計るタイマー
        int m_frameCount; // 現在のフレーム数

        public FPSCounter() {
            FPS = 0.0f;
            m_interval = 2.0f;
            m_updateTimer = 0.0f;
            m_frameCount = 0;
        }

        // Game1内のDraw()メソッドで呼んでください
        public void Draw() {
            // フレーム数を増やす（目標は１秒に６０回）
            m_frameCount++;

            // タイマーに前のフレームから過ぎた時間を加算する
            m_updateTimer += TimeManager.deltaTime;

            // タイマーが1秒を超えたら
            if (m_updateTimer >= m_interval) {
                // FPSを計算する, 速度が下がっていた場合はここで差を計算する
                FPS = m_frameCount / m_updateTimer;

                // タイマーとカウンターをリセットする
                m_frameCount = 0;
                m_updateTimer -= m_interval;
            }

            // FPSの数値を描画する
            GraphicsUltis.DrawText("FPS: " + FPS, new Vector2(0, 0), Color.Firebrick);
        }
    }

}
