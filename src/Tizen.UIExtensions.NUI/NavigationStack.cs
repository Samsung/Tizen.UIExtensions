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
        Dictionary<View, WeakReference<View>> _focusStack = new Dictionary<View, WeakReference<View>>();

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
            DidSaveFocus();

            view.WidthResizePolicy = ResizePolicyType.FillToParent;
            view.HeightResizePolicy = ResizePolicyType.FillToParent;
            InternalStack.Add(view);
            Add(view);

            if (animated)
            {
                if (PushAnimation != null)
                {
                    await RunCustomPushAnimation(view, PushAnimation);
                }
                else
                {
                    await RunDefaultPushAnimation(view);
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

                if (animated)
                {
                    if (PopAnimation != null)
                    {
                        await RunCustomPopAnimation(tobeRemoved, PopAnimation);
                    }
                    else
                    {
                        await RunDefaultPopAnimation(tobeRemoved);
                    }
                }

                InternalStack.Remove(tobeRemoved);
                _focusStack.Remove(tobeRemoved);
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
            _focusStack.Clear();
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
            _focusStack.Remove(view);
            Remove(view);
        }

        protected virtual void DidSaveFocus()
        {
            if (Top != null)
            {
                var currentFocused = FocusManager.Instance.GetCurrentFocusView();
                if (currentFocused != null)
                {
                    _focusStack[Top] = new WeakReference<View>(currentFocused);
                }
            }
        }

        protected virtual void DidRestoreFocus()
        {
            if (Top != null)
            {
                if (_focusStack.ContainsKey(Top) && _focusStack[Top].TryGetTarget(out var target))
                {
                    FocusManager.Instance.SetCurrentFocusView(target);
                }
            }
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
                DidRestoreFocus();
            }
        }

        void SendNavigated()
        {
            Navigated?.Invoke(this, EventArgs.Empty);
        }

        async Task RunCustomPushAnimation(View view, Action<View, double> customAnimation)
        {
            if (Top != null)
            {
                Top.Sensitive = false;
            }

            view.Sensitive = false;
            var tcs = new TaskCompletionSource<bool>();
            var pushAni = new Animation((d) => customAnimation(view, d), easing: Easing.Linear);
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

        async Task RunCustomPopAnimation(View tobeRemoved, Action<View, double> customAnimation)
        {
            tobeRemoved.Sensitive = false;
            if (BelowTop != null)
            {
                BelowTop.Show();
                BelowTop.Sensitive = false;
            }

            var tcs = new TaskCompletionSource<bool>();
            var pushAni = new Animation((d) => customAnimation(tobeRemoved, d), easing: Easing.Linear);
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

        async Task RunDefaultPushAnimation(View view)
        {
            if (Top != null)
            {
                Top.Sensitive = false;
            }
            view.Sensitive = false;
            float oldOpacity = view.Opacity;

            try
            {
                view.Opacity = 0.5f;
                await view.FadeTo(1);

            }
            catch
            {
                // ignore exception
            }
            finally
            {
                view.Opacity = oldOpacity;
                view.Sensitive = true;

                if (Top != null)
                {
                    Top.Sensitive = true;
                }
            }
        }

        async Task RunDefaultPopAnimation(View view)
        {
            view.Sensitive = false;
            if (BelowTop != null)
            {
                BelowTop.Show();
                BelowTop.Sensitive = false;
            }

            var oldOpacity = view.Opacity;

            try
            {
                view.Opacity = 1f;
                await view.FadeTo(0);
            }
            catch
            {
                // ignore exception
            }
            finally
            {
                view.Opacity = oldOpacity;
                view.Sensitive = true;

                if (BelowTop != null)
                {
                    BelowTop.Sensitive = true;
                }
            }
        }

        void IAnimatable.BatchBegin() {}

        void IAnimatable.BatchCommit() {}
    }
}
