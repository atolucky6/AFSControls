using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyScada.Core
{
    public class DiscreteAlarm : AlarmItemBase
    {
        public string TriggerValue { get; set; }

        public override event EventHandler<AlarmStateChangedEventArgs> AlarmStateChanged;

        public override void Refresh()
        {
            base.Refresh();
            if (TriggerTag != null &&
                TriggerTag.Quality == Quality.Good &&
                double.TryParse(TriggerValue, out double triggerValue) &&
                double.TryParse(TriggerTag.Value, out double triggerTagValue))
            {
                if (IsNormal)
                {
                    if (triggerTagValue == triggerValue)
                    {
                        IsNormal = false;
                        var oldState = AlarmState;
                        AlarmState = AlarmState.Incoming;
                        AlarmStateChanged?.Invoke(this, new AlarmStateChangedEventArgs(this, oldState, AlarmState, triggerTagValue.ToString(), triggerValue.ToString(), "Equal"));
                    }
                }
                else
                {
                    if (triggerTagValue != triggerValue)
                    {
                        IsNormal = true;
                        var oldState = AlarmState;
                        AlarmState = AlarmState.Outgoing;
                        AlarmStateChanged?.Invoke(this, new AlarmStateChangedEventArgs(this, oldState, AlarmState, triggerTagValue.ToString(), triggerValue.ToString(), "Equal"));
                    }
                }
            }
        }
    }

    
}
