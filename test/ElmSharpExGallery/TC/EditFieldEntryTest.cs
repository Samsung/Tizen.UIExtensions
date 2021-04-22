using Tizen.UIExtensions.ElmSharp;
using Color = Tizen.UIExtensions.Common.Color;

namespace ElmSharpExGallery.TC
{
    public class EditFieldEntryTest : TestCaseBase
    {
        public override string TestName => "EditFieldEntryTest";

        public override string TestDescription => "Test EditFieldEntry";

        public override void Run(ElmSharp.Box parent)
        {
            var entry = new EditfieldEntry(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = .1,
                AlignmentX = -1,
                EnableClearButton = true,
                ClearButtonColor = Color.DeepPink.ToNative(),
                Text = "EditFiledEntry"
            };
            
            entry.Show();
            parent.PackEnd(entry);
        }
    }
}
