using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Tizen.UIExtensions.Common
{
    [ExcludeFromCodeCoverage]
    public static class BatchableExtensions
    {
        static readonly ConditionalWeakTable<IBatchable, BatchCount> s_counters = new ConditionalWeakTable<IBatchable, BatchCount>();

        public static void BatchBegin(this IBatchable target)
        {
            if (s_counters.TryGetValue(target, out BatchCount? value))
            {
                value.Count++;
            }
            else
            {
                s_counters.Add(target, new BatchCount());
            }
        }

        public static void BatchCommit(this IBatchable target)
        {
            if (s_counters.TryGetValue(target, out BatchCount? value))
            {
                value.Count--;
                if (value.Count == 0)
                {
                    target.OnBatchCommitted();
                }
                else if (value.Count < 0)
                {
                    Log.Error("Called BatchCommit() without BatchBegin().");
                    value.Count = 0;
                }
            }
            else
            {
                Log.Error("Called BatchCommit() without BatchBegin().");
            }
        }

        public static bool IsBatched(this IBatchable target)
        {
            if (s_counters.TryGetValue(target, out BatchCount? value))
            {
                return value.Count != 0;
            }
            else
            {
                return false;
            }
        }

        class BatchCount
        {
            public int Count = 1;
        }
    }
}
