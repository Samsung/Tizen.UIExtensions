using GColor = Microsoft.Maui.Graphics.Color;
using TColor = Tizen.UIExtensions.Common.Color;

namespace Tizen.UIExtensions.Common.GraphicsView
{
    public static class GraphicsColorExtensions
    {
        public static GColor ToGraphicsColor(this TColor color, string fallback)
        {
            if (!color.IsDefault)
                return new GColor((float)color.R, (float)color.G, (float)color.B, (float)color.A);
            else
                return GColor.FromArgb(fallback);
        }
    }

    internal static class Material
    {
        public static class Color
        {
            public const string Red = "#f44336";
            public const string Pink = "#e91e63";
            public const string Purple = "#9c27b0";
            public const string DeepPurple = "#673ab7";
            public const string Indigo = "#3f51b5";
            public const string Blue = "#2196f3";
            public const string LightBlue = "#03a9f4";
            public const string Cyan = "#00bcd4";
            public const string Teal = "#009688";
            public const string Green = "#4caf50";
            public const string LightGreen = "#8bc34a";
            public const string Lime = "#cddc39";
            public const string Yellow = "#ffeb3b";
            public const string Amber = "#ffc107";
            public const string Orange = "#ff9800";
            public const string DeepOrange = "#ff5722";

            public const string Gray1 = "#8E8E93";
            public const string Gray2 = "#C7C7CC";
            public const string Gray3 = "#D1D1D6";
            public const string Gray4 = "#E5E5EA";
            public const string Gray5 = "#EFEFF4";
            public const string Gray6 = "#F2F2F7";

            public const string Black = "#000000";
            public const string Dark = "#1F1F1F";
            public const string White = "#FFFFFF";
            public const string Light = "#E3E3E3";
        }

        public static class Font
        {
            public const float H1 = 96;
            public const float H2 = 60;
            public const float H3 = 48;
            public const float H4 = 34;
            public const float H5 = 24;
            public const float H6 = 20;
            public const float Subtitle1 = 16;
            public const float Subtitle2 = 14;
            public const float Body1 = 16;
            public const float Body2 = 14;
            public const float Button = 14;
            public const double Caption = 12;
            public const float Overline = 10;
        }
    }

    internal static class Cupertino
    {
        public static class Color
        {
            public static class SystemColor
            {
                public static class Light
                {
                    public const string Blue = "#007AFF";
                    public const string Green = "#33C759";
                    public const string Indigo = "#5856D6";
                    public const string Orange = "#FF9500";
                    public const string Pink = "#FF2D55";
                    public const string Purple = "#AF52DE";
                    public const string Red = "#FF3B30";
                    public const string Teal = "#59C8FA";
                    public const string Yellow = "#FFCC00";
                }

                public static class Dark
                {
                    public const string Blue = "#0684FF";
                    public const string Green = "#30D158";
                    public const string Indigo = "#5E5CE6";
                    public const string Orange = "#FF9F08";
                    public const string Pink = "#FF375F";
                    public const string Purple = "#BF5AF2";
                    public const string Red = "#FF443A";
                    public const string Teal = "#65D1FF";
                    public const string Yellow = "#FFD606";
                }
            }

            public static class SystemGray
            {
                public static class Light
                {
                    public const string Gray1 = "#8E8E93";
                    public const string Gray2 = "#C7C7CC";
                    public const string Gray3 = "#D1D1D6";
                    public const string Gray4 = "#E5E5EA";
                    public const string Gray5 = "#EFEFF4";
                    public const string Gray6 = "#F2F2F7";
                    public const string InactiveGray = "#999999";
                }

                public static class Dark
                {
                    public const string Gray1 = "#8E8E93";
                    public const string Gray2 = "#636366";
                    public const string Gray3 = "#48484A";
                    public const string Gray4 = "#3A3A3C";
                    public const string Gray5 = "#2C2C2E";
                    public const string Gray6 = "#1C1C1E";
                    public const string InactiveGray = "#757575";
                }
            }

