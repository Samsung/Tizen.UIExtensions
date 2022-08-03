using System;
using System.Collections.Generic;
using System.Text;
using Tizen.NUI.BaseComponents;
using Tizen.NUI;
using Tizen.UIExtensions.NUI;
using Tizen.UIExtensions.Common;
using TColor = Tizen.UIExtensions.Common.Color;
using Tizen.Applications;

namespace NUIExGallery.TC
{
    public class NavigationTest : TestCaseBase
    {
        public override string TestName => "Navigation Animation Test";

        public override string TestDescription => "Navigation Animation test";

        public override View Run()
        {
            var navi = new NavigationStack
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };

            navi.PushAnimation = (view, progress) =>
            {
                view.Opacity = 0.5f + 0.5f * (float)progress;
            };

            navi.PopAnimation = (view, progress) =>
            {
                view.Opacity = 0.5f + 0.5f * (float)(1 - progress);
            };

            _ = navi.Push(new PushPopPage(navi, 1), true);

            return navi;
        }

        class PushPopPage : View
        {
            NavigationStack _stack;
            public PushPopPage(NavigationStack stack, int depth)
            {
                _stack = stack;
                WidthSpecification = LayoutParamPolicies.MatchParent;
                HeightSpecification = LayoutParamPolicies.MatchParent;
                var rnd = new Random();
                BackgroundColor = TColor.FromRgba(rnd.Next(100, 200), rnd.Next(100, 200), rnd.Next(100, 200), 255).ToNative();


                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical
                };

                Add(new Label
                {
                    Text = $"Text {depth}"
                });

                var push = new Button
                {
                    Text = "Push (fade)"
                };
                Add(push);
                push.Clicked += (s, e) =>
                {
                    stack.PushAnimation = (view, progress) =>
                    {
                        view.Opacity = 0.5f + 0.5f * (float)progress;
                    };
                    _ = _stack.Push(new PushPopPage(_stack, depth+1), true);
                };

                var push2 = new Button
                {
                    Text = "Push (fade + Left)"
                };
                Add(push2);
                push2.Clicked += (s, e) =>
                {

                    float? originalX = null;
                    stack.PushAnimation = (view, progress) =>
                    {
                        if (originalX == null)
                        {
                            originalX = view.PositionX;
                        }
                        view.PositionX = originalX.Value + (float)(view.SizeWidth / 4.0f * (1 - progress));
                        view.Opacity = 0.5f + 0.5f * (float)progress;
                    };

                    _ = _stack.Push(new PushPopPage(_stack, depth + 1), true);
                };

                var push3 = new Button
                {
                    Text = "Push (fade + Up)"
                };
                Add(push3);
                push3.Clicked += (s, e) =>
                {

                    float? originY = null;
                    stack.PushAnimation = (view, progress) =>
                    {
                        if (originY == null)
                        {
                            originY = view.PositionY;
                        }
                        view.PositionY = originY.Value + (float)(view.SizeHeight / 4.0f * (1 - progress));
                        view.Opacity = 0.5f + 0.5f * (float)progress;
                    };

                    _ = _stack.Push(new PushPopPage(_stack, depth + 1), true);
                };

                var push4 = new Button
                {
                    Text = "Push (default)"
                };
                Add(push4);
                push4.Clicked += (s, e) =>
                {
                    stack.PushAnimation = null;
                    _ = _stack.Push(new PushPopPage(_stack, depth + 1), true);
                };



                var pop = new Button
                {
                    Text = "Pop (fade)"
                };
                Add(pop);
                pop.Clicked += (s, e) =>
                {
                    stack.PopAnimation = (view, progress) =>
                    {
                        view.Opacity = 0.5f + 0.5f * (float)(1 - progress);
                    };
                    _ = _stack.Pop(true);
                };

                var pop2 = new Button
                {
                    Text = "Pop (fade + Right)"
                };
                Add(pop2);
                pop2.Clicked += (s, e) =>
                {
                    float? originX = null;
                    stack.PopAnimation = (view, progress) =>
                    {
                        if (originX == null)
                        {
                            originX = view.PositionX;
                        }
                        view.PositionX = originX.Value + (float)(view.SizeWidth / 4.0f * progress);
                        view.Opacity = 0.5f + 0.5f * (float)(1 - progress);
                    };
                    _ = _stack.Pop(true);
                };

                var pop3 = new Button
                {
                    Text = "Pop (fade + down)"
                };
                Add(pop3);
                pop3.Clicked += (s, e) =>
                {
                    float? originY = null;
                    stack.PopAnimation = (view, progress) =>
                    {
                        if (originY == null)
                        {
                            originY = view.PositionY;
                        }
                        view.PositionY = originY.Value + (float)(view.SizeHeight / 4.0f * progress);
                        view.Opacity = 0.5f + 0.5f * (float)(1 - progress);
                    };
                    _ = _stack.Pop(true);
                };

                var pop4 = new Button
                {
                    Text = "Pop (default)"
                };
                Add(pop4);
                pop4.Clicked += (s, e) =>
                {
                    stack.PopAnimation = null;
                    _ = _stack.Pop(true);
                };


                Add(new Image
                {
                    ResourceUrl = Application.Current.DirectoryInfo.Resource + (depth % 2 == 0 ? "image.png" : "image2.jpg")
                });
            }
        }
    }
}
