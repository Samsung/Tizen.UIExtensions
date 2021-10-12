using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Tizen.UIExtensions.Common;
using Tizen.UIExtensions.Common.GraphicsView;
using Tizen.UIExtensions.Common.Internal;
using Tizen.UIExtensions.ElmSharp.GraphicsView;
using Button = Tizen.UIExtensions.ElmSharp.Button;
using Label = Tizen.UIExtensions.ElmSharp.Label;
using GPoint = Microsoft.Maui.Graphics.Point;

namespace ElmSharpExGallery.TC
{
    public class DrawableTest : TestCaseBase, IAnimatable
    {
        public class SampleRefreshIcon : RefreshIcon
        {
            public SampleRefreshIcon(ElmSharp.EvasObject parent) : base(parent)
            {
                Drawable = new SampleDrawable(this);
                (Drawable as SampleDrawable).PropertyChanged += SampleRefreshIcon_PropertyChanged;
                // Set `AllowFocus` to true to allow the widget to have focus.
                AllowFocus(true);
            }

            public event EventHandler PointChanged;

            public event EventHandler FocusChanged;

            public GPoint TouchPoint { get; set; }

            public bool IsDrawableFocused{ get; set; }

            private void SampleRefreshIcon_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "TouchPoint")
                {
                    TouchPoint = (Drawable as SampleDrawable).TouchPoint;
                    PointChanged?.Invoke(this, EventArgs.Empty);
                }
                else if(e.PropertyName == "IsFocused")
                {
                    IsDrawableFocused = (Drawable as SampleDrawable).IsFocused;
                    FocusChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public class SampleDrawable : RefreshIconDrawable, INotifyPropertyChanged
        {
            public SampleDrawable(IRefreshIcon view) : base(view) { }

            GPoint _touchPoint;
            bool _isFocused;

            public GPoint TouchPoint
            {
                get => _touchPoint;
                set
                {
                    _touchPoint = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TouchPoint"));
                }
            }

            public bool IsFocused
            {
                get => _isFocused;
                set
                {
                    _isFocused = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsFocused"));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public override void OnTouchDown(GPoint point)
            {
                base.OnTouchDown(point);
                TouchPoint = new GPoint(point.X, point.Y);
            }

            public override void OnTouchUp(GPoint point)
            {
                base.OnTouchUp(point);
                TouchPoint = new GPoint(point.X, point.Y);
            }

            public override void OnTouchMove(GPoint point)
            {
                base.OnTouchMove(point);
                TouchPoint = new GPoint(point.X, point.Y);
            }

            public override void OnFocused()
            {
                base.OnFocused();
                IsFocused = true;
            }

            public override void OnUnfocused()
            {
                base.OnUnfocused();
                IsFocused = false;
            }
        }

        public override string TestName => "Drawable Test";

        public override string TestDescription => "Test Drawable";

        public override void Run(ElmSharp.Box parent)
        {
            var layout = new ElmSharp.Box(parent)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
            };
            layout.Show();
            parent.PackEnd(layout);

            var label = new Label(parent)
            {
                Text = "refreshIcon",
                FontSize = 20
            };
            label.Show();
            layout.PackEnd(label);
            var refreshIcon = new SampleRefreshIcon(parent)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1
            };
            var isFocusedLabel = new Label(parent)
            {
                Text = $"Is View Focused: {refreshIcon.IsDrawableFocused}",
                FontSize = 20
            };
            refreshIcon.FocusChanged += (s, e) =>
            {
                isFocusedLabel.Text = $"Is View Focused: {refreshIcon.IsDrawableFocused}";
            };
            isFocusedLabel.Show();
            layout.PackEnd(isFocusedLabel);
            var pointLabel = new Label(parent)
            {
                Text = $"Touch Point: (X={(int)refreshIcon.TouchPoint.X}, Y={(int)refreshIcon.TouchPoint.Y})",
                FontSize = 20
            };
            refreshIcon.PointChanged += (s, e) =>
            {
                pointLabel.Text = $"Touch Point: (X={(int)refreshIcon.TouchPoint.X}, Y={(int)refreshIcon.TouchPoint.Y})";
            };
            pointLabel.Show();
            layout.PackEnd(pointLabel);
            refreshIcon.Show();
            layout.PackEnd(refreshIcon);

            var isPullingButton = new Button(parent)
            {
                Text = $"IsPulling : {refreshIcon.IsPulling}",
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1
            };
            isPullingButton.Clicked += (s, e) =>
            {
                refreshIcon.IsPulling = refreshIcon.IsPulling ? false : true;
                isPullingButton.Text = $"IsPulling : {refreshIcon.IsPulling}";
            };
            isPullingButton.Show();
            layout.PackEnd(isPullingButton);

            var isRunningButton = new Button(parent)
            {
                Text = "IsRefreshing",
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1
            };
            isRunningButton.Clicked += (s, e) =>
            {
                refreshIcon.IsRunning = refreshIcon.IsRunning ? false : true;
            };
            isRunningButton.Show();
            layout.PackEnd(isRunningButton);

            var colorChangeButton = new Button(parent)
            {
                Text = "Change Color",
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1
            };
            colorChangeButton.Clicked += (s, e) =>
            {
                refreshIcon.Color = refreshIcon.Color == Color.Default ? Color.Red : Color.Default;
            };
            colorChangeButton.Show();
            layout.PackEnd(colorChangeButton);

            var backgroundColorChangeButton = new Button(parent)
            {
                Text = "Change Background",
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1
            };
            backgroundColorChangeButton.Clicked += (s, e) =>
            {
                refreshIcon.BackgroundColor = refreshIcon.BackgroundColor == Color.Yellow ? Color.White : Color.Yellow;
            };
            backgroundColorChangeButton.Show();
            layout.PackEnd(backgroundColorChangeButton);

            var simulateButton = new Button(parent)
            {
                Text = "Simulate Animation",
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1
            };
            simulateButton.Clicked += async (s, e) =>
            {
                if (refreshIcon.IsPulling)
                {
                    var iconMoveLength = 150;
                    var iconGeometry = refreshIcon.Geometry;
                    var _refreshIconStartAnimation = new Animation(v => {
                        refreshIcon.PullDistance = (float)(v / iconMoveLength);
                        refreshIcon.Move(iconGeometry.X, iconGeometry.Y + (int)v);
                    }, 0, iconMoveLength, Easing.Linear);
                    _refreshIconStartAnimation.Commit(this, "RefreshStart");
                    await Task.Delay(2000);
                    var _refreshIconEndAnimation = new Animation(v => refreshIcon.Move(iconGeometry.X, iconGeometry.Y + iconMoveLength - (int)v), 0, iconMoveLength, Easing.Linear);
                    _refreshIconEndAnimation.Commit(this, "RefreshEnd");
                }
            };
            simulateButton.Show();
            layout.PackEnd(simulateButton);
        }

        public void BatchBegin() { }

        public void BatchCommit() { }
    }
}
