using System;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace NUIExGallery
{
    [Flags]
    public enum TargetProfile
    {
        Mobile = 1,
        Tv = 2,
        Wearable = 4
    }
    public abstract class TestCaseBase
    {
        public abstract string TestName { get; }
        public abstract string TestDescription { get; }
        public virtual TargetProfile TargetProfile => TargetProfile.Mobile | TargetProfile.Tv;
        public abstract View Run();
    }

    public abstract class WearableTestCase : TestCaseBase
    {
        public override TargetProfile TargetProfile => TargetProfile.Wearable;
    }
}
