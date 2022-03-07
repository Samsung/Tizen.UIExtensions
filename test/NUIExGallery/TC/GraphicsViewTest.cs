using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.UIExtensions.NUI;
using Tizen.UIExtensions.NUI.GraphicsView;
using Color = Tizen.UIExtensions.Common.Color;

namespace NUIExGallery.TC
{
    public class GraphcisViewTest : TestCaseBase
    {
        public override string TestName => "GraphcisView Test";

        public override string TestDescription => "GraphcisViewTest test1";

        public override View Run()
        {
            var scrollview = new Tizen.UIExtensions.NUI.ScrollView();

            scrollview.ContentContainer.Layout = new LinearLayout
            {
                LinearAlignment = LinearLayout.Alignment.Center,
                LinearOrientation = LinearLayout.Orientation.Vertical,
            };

            var view = scrollview.ContentContainer;


            view.Add(new Label
            {
                Text = "Graphics View",
                TextColor = Color.White,
                FontSize = 9,
                FontAttributes = Tizen.UIExtensions.Common.FontAttributes.Bold,
                VerticalTextAlignment = Tizen.UIExtensions.Common.TextAlignment.Center,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                SizeHeight = 100,
                Padding = new Extents(20, 10, 10, 10),
                BackgroundColor = Color.FromHex("#2196f3").ToNative(),
                BoxShadow = new Shadow(5, Color.FromHex("#bbbbbb").ToNative(), new Vector2(0, 5))
            });

            view.Add(new View
            {
                SizeHeight = 20,
            });

            view.Add(new Label
            {
                Padding = new Extents(10, 0, 0, 0),
                Text = "ActivityIndicator",
                FontSize = 7,
                HorizontalTextAlignment = Tizen.UIExtensions.Common.TextAlignment.Start,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            });

            var hlayout = new View
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                },
                Padding = 10,
            };

            {
                var ai = new ActivityIndicator
                {
                    IsRunning = true,
                    Margin = 5,
                };
                ai.SizeHeight = (float)ai.Measure(300, 300).Height;
                ai.SizeWidth = (float)ai.Measure(300, 300).Width;
                view.Add(ai);
                hlayout.Add(ai);
                var timer = ElmSharp.EcoreMainloop.AddTimer(3, () =>
                {
                    ai.IsRunning = !ai.IsRunning;
                    return true;
                });
                ai.RemovedFromWindow += (s, e) =>
                {
                    ElmSharp.EcoreMainloop.RemoveTimer(timer);
                    (s as View).Dispose();
                };
            }

            {
                var ai = new ActivityIndicator
                {
                    Color = Color.FromHex("#ff9800"),
                    IsRunning = true,
                    Margin = 5
                };
                ai.SizeHeight = (float)ai.Measure(300, 300).Height;
                ai.SizeWidth = (float)ai.Measure(300, 300).Width;
                view.Add(ai);
                hlayout.Add(ai);
                var timer = ElmSharp.EcoreMainloop.AddTimer(3.3, () =>
                {
                    ai.IsRunning = !ai.IsRunning;
                    return true;
                });
                ai.RemovedFromWindow += (s, e) =>
                {
                    ElmSharp.EcoreMainloop.RemoveTimer(timer);
                    (s as View).Dispose();
                };
            }

            {
                var ai = new ActivityIndicator
                {
                    Color = Color.FromHex("#ffeb3b"),
                    IsRunning = true,
                    Margin = 5
                };
                ai.SizeHeight = (float)ai.Measure(300, 300).Height;
                ai.SizeWidth = (float)ai.Measure(300, 300).Width;
                view.Add(ai);
                hlayout.Add(ai);
                var timer = ElmSharp.EcoreMainloop.AddTimer(4, () =>
                {
                    ai.IsRunning = !ai.IsRunning;
                    return true;
                });
                ai.RemovedFromWindow += (s, e) =>
                {
                    ElmSharp.EcoreMainloop.RemoveTimer(timer);
                    (s as View).Dispose();
                };
            }

            view.Add(hlayout);

            view.Add(new Label
            {
                Padding = new Extents(10, 0, 0, 0),
                Text = "ProgressBar",
                FontSize = 7,
                HorizontalTextAlignment = Tizen.UIExtensions.Common.TextAlignment.Start,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            });

