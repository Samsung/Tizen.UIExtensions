using Tizen.UIExtensions.ElmSharp;

namespace ElmSharpExGallery.TC
{
    public class SearchBarTest : TestCaseBase
    {
        public override string TestName => "SearchBarTest";

        public override string TestDescription => "Test SearchBar";

        public override void Run(ElmSharp.Box parent)
        {
            var searchBar = new SearchBar(parent)
            {
                WeightX = 1,
                WeightY = 1,
                AlignmentY = .1,
                AlignmentX = -1,
                Text = "searchBar"
            };

            searchBar.Show();
            parent.PackEnd(searchBar);
        }
    }
}
