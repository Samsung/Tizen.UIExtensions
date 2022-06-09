namespace UnitTests
{
    public class ColorTests
    {
        [Fact]
        public void TestDefaultColor()
        {
            var color = Color.Default;
            Assert.Equal(-1d, color.R);
            Assert.Equal(-1d, color.G);
            Assert.Equal(-1d, color.B);
            Assert.Equal(-1d, color.A);
            Assert.True(color.IsDefault);
        }

        [Fact]
        public void TestAccent()
        {
            var hotpink = Color.FromHex("#FF69B4");
            Color.SetAccent(hotpink);
            Assert.Equal(hotpink, Color.Accent);
        }

        [Fact]
        public void TestHSLPostSetEquality()
        {
            var color = new Color(1, 0.5f, 0.2f);
            var color2 = color;

            color2 = color.WithLuminosity(.2f);
            Assert.False(color == color2);
        }

        [Fact]
        public void TestHSLPostSetInequality()
        {
            var color = new Color(1, 0.5f, 0.2f);
            var color2 = color;

            color2 = color.WithLuminosity(.2f);

            Assert.True(color != color2);
        }

        [Fact]
        public void TestHSLSetToDefaultValue()
        {
            var color = new Color(0.2f, 0.5f, 0.8f);

            // saturation is initialized to 0, make sure we still update
            color = color.WithSaturation(0);

            Assert.Equal(color.R, color.G);
            Assert.Equal(color.R, color.B);
        }

        [Fact]
        public void TestHSLModifiers()
        {
            var color = Color.FromHsla(.8f, .6f, .2f);
            Assert.Equal(Color.FromHsla(.1f, .6f, .2f), color.WithHue(.1f));
            Assert.Equal(Color.FromHsla(.8f, .1f, .2f), color.WithSaturation(.1f));
            Assert.Equal(Color.FromHsla(.8f, .6f, .1f), color.WithLuminosity(.1f));
        }

        [Fact]
        public void TestMultiplyAlpha()
        {
            var color = new Color(1, 1, 1, 1);
            color = color.MultiplyAlpha(0.25f);
            Assert.Equal(.25, color.A);

            color = Color.FromHsla(1, 1, 1, 1);
            color = color.MultiplyAlpha(0.25f);
            Assert.Equal(.25, color.A);
        }

        [Fact]
        public void TestClamping()
        {
            var color = new Color(2, 2, 2, 2);

            Assert.Equal(1, color.R);
            Assert.Equal(1, color.G);
            Assert.Equal(1, color.B);
            Assert.Equal(1, color.A);

            color = new Color(-1, -1, -1, -1);

            Assert.Equal(0, color.R);
            Assert.Equal(0, color.G);
            Assert.Equal(0, color.B);
            Assert.Equal(0, color.A);
        }

        [Fact]
        public void TestRGBToHSL()
        {
            var color = new Color(.5f, .1f, .1f);

            Assert.Equal(1, color.Hue, 3);
            Assert.Equal(0.662, color.Saturation, 1);
            Assert.Equal(0.302, color.Luminosity, 1);
        }

        [Fact]
        public void TestHSLToRGB()
        {
            var color = Color.FromHsla(0, .662, .302);

            Assert.Equal(0.5, color.R, 2);
            Assert.Equal(0.1, color.G, 2);
            Assert.Equal(0.1, color.B, 2);
        }

        [Fact]
        public void TestColorFromValue()
        {
            var color = new Color(0.2f);

            Assert.Equal(new Color(0.2f, 0.2f, 0.2f, 1), color);
        }

        [Fact]
        public void TestAddLuminosity()
        {
            var color = new Color(0.2f);
            var brighter = color.AddLuminosity(0.2f);
            Assert.Equal(brighter.Luminosity, color.Luminosity + 0.2, 3);
        }

        [Fact]
        public void TestZeroLuminosity()
        {
            var color = new Color(0.1f, 0.2f, 0.3f);
            color = color.AddLuminosity(-1);

            Assert.Equal(0, color.Luminosity);
            Assert.Equal(0, color.R);
            Assert.Equal(0, color.G);
            Assert.Equal(0, color.B);
        }

        [Fact]
        public void TestHashCode()
        {
            var color1 = new Color(0.1f);
            var color2 = new Color(0.1f);

            Assert.True(color1.GetHashCode() == color2.GetHashCode());
        }

        [Fact]
        public void TestHashCodeNamedColors()
        {
            Color red = Color.Red; //R=1, G=0, B=0, A=1
            int hashRed = red.GetHashCode();

            Color blue = Color.Blue; //R=0, G=0, B=1, A=1
            int hashBlue = blue.GetHashCode();

            Assert.False(hashRed == hashBlue);
        }

        [Fact]
        public void TestHashCodeAll()
        {
            Dictionary<int, Color> colorsAndHashes = new Dictionary<int, Color>();
            colorsAndHashes.Add(Color.Transparent.GetHashCode(), Color.Transparent);
            colorsAndHashes.Add(Color.Aqua.GetHashCode(), Color.Aqua);
            colorsAndHashes.Add(Color.Black.GetHashCode(), Color.Black);
            colorsAndHashes.Add(Color.Blue.GetHashCode(), Color.Blue);
            colorsAndHashes.Add(Color.Fuchsia.GetHashCode(), Color.Fuchsia);
            colorsAndHashes.Add(Color.Gray.GetHashCode(), Color.Gray);
            colorsAndHashes.Add(Color.Green.GetHashCode(), Color.Green);
            colorsAndHashes.Add(Color.Lime.GetHashCode(), Color.Lime);
            colorsAndHashes.Add(Color.Maroon.GetHashCode(), Color.Maroon);
            colorsAndHashes.Add(Color.Navy.GetHashCode(), Color.Navy);
            colorsAndHashes.Add(Color.Olive.GetHashCode(), Color.Olive);
            colorsAndHashes.Add(Color.Purple.GetHashCode(), Color.Purple);
            colorsAndHashes.Add(Color.Pink.GetHashCode(), Color.Pink);
            colorsAndHashes.Add(Color.Red.GetHashCode(), Color.Red);
            colorsAndHashes.Add(Color.Silver.GetHashCode(), Color.Silver);
            colorsAndHashes.Add(Color.Teal.GetHashCode(), Color.Teal);
            colorsAndHashes.Add(Color.Yellow.GetHashCode(), Color.Yellow);
        }

        [Fact]
        public void TestSetHue()
        {
            var color = new Color(0.2f, 0.5f, 0.7f);
            color = Color.FromHsla(.2f, color.Saturation, color.Luminosity);

            Assert.Equal(0.6f, color.R, 3);
            Assert.Equal(0.7f, color.G, 3);
            Assert.Equal(0.2f, color.B, 3);
        }

        [Fact]
        public void ZeroLuminToRGB()
        {
            var color = new Color(0);
            Assert.Equal(0, color.Luminosity);
            Assert.Equal(0, color.Hue);
            Assert.Equal(0, color.Saturation);
        }

        [Fact]
        public void TestToString()
        {
            var color = new Color(1, 1, 1, 0.5f);
            Assert.Equal("[Color: A=0.5, R=1, G=1, B=1, Hue=0, Saturation=0, Luminosity=1]", color.ToString());
        }

        [Fact]
        public void TestFromRgbAndHex()
        {
            var color = Color.FromRgb(138, 43, 226);
            Assert.Equal(color, Color.FromHex("8a2be2"));

            Assert.Equal(Color.FromRgba(138, 43, 226, 128), Color.FromHex("#808a2be2"));
            Assert.Equal(Color.FromHex("#aabbcc"), Color.FromHex("#abc"));
            Assert.Equal(Color.FromHex("#aabbccdd"), Color.FromHex("#abcd"));
        }

        [Fact]
        public void TestToHex()
        {
            var colorRgb = Color.FromRgb(138, 43, 226);
            Assert.Equal(Color.FromHex(colorRgb.ToHex()), colorRgb);
            var colorRgba = Color.FromRgba(138, 43, 226, .2);
            Assert.Equal(Color.FromHex(colorRgba.ToHex()), colorRgba);
            var colorHsl = Color.FromHsla(240, 1, 1);
            Assert.Equal(Color.FromHex(colorHsl.ToHex()), colorHsl);
            var colorHsla = Color.FromHsla(240, 1, 1, .1f);
            var hexFromHsla = Color.FromHex(colorHsla.ToHex());
            Assert.Equal(hexFromHsla.A, colorHsla.A, 2);
            Assert.Equal(hexFromHsla.R, colorHsla.R, 3);
            Assert.Equal(hexFromHsla.G, colorHsla.G, 3);
            Assert.Equal(hexFromHsla.B, colorHsla.B, 3);
        }

        [Fact]
        public void TestFromHsv()
        {
            var color = Color.FromRgb(1, .29f, .752f);
            var colorHsv = Color.FromHsv(321, 71, 100);
            Assert.Equal(color.R, colorHsv.R, 3);
            Assert.Equal(color.G, colorHsv.G, 3);
            Assert.Equal(color.B, colorHsv.B, 3);
        }

        [Fact]
        public void TestFromHsva()
        {
            var color = Color.FromRgba(1, .29, .752, .5);
            var colorHsv = Color.FromHsva(321, 71, 100, 50);
            Assert.Equal(color.R, colorHsv.R, 3);
            Assert.Equal(color.G, colorHsv.G, 3);
            Assert.Equal(color.B, colorHsv.B, 3);
            Assert.Equal(color.A, colorHsv.A, 3);
        }

        [Fact]
        public void TestFromHsvDouble()
        {
            var color = Color.FromRgb(1, .29f, .758f);
            var colorHsv = Color.FromHsv(.89f, .71f, 1);
            Assert.Equal(color.R, colorHsv.R, 2);
            Assert.Equal(color.G, colorHsv.G, 2);
            Assert.Equal(color.B, colorHsv.B, 2);
        }

        [Fact]
        public void TestFromHsvaDouble()
        {
            var color = Color.FromRgba(1, .29, .758, .5);
            var colorHsv = Color.FromHsva(.89f, .71f, 1f, .5f);
            Assert.Equal(color.R, colorHsv.R, 2);
            Assert.Equal(color.G, colorHsv.G, 2);
            Assert.Equal(color.B, colorHsv.B, 2);
            Assert.Equal(color.A, colorHsv.A, 2);
        }

        [Fact]
        public void FromRGBDouble()
        {
            var color = Color.FromRgb(0.2, 0.3, 0.4);

            Assert.Equal(new Color(0.2f, 0.3f, 0.4f), color);
        }

        [Fact]
        public void FromRGBADouble()
        {
            var color = Color.FromRgba(0.2, 0.3, 0.4, 0.5);

            Assert.Equal(new Color(0.2f, 0.3f, 0.4f, 0.5f), color);
        }

        [Fact]
        public void DefaultColorsMatch()
        {
            //This spot-checks a few of the fields in Color
            Assert.Equal(Color.CornflowerBlue, Color.FromRgb(100, 149, 237));
            Assert.Equal(Color.DarkSalmon, Color.FromRgb(233, 150, 122));
            Assert.Equal(Color.Transparent, Color.FromRgba(255, 255, 255, 0));
            Assert.Equal(Color.Wheat, Color.FromRgb(245, 222, 179));
            Assert.Equal(Color.White, Color.FromRgb(255, 255, 255));
        }

        [Fact]
        public void TestFromUint()
        {
            var expectedColor = new Color(1, 0.65f, 0, 1);

            // Convert the expected color to a uint (argb)
            var blue = (int)(expectedColor.B * 255);
            var red = (int)(expectedColor.R * 255);
            var green = (int)(expectedColor.G * 255);
            var alpha = (int)(expectedColor.A * 255);

            uint argb = (uint)(blue | (green << 8) | (red << 16) | (alpha << 24));

            // Create a new color from the uint
            var fromUint = Color.FromUint(argb);

            // Verify the components
            Assert.Equal(expectedColor.A, fromUint.A, 2);
            Assert.Equal(expectedColor.R, fromUint.R, 2);
            Assert.Equal(expectedColor.G, fromUint.G, 2);
            Assert.Equal(expectedColor.B, fromUint.B, 2);
        }

        public static IEnumerable<object[]> TestFromHexValues()
        {
            yield return new object[] { "#111", Color.FromRgb(0x11, 0x11, 0x11) };
            yield return new object[] { "#F2E2D2", Color.FromRgb(0xF2, 0xE2, 0xD2) };
            yield return new object[] { "111", Color.FromRgb(0x11, 0x11, 0x11) };
            yield return new object[] { "F2E2D2", Color.FromRgb(0xF2, 0xE2, 0xD2) };
        }

        [Theory]
        [MemberData(nameof(TestFromHexValues))]
        public void TestFromHex(string value, Color expected)
        {
            Color actual = Color.FromHex(value);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> TestFromHexValuesHash()
        {
            yield return new object[] { "#111", Color.FromRgb(0x11, 0x11, 0x11) };
            yield return new object[] { "#a222", Color.FromRgba(0x22, 0x22, 0x22, 0xaa) };
            yield return new object[] { "#F2E2D2", Color.FromRgb(0xF2, 0xE2, 0xD2) };
            yield return new object[] { "#C2F2E2D2", Color.FromRgba(0xF2, 0xE2, 0xD2, 0xC2) };
        }

        public static IEnumerable<object[]> TestFromHexValuesNoHash()
        {
            yield return new object[] { "111", Color.FromRgb(0x11, 0x11, 0x11) };
            yield return new object[] { "a222", Color.FromRgba(0x22, 0x22, 0x22, 0xaa) };
            yield return new object[] { "F2E2D2", Color.FromRgb(0xF2, 0xE2, 0xD2) };
            yield return new object[] { "C2F2E2D2", Color.FromRgba(0xF2, 0xE2, 0xD2, 0xC2) };
        }

        [Theory]
        [MemberData(nameof(TestFromHexValuesHash))]
        [MemberData(nameof(TestFromHexValuesNoHash))]
        public void TestFromHex2(string value, Color expected)
        {
            Color actual = Color.FromHex(value);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> TestParseValidValues()
        {
            foreach (object[] argb in TestFromHexValuesHash())
            {
                yield return argb;
            }

            yield return new object[] { "rgb(255,0,0)", Color.FromRgb(255, 0, 0) };
            yield return new object[] { "rgb(100%, 0%, 0%)", Color.FromRgb(255, 0, 0) };

            yield return new object[] { "rgba(0, 255, 0, 0.7)", Color.FromRgba(0, 255, 0, 0.7f) };
            yield return new object[] { "rgba(0%, 100%, 0%, 0.7)", Color.FromRgba(0, 255, 0, 0.7f) };

            yield return new object[] { "hsl(120, 100%, 50%)", Color.FromHsla(120f / 360f, 1.0f, .5f) };
            yield return new object[] { "hsl(120, 75, 20%)", Color.FromHsla(120f / 360f, .75f, .2f) };

            yield return new object[] { "hsla(160, 100%, 50%, .4)", Color.FromHsla(160f / 360f, 1.0f, .5f, .4f) };
            yield return new object[] { "hsla(160,100%,50%,.6)", Color.FromHsla(160f / 360f, 1.0f, .5f, .6f) };

            yield return new object[] { "hsv(120, 85%, 35%)", Color.FromHsv(120f / 360f, .85f, .35f) };
            yield return new object[] { "hsv(120, 85, 35)", Color.FromHsv(120f / 360f, .85f, .35f) };

            yield return new object[] { "hsva(120, 100%, 50%, .8)", Color.FromHsva(120f / 360f, 1.0f, .5f, .8f) };
            yield return new object[] { "hsva(120, 100, 50, .8)", Color.FromHsva(120f / 360f, 1.0f, .5f, .8f) };
        }
    }
}