using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Tizen.Applications;

namespace Tizen.UIExtensions.Common.Internal
{
    public class Ticker
    {
        static Ticker? s_ticker;
        readonly Stopwatch _stopwatch;
        readonly List<Tuple<int, Func<long, bool>>> _timeouts;
        readonly SynchronizationContext? _context;
        Timer _timer;

        int _count;
        bool _enabled;

        protected Ticker()
        {
            _count = 0;
            _timeouts = new List<Tuple<int, Func<long, bool>>>();
            _stopwatch = new Stopwatch();

            if (SynchronizationContext.Current == null)
            {
                TizenSynchronizationContext.Initialize();
            }
            _context = SynchronizationContext.Current;
            _timer = new Timer((object? o) => HandleElapsed(o), this, Timeout.Infinite, Timeout.Infinite);
        }

        public static Ticker Default
        {
            get
            {
                if (s_ticker == null)
                {
                    s_ticker = new Ticker();
                }

                return s_ticker;
            }
        }

        public virtual int Insert(Func<long, bool> timeout)
        {
            _count++;
            _timeouts.Add(new Tuple<int, Func<long, bool>>(_count, timeout));

            if (!_enabled)
            {
                _enabled = true;
                Enable();
            }

            return _count;
        }

        public virtual void Remove(int handle)
        {
            global::ElmSharp.EcoreMainloop.Post(() => RemoveTimeout(handle));
        }

        void RemoveTimeout(int handle)
        {
            _timeouts.RemoveAll(t => t.Item1 == handle);

            if (_timeouts.Count == 0)
            {
                _enabled = false;
                Disable();
            }
        }

        protected void DisableTimer()
        {
            _timer.Change(-1, -1);
        }

        protected void EnableTimer()
        {
            _timer.Change(16, 16);
        }

        protected void SendFinish()
        {
            SendSignals(long.MaxValue);
        }

        protected void SendSignals(int timestep = -1)
        {
            long step = timestep >= 0
                ? timestep
                : _stopwatch.ElapsedMilliseconds;

            SendSignals(step);
        }

        protected void SendSignals(long step)
        {
            _stopwatch.Reset();
            _stopwatch.Start();

            var localCopy = new List<Tuple<int, Func<long, bool>>>(_timeouts);
            foreach (Tuple<int, Func<long, bool>> timeout in localCopy)
            {
                bool remove = !timeout.Item2(step);
                if (remove)
                    _timeouts.RemoveAll(t => t.Item1 == timeout.Item1);
            }

            if (_timeouts.Count == 0)
            {
                _enabled = false;
                Disable();
            }
        }

        void HandleElapsed(object? state)
        {
            _context?.Post((o) => SendSignals(-1), null);
        }

        void Disable()
        {
            _stopwatch.Reset();
            DisableTimer();
        }

        void Enable()
        {
            _stopwatch.Start();
            EnableTimer();
        }
    }
}
