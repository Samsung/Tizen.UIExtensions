using System;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace Tizen.UIExtensions.NUI
{
    public static class PerformanceMonitor
    {
        public class FrameUpdateCallback : FrameUpdateCallbackInterface
        {
            float _fps = 0;
            float _totalElapsed = 0;
            float[] _elapsedHistory = new float[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            int _idx = 0;

            public float FPS => _fps;

            public void Reset()
            {
                _totalElapsed = 0;
                _fps = 0;
            }

            public override void OnUpdate(float elapsedSeconds)
            {
                _elapsedHistory[_idx] = elapsedSeconds;
                _idx = (_idx + 1) % 10;
                _totalElapsed += (elapsedSeconds - _elapsedHistory[_idx]);
                var avgElapsedTime = _totalElapsed / 10;
                _fps = avgElapsedTime == 0 ? 0 : 1 / avgElapsedTime;
            }
        }

        static FrameUpdateCallback? _frameUpdate;
        static Layer? _overlayLayer;
        static TextLabel? _fpsLabel;
        // Main loop fps
        static DateTime _lastUpdated;
        static float[] _elapsedHistory = new float[10];
        static float _totalElapsed = 0;
        static int _idx = 0;

        public static void Attach()
        {
            if (_frameUpdate != null)
                return;

            _frameUpdate = new FrameUpdateCallback();
            Window.Instance.AddFrameUpdateCallback(_frameUpdate);

            _overlayLayer = new Layer();

            _fpsLabel = new TextLabel()
            {
                Position = new Position(0, 0),
                WidthResizePolicy = ResizePolicyType.UseNaturalSize,
                HeightResizePolicy = ResizePolicyType.UseNaturalSize,
                BackgroundColor = new Color(1f, 1f, 1f, 0.5f),
                PixelSize = 20,
            };
            _overlayLayer.Add(_fpsLabel);

            Window.Instance.AddLayer(_overlayLayer);
            Array.Clear(_elapsedHistory, 0, _elapsedHistory.Length);
            _lastUpdated = DateTime.Now;
            _totalElapsed = 0;
            Post(UpdateFPS);
        }

        public static void Detach()
        {
            if (_frameUpdate == null)
                return;

            Window.Instance.RemoveFrameUpdateCallback(_frameUpdate);
            Window.Instance.RemoveLayer(_overlayLayer);

            _overlayLayer?.Remove(_fpsLabel);
            _fpsLabel?.Dispose();
            _overlayLayer?.Dispose();
            _fpsLabel = null;
            _overlayLayer = null;
            _frameUpdate.Dispose();
            _frameUpdate = null;
        }

        static void UpdateFPS()
        {
            if (_frameUpdate == null)
                return;

            var now = DateTime.Now;
            var diff = now.Subtract(_lastUpdated);

            var elapsedMilliseconds = (float)diff.TotalMilliseconds;
            _elapsedHistory[_idx] = elapsedMilliseconds;
            _idx = (_idx + 1) % 10;
            _totalElapsed += (elapsedMilliseconds - _elapsedHistory[_idx]);
            var avgElapsedTime = _totalElapsed / 10f;
            var uiFPS= avgElapsedTime == 0 ? 0 : 1000.0 / avgElapsedTime;

            _fpsLabel!.Text = $"UI FPS : {uiFPS:0.00} , Rendering FPS : {_frameUpdate.FPS:0.00}";

            Post(UpdateFPS);
            _lastUpdated = DateTime.Now;
        }

        static void Post(Action action)
        {
            ElmSharp.EcoreMainloop.AddTimer(0.014, () =>
            {
                action();
                return false;
            });
        }
    }
}
