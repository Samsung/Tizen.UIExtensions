using System;
using SkiaSharp;
using SkiaSharp.Views.Tizen;
using System.Runtime.InteropServices;
using System.Threading;
using Tizen.NUI;
using NView = Tizen.NUI.BaseComponents.View;

namespace Tizen.UIExtensions.NUI
{
    /// <summary>
    /// A container view that clipping children view
    /// </summary>
    public class SKClipperView : NView
    {
        static readonly string VERTEX_SHADER =
            "attribute mediump vec2 aPosition;\n" +
            "varying mediump vec2 vTexCoord;\n" +
            "uniform highp mat4 uMvpMatrix;\n" +
            "uniform mediump vec3 uSize;\n" +
            "varying mediump vec2 sTexCoordRect;\n" +
            "void main()\n" +
            "{\n" +
            "   gl_Position = uMvpMatrix * vec4(aPosition * uSize.xy, 0.0, 1.0);\n" +
            "   vTexCoord = aPosition + vec2(0.5);\n" +
            "}\n";

        static readonly string FRAGMENT_SHADER = "" +
            "#extension GL_OES_EGL_image_external:require\n" +
            "uniform lowp vec4 uColor;\n" +
            "varying mediump vec2 vTexCoord;\n" +
            "uniform samplerExternalOES sTexture;\n" +
            "\n" +
            "void main(){\n" +
            "  mediump vec4 texColor = texture2D(sTexture, vTexCoord) * uColor;\n" +
            "  if (texColor.r < 1.0 || texColor.g < 1.0 || texColor.b < 1.0) discard;\n" +
            "  gl_FragColor = vec4(0.0, 0.0, 0.0, 0.0);\n" +
            "}\n" +
            "";

        Renderer _renderer;
        Geometry _geometry;
        Shader _shader;

        NativeImageQueue? _bufferQueue;
        Texture? _texture;
        TextureSet? _textureSet;

        int _bufferWidth = 0;
        int _bufferHeight = 0;
        int _bufferStride = 0;

        bool _redrawRequest;

        SynchronizationContext MainloopContext { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SKClipperView"/> class.
        /// </summary>
        public SKClipperView()
        {
            Layout = new CustomLayout
            {
                SizeUpdated = OnResized
            };
            ClippingMode = ClippingModeType.ClipChildren;
            MainloopContext = SynchronizationContext.Current ?? throw new InvalidOperationException("Must create on main thread");
            _geometry = CreateQuadGeometry();
            _shader = new Shader(VERTEX_SHADER, FRAGMENT_SHADER);

            RemoveRenderer(0);

            _bufferQueue = new NativeImageQueue(1, 1, NativeImageQueue.ColorFormat.RGBA8888);
            _texture = new Texture(_bufferQueue);
            _textureSet = new TextureSet();
            _textureSet.SetTexture(0u, _texture);
            _renderer = new Renderer(_geometry, _shader);
            _renderer.SetTextures(_textureSet);
            AddRenderer(_renderer);

            OnResized();
        }

        /// <summary>
        /// Occurs when need to draw clipping area. A white area will be shown, others will be clipped
        /// </summary>
        public event EventHandler<SKPaintSurfaceEventArgs>? DrawClippingArea;

        /// <summary>
        /// Invalidate clipping area
        /// </summary>
        public void Invalidate()
        {
            if (!_redrawRequest)
            {
                _redrawRequest = true;
                MainloopContext.Post((s)=>
                {
                    _redrawRequest = false;
                    if (!Disposed && _bufferQueue != null)
                    {
                        OnDrawFrame();
                    }
                }, null);
            }
        }

        protected void OnDrawFrame()
        {
            if (Size.Width == 0 || Size.Height == 0)
                return;

            if (_bufferQueue?.CanDequeueBuffer() ?? false)
            {
                var buffer = _bufferQueue!.DequeueBuffer(ref _bufferWidth, ref _bufferHeight, ref _bufferStride);
                var info = new SKImageInfo(_bufferWidth, _bufferHeight);
                using (var surface = SKSurface.Create(info, buffer, _bufferStride))
                {
                    // draw using SkiaSharp
                    OnDrawFrame(new SKPaintSurfaceEventArgs(surface, info));
                    surface.Canvas.Flush();
                }
                _bufferQueue.EnqueueBuffer(buffer);
                Window.Instance.KeepRendering(0);
            }
        }

        void UpdateBuffer()
        {
            _texture?.Dispose();
            _textureSet?.Dispose();
            _texture = new Texture(_bufferQueue);
            _textureSet = new TextureSet();
            _textureSet.SetTexture(0u, _texture);
            _renderer.SetTextures(_textureSet);
        }

        protected virtual void OnDrawFrame(SKPaintSurfaceEventArgs e)
        {
            DrawClippingArea?.Invoke(this, e);
        }

        protected virtual void OnResized()
        {
            if (Size.Width == 0 || Size.Height == 0)
                return;

            UpdateSurface();
            OnDrawFrame();
            UpdateBuffer();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _bufferQueue?.Dispose();
                _texture?.Dispose();
                _textureSet?.Dispose();
                _renderer?.Dispose();
            }
            base.Dispose(disposing);
        }