            {
                var progressBar2 = new ProgressBar
                {
                    Margin = 10,
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    ProgressColor = Color.GreenYellow,
                };
                progressBar2.SizeHeight = (float)progressBar2.Measure(300, 300).Height;

                view.Add(progressBar2);
                var timer = ElmSharp.EcoreMainloop.AddTimer(1, () =>
                {
                    if (progressBar2.Progress >= 1.0)
                        progressBar2.Progress = 0;

                    progressBar2.ProgressTo(progressBar2.Progress + 0.2);
                    return true;
                });
                progressBar2.RemovedFromWindow += (s, e) => ElmSharp.EcoreMainloop.RemoveTimer(timer);
            }


            var progressBar1 = new ProgressBar
            {
                Margin = 10,
                WidthSpecification = LayoutParamPolicies.MatchParent,
            };
            progressBar1.SizeHeight = (float)progressBar1.Measure(300, 300).Height;

            view.Add(progressBar1);

            view.Add(new View
            {
                SizeHeight = 10,
                WidthSpecification = LayoutParamPolicies.MatchParent,
            });

            view.Add(new Label
            {
                Padding = new Extents(10, 0, 10, 0),
                Text = "Slider",
                FontSize = 7,
                HorizontalTextAlignment = Tizen.UIExtensions.Common.TextAlignment.Start,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            });

            {
                var slider1 = new Slider
                {
                    IsEnabled = false,
                    Margin = 5,
                    Value = 0,
                    Minimum = 0,
                    Maximum = 1,
                    SizeHeight = 50,
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                };
                slider1.ValueChanged += (s, e) =>
                {
                    progressBar1.Progress = slider1.Value;
                };
                view.Add(slider1);
            }

            {
                var slider1 = new Slider
                {
                    Margin = 5,
                    Value = 0,
                    Minimum = 0,
                    Maximum = 1,
                    SizeHeight = 50,
                    MaximumTrackColor = Color.Red,
                    MinimumTrackColor = Color.Green,
                    ThumbColor = Color.Yellow,
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                };
                view.Add(slider1);
            }

            {
                var slider1 = new Slider
                {
                    Margin = 5,
                    Value = 0,
                    Minimum = -20,
                    Maximum = 10,
                    SizeHeight = 50,
                    MaximumTrackColor = Color.Red,
                    MinimumTrackColor = Color.Green,
                    ThumbColor = Color.Yellow,
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                };
                view.Add(slider1);
            }

            view.Add(new Label
            {
                Padding = new Extents(10, 0, 10, 0),
                Text = "Button",
                FontSize = 7,
                HorizontalTextAlignment = Tizen.UIExtensions.Common.TextAlignment.Start,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            });

            {
                var button = new Tizen.UIExtensions.NUI.GraphicsView.Button
                {
                    IsEnabled = false,
                    Margin = 5,
                    Text = "Clicked 0",
                    CornerRadius = 10,
                };

                button.SizeHeight = (float)button.Measure(300, 300).Height;
                button.SizeWidth = (float)button.Measure(300, 300).Width;
                int count = 1;
                button.Clicked += (s, e) =>
                {
                    button.Text = $"Clicked {count++}";
                };
                view.Add(button);
            }

            {
                var button = new Tizen.UIExtensions.NUI.GraphicsView.Button
                {
                    Margin = 5,
                    BackgroundColor = Color.Green,
                    Text = "BUTTON",
                };

                button.SizeHeight = (float)button.Measure(300, 300).Height;
                button.SizeWidth = (float)button.Measure(300, 300).Width;
                view.Add(button);
            }

            {
                var button = new Tizen.UIExtensions.NUI.GraphicsView.Button
                {
                    Margin = 5,
                    BackgroundColor = Color.Red,
                    Text = "BUTTON",
                };

                button.SizeHeight = (float)button.Measure(300, 300).Height;
                button.SizeWidth = (float)button.Measure(300, 300).Width + 100;
                view.Add(button);
            }

            view.Add(new Label
            {
                Padding = new Extents(10, 0, 10, 0),
                Text = "CheckBox",
                FontSize = 7,
                HorizontalTextAlignment = Tizen.UIExtensions.Common.TextAlignment.Start,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            });

