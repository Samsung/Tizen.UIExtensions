using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.Common.Internal;
using Animation = Tizen.UIExtensions.Common.Internal.Animation;

namespace Tizen.UIExtensions.NUI
{

    /// <summary>
    /// A View that provide view stacking with push/pop animation
    /// </summary>
    public class NavigationStack : View, IAnimatable
    {
        View? _lastTop;

        /// <summary>
        /// /// Initializes a new instance of the <see cref="NavigationStack"/> class.
        /// </summary>
        public NavigationStack()
        {
            Layout = new AbsoluteLayout();
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            InternalStack = new List<View>();
        }

        /// <summary>
        /// Raised when top page was changed
        /// </summary>
        public event EventHandler? Navigated;

        /// <summary>
        /// A stack of views
        /// </summary>
        public IReadOnlyList<View> Stack => InternalStack;

        /// <summary>
        /// A view at the top
        /// </summary>
        public View? Top => _lastTop;

        List<View> InternalStack { get; set; }

        View? BelowTop
        {
            get
            {
                if (InternalStack.Count < 2)
                {
                    return null;
                }
                return InternalStack[InternalStack.Count - 2];
            }
        }

        /// <summary>
        /// User customized push animation function
        /// </summary>
        /// <remarks>
        /// View is a target that is pushed and `double` is progress of push
        /// </remarks>
        public Action<View, double>? PushAnimation { get; set; }

        /// <summary>
        /// User customized pop animation function
        /// </summary>
        /// <remarks>
        /// View is a target that is popped and `double` is progress of pop
        /// </remarks>
        public Action<View, double>? PopAnimation { get; set; }

        /// <summary>
        /// Options to show the page that behind of the top page
        /// </summary>
        public bool ShownBehindPage { get; set; } = false;

        /// <summary>
        /// Push a view on stack
        /// </summary>
        /// <param name="view">A view to push</param>
        /// <param name="animated">Flags for animation</param>
        public async Task Push(View view, bool animated)
        {
            view.WidthResizePolicy = ResizePolicyType.FillToParent;
            view.HeightResizePolicy = ResizePolicyType.FillToParent;

            InternalStack.Add(view);
            Add(view);

            if (animated && PushAnimation != null)
            {
                if (Top != null)
                {
                    Top.Sensitive = false;
                }

                view.Sensitive = false;
                var tcs = new TaskCompletionSource<bool>();
                var pushAni = new Animation((d) => PushAnimation(view, d), easing: Easing.SinOut);
                pushAni.Commit(this, "PushAnimation", finished: (d, b) =>
                 {
                     tcs.SetResult(true);
                 });
                await tcs.Task;
                view.Sensitive = true;

                if (Top != null)
                {
                    Top.Sensitive = true;
                }
            }
            UpdateTopView();
        }

        /// <summary>
        /// Remove top view on stack
        /// </summary>
        /// <param name="animated">Flags for animation</param>
        public async Task Pop(bool animated)
        {
            if (Top != null)
            {
                var tobeRemoved = Top;

                if (animated && PopAnimation != null)
                {
                    tobeRemoved.Sensitive = false;
                    if (BelowTop != null)
                    {
                        BelowTop.Show();
                        BelowTop.Sensitive = false;
                    }

                    var tcs = new TaskCompletionSource<bool>();
                    var pushAni = new Animation((d) => PopAnimation(tobeRemoved, d), easing: Easing.SinOut);
                    pushAni.Commit(this, "PopAnimation", finished: (d, b) =>
                    {
                        tcs.SetResult(true);
                    });
                    await tcs.Task;
                    tobeRemoved.Sensitive = true;
                    if (BelowTop != null)
                    {
                        BelowTop.Sensitive = true;
                    }
                }

                InternalStack.Remove(tobeRemoved);
                Remove(tobeRemoved);
                UpdateTopView();
                tobeRemoved.Dispose();
            }
        }

        /// <summary>
        /// Pops all but the root view off the navigation stack.
        /// </summary>
        public void PopToRoot()
        {
            while (InternalStack.Count > 1)
            {
                _ = Pop(false);
            }
        }

        /// <summary>
        /// Clear all children
        /// </summary>
        public void Clear()
        {
            foreach (var child in InternalStack)
            {
                Remove(child);
                child.Dispose();
            }
            InternalStack.Clear();
            _lastTop = null;
        }

        /// <summary>
        /// Inserts a view in the navigation stack before an existing view in the stack.
        /// </summary>
        /// <param name="before">The existing view, before which view will be inserted.</param>
        /// <param name="view">The view to insert</param>
        public void Insert(View before, View view)
        {
            view.Hide();
            var idx = InternalStack.IndexOf(before);
            InternalStack.Insert(idx, view);
            Add(view);
            UpdateTopView();
        }

        /// <summary>
        /// Removes a view in the navigation stack
        /// </summary>
        /// <param name="view">The view to remove</param>
        public void Pop(View view)
        {
            InternalStack.Remove(view);
            Remove(view);
        }

        void UpdateTopView()
        {
            if (_lastTop != InternalStack.LastOrDefault())
            {
                if (_lastTop != null)
                {
                    if (!ShownBehindPage)
                        _lastTop.Hide();
                    _lastTop.FocusableChildren = false;
                }

                _lastTop = InternalStack.LastOrDefault();

                if (_lastTop != null)
                {
                    _lastTop.Show();
                    _lastTop.FocusableChildren = true;
                }
                SendNavigated();
            }
        }

        void SendNavigated()
        {
            Navigated?.Invoke(this, EventArgs.Empty);
        }

        void IAnimatable.BatchBegin() {}

        void IAnimatable.BatchCommit() {}
    }
}