            public static class Label
            {
                public static class Light
                {
                    public const string Primary = "#000000";
                    public const string Secondary = "#7C7C80";
                    public const string Tertiary = "#ACACAE";
                    public const string Quaternary = "#BFBFC0";
                    public const string Link = "#0077FF";
                    public const string Black = "#000000";
                    public const string Error = "#FF453A";
                }

                public static class Dark
                {
                    public const string Primary = "#FFFFFF";
                    public const string Secondary = "#A0A0A7";
                    public const string Tertiary = "#67676A";
                    public const string Quaternary = "#515153";
                    public const string Link = "#007AFF";
                    public const string Black = "#000000";
                    public const string Error = "#FF453A";
                }
            }

            public static class Fill
            {
                public static class Light
                {
                    public const string Black = "#000000";
                    public const string White = "#99EBEBF5";
                    public const string Primary = "#C8C8CA";
                    public const string Secondary = "#CCCCCD";
                    public const string Tertiary = "#D0D0D1";
                    public const string Quaternary = "#D4D4D5";
                    public const string NonOpaqueSeparator = "#AEAEB0";
                    public const string OpaqueSeparator = "#C6C6C8";
                }

                public static class Dark
                {
                    public const string Black = "#000000";
                    public const string White = "#99EBEBF5";
                    public const string Primary = "#49494C";
                    public const string Secondary = "#464649";
                    public const string Tertiary = "#404042";
                    public const string Quaternary = "#3B3B3E";
                    public const string NonOpaqueSeparator = "#47474A";
                    public const string OpaqueSeparator = "#38383A";
                }
            }

            public static class Background
            {
                public static class Light
                {
                    public const string Primary = "#FFFFFF";
                    public const string Secondary = "#F2F2F7";
                    public const string Transparent = "#FFFFFFFF";
                }

                public static class Dark
                {
                    public const string Primary = "#1C1C1E";
                    public const string Secondary = "#2C2C2E";
                    public const string Transparent = "#FFFFFFFF";
                }
            }
        }

        public static class Font
        {
            public const double LargeTitle = 34;
            public const double Title1 = 28;
            public const double Title2 = 22;
            public const double Title3 = 20;
            public const double Headline = 17;
            public const double Body = 17;
            public const double Callout = 16;
            public const double Subhead = 15;
            public const double Footnote = 13;
            public const double Caption1 = 12;
            public const double Caption2 = 11;
        }
    }

    internal static class Fluent
    {
        public static class Color
        {
            public static class Primary
            {
                public const string ThemeDarker = "#004578";
                public const string ThemeDark = "#005a9e";
                public const string ThemeDarkAlt = "#106ebe";
                public const string ThemePrimary = "#0078d4";
                public const string ThemeSecondary = "#2b88d8";
                public const string ThemeTertiary = "#71afe5";
                public const string ThemeLight = "#c7e0f4";
                public const string ThemeLighter = "#deecf9";
                public const string ThemeLighterAlt = "#eff6fc";
            }

            public static class Foreground
            {
                public const string Black = "#000000";
                public const string NeutralDark = "#201f1e";
                public const string NeutralPrimary = "#323130";
                public const string NeutralPrimaryAlt = "#3b3a39";
                public const string NeutralSecondary = "#605e5c";
                public const string NeutralTertiary = "#a19f9d";
                public const string White = "#ffffff";
            }

            public static class Background
            {
                public const string NeutralTertiaryAlt = "#c8c6c4";
                public const string NeutralDark = "#201f1e";
                public const string NeutralQuaternaryAlt = "#e1dfdd";
                public const string NeutralLight = "#edebe9";
                public const string NeutralLighter = "#f3f2f1";
                public const string NeutralLighterAlt = "#faf9f8";
            }
        }

        public static class Font
        {
            public const double Header = 46;
            public const double SubHeader = 34;
            public const double Title = 24;
            public const double SubTitle = 20;
            public const double Base = 14;
            public const double Body = 14;
            public const double Caption = 12;
        }
    }
}
