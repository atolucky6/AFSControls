using System;

namespace EasyScada.Core
{
    public class QualityAlarm : AlarmItemBase
    {
        public Quality TriggerQuality { get; set; }
        public override event EventHandler<AlarmStateChangedEventArgs> AlarmStateChanged;

        public override void Refresh()
        {
            base.Refresh();
            if (TriggerTag != null &&
                TriggerTag.Quality != Quality.Uncertain &&
                TriggerQuality != Quality.Uncertain)
            {
                if (IsNormal)
                {
                    if (TriggerTag.Quality == TriggerQuality)
                    {
                        IsNormal = false;
                        var oldState = AlarmState;
                        AlarmState = AlarmState.Incoming;
                        AlarmStateChanged?.Invoke(this, new AlarmStateChangedEventArgs(this, oldState, AlarmState, TriggerTag.Quality.ToString(), TriggerQuality.ToString(), "Equal"));
                    }
                }
                else
                {
                    if (TriggerTag.Quality != TriggerQuality)
                    {
                        IsNormal = true;
                        var oldState = AlarmState;
                        AlarmState = AlarmState.Outgoing;
                        AlarmStateChanged?.Invoke(this, new AlarmStateChangedEventArgs(this, oldState, AlarmState, TriggerTag.Quality.ToString(), TriggerQuality.ToString(), "Equal"));
                    }
                }
            }
        }
    }
}