        void UpdateSurface()
        {
            _bufferQueue?.Dispose();
            _bufferQueue = new NativeImageQueue((uint)Size.Width, (uint)Size.Height, NativeImageQueue.ColorFormat.RGBA8888);
        }

        static Geometry CreateQuadGeometry()
        {
            PropertyBuffer vertexData = CreateVertextBuffer();

            TexturedQuadVertex vertex1 = new TexturedQuadVertex();
            TexturedQuadVertex vertex2 = new TexturedQuadVertex();
            TexturedQuadVertex vertex3 = new TexturedQuadVertex();
            TexturedQuadVertex vertex4 = new TexturedQuadVertex();
            vertex1.position = new Vec2(-0.5f, -0.5f);
            vertex2.position = new Vec2(-0.5f, 0.5f);
            vertex3.position = new Vec2(0.5f, -0.5f);
            vertex4.position = new Vec2(0.5f, 0.5f);

            TexturedQuadVertex[] texturedQuadVertexData = new TexturedQuadVertex[4] { vertex1, vertex2, vertex3, vertex4 };

            int lenght = Marshal.SizeOf(vertex1);
            IntPtr pA = Marshal.AllocHGlobal(lenght * 4);

            for (int i = 0; i < 4; i++)
            {
                Marshal.StructureToPtr(texturedQuadVertexData[i], pA + i * lenght, true);
            }
            vertexData.SetData(pA, 4);

            Geometry geometry = new Geometry();
            geometry.AddVertexBuffer(vertexData);
            geometry.SetType(Geometry.Type.TRIANGLE_STRIP);
            return geometry;
        }

        static PropertyBuffer CreateVertextBuffer()
        {
            PropertyMap vertexFormat = new PropertyMap();
            vertexFormat.Add("aPosition", new PropertyValue((int)PropertyType.Vector2));
            return new PropertyBuffer(vertexFormat);
        }

        struct TexturedQuadVertex
        {
            public Vec2 position;
        };

        [StructLayout(LayoutKind.Sequential)]
        struct Vec2
        {
            float x;
            float y;
            public Vec2(float xIn, float yIn)
            {
                x = xIn;
                y = yIn;
            }
        }

        class CustomLayout : AbsoluteLayout
        {
            float _width;
            float _height;

            public Action? SizeUpdated { get; set; }

            protected override void OnLayout(bool changed, LayoutLength left, LayoutLength top, LayoutLength right, LayoutLength bottom)
            {
                var sizeChanged = _width != Owner.SizeWidth || _height != Owner.SizeHeight;
                _width = Owner.SizeWidth;
                _height = Owner.SizeHeight;
                if (sizeChanged)
                {
                    SizeUpdated?.Invoke();
                }
                base.OnLayout(changed, left, top, right, bottom);
            }
        }
    }
}
