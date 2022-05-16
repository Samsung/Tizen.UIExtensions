using System.Collections.Generic;
using System.Threading.Tasks;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;


namespace Tizen.UIExtensions.NUI
{
    public static class AnimationExtensions
    {
        static HashSet<Animation> s_animations = new HashSet<Animation>();
        public static Task<bool> FadeTo(this View view, float opacity, int length = 250)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            var animation = new Animation(length);
            animation.AnimateTo(view, nameof(view.Opacity), opacity);
            animation.EndAction = Animation.EndActions.StopFinal;
            animation.Finished += (s, e) =>
            {
                tcs.SetResult(true);
                s_animations.Remove(animation);
            };
            s_animations.Add(animation);
            animation.Play();
            return tcs.Task;
        }

        public static Task<bool> AnimationTo(this View view, string property, object value, int length = 250)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            var animation = new Animation(length);
            animation.AnimateTo(view, property, value);
            animation.EndAction = Animation.EndActions.StopFinal;
            animation.Finished += (s, e) =>
            {
                tcs.SetResult(true);
                s_animations.Remove(animation);
            };
            s_animations.Add(animation);
            animation.Play();
            return tcs.Task;
        }
    }
}