            {
                var checkbox1 = new CheckBox
                {
                    Margin = 5,
                    Text = "CheckBox1",
                };
                checkbox1.SizeHeight = (float)checkbox1.Measure(300, 300).Height;
                checkbox1.SizeWidth = (float)checkbox1.Measure(300, 300).Width;
                view.Add(checkbox1);
            }
            {
                var checkbox1 = new CheckBox
                {
                    IsEnabled = false,
                    Margin = 5,
                    Text = "CheckBox1",
                };
                checkbox1.SizeHeight = (float)checkbox1.Measure(300, 300).Height;
                checkbox1.SizeWidth = (float)checkbox1.Measure(300, 300).Width;
                view.Add(checkbox1);
            }
            {
                var checkbox1 = new CheckBox
                {
                    IsEnabled = false,
                    IsChecked = true,
                    Margin = 5,
                    Text = "CheckBox1",
                };
                checkbox1.SizeHeight = (float)checkbox1.Measure(300, 300).Height;
                checkbox1.SizeWidth = (float)checkbox1.Measure(300, 300).Width;
                view.Add(checkbox1);
            }
            {
                var checkbox1 = new CheckBox
                {
                    Margin = 5,
                    Color = Color.Red,
                    Text = "Red",
                };
                checkbox1.SizeHeight = (float)checkbox1.Measure(300, 300).Height;
                checkbox1.SizeWidth = (float)checkbox1.Measure(300, 300).Width;
                view.Add(checkbox1);
            }
            {
                var checkbox1 = new CheckBox
                {
                    Margin = 5,
                    Color = Color.Blue,
                    Text = "Blue",
                };
                checkbox1.SizeHeight = (float)checkbox1.Measure(300, 300).Height;
                checkbox1.SizeWidth = (float)checkbox1.Measure(300, 300).Width;
                view.Add(checkbox1);
            }

            view.Add(new Label
            {
                Padding = new Extents(10, 0, 10, 0),
                Text = "Switch",
                FontSize = 7,
                HorizontalTextAlignment = Tizen.UIExtensions.Common.TextAlignment.Start,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            });

            hlayout = new View
            {
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                },
                Padding = 10,
            };

            {
                var switch1 = new Switch
                {
                    Margin = 5,
                };
                switch1.SizeHeight = (float)switch1.Measure(300, 300).Height;
                switch1.SizeWidth = (float)switch1.Measure(300, 300).Width;
                hlayout.Add(switch1);
            }
            {
                var switch1 = new Switch
                {
                    IsEnabled = false,
                    Margin = 5,
                    ThumbColor = Color.Red,
                    OnColor = Color.Yellow
                };
                switch1.SizeHeight = (float)switch1.Measure(300, 300).Height;
                switch1.SizeWidth = (float)switch1.Measure(300, 300).Width;
                hlayout.Add(switch1);
            }
            {
                var switch1 = new Switch
                {
                    Margin = 5,
                    ThumbColor = Color.BlueViolet,
                    OnColor = Color.Red,
                };
                switch1.SizeHeight = (float)switch1.Measure(300, 300).Height;
                switch1.SizeWidth = (float)switch1.Measure(300, 300).Width;
                hlayout.Add(switch1);
            }
            view.Add(hlayout);

            view.Add(new Label
            {
                Padding = new Extents(10, 0, 10, 0),
                Text = "Stepper",
                FontSize = 7,
                HorizontalTextAlignment = Tizen.UIExtensions.Common.TextAlignment.Start,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            });

            var stepper = new Stepper();
            stepper.SizeHeight = (float)stepper.Measure(300, 300).Height;
            stepper.SizeWidth = (float)stepper.Measure(300, 300).Width;
            view.Add(stepper);

            var stepperLabel = new Label
            {
                Text = "0",
            };
            view.Add(stepperLabel);

            stepper.ValueChanged += (s, e) => stepperLabel.Text = $"{stepper.Value}";


            view.Add(new Label
            {
                Padding = 10,
                Text = "Entry",
                FontSize = 7,
                HorizontalTextAlignment = Tizen.UIExtensions.Common.TextAlignment.Start,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
            });

            var gEntry = new Tizen.UIExtensions.NUI.GraphicsView.Entry
            {
                Placeholder = "Entry",
            };


            gEntry.SizeHeight = (float)gEntry.Measure(600, 300).Height;
            gEntry.SizeWidth = (float)gEntry.Measure(600, 300).Width;

            view.Add(gEntry);

            view.Add(new View
            {
                SizeHeight = 20
            });

            var gEntry2 = new Tizen.UIExtensions.NUI.GraphicsView.Entry
            {
                Placeholder = "Entry2",
                PlaceholderColor = Color.Red,
            };
            gEntry2.SizeHeight = (float)gEntry2.Measure(600, 300).Height;
            gEntry2.SizeWidth = (float)gEntry2.Measure(600, 300).Width;

            view.Add(gEntry2);

            view.Add(new View
            {
                SizeHeight = 60
            });


            var editor = new Tizen.UIExtensions.NUI.GraphicsView.Editor
            {
                Placeholder = "Editor"
            };

            editor.SizeHeight = 500;
            editor.SizeWidth = (float)editor.Measure(600, 300).Width;

            view.Add(editor);


            view.Add(new View
            {
                SizeHeight = 600
            });

            return scrollview;
        }
    }
}
