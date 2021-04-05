using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyScada.Core
{
    public class AnalogAlarm : AlarmItemBase
    {
        public string Limit { get; set; }
        public LimitMode LimitMode { get; set; }
        public int Delay { get; set; }
        public TimeUnit TimeUnit { get; set; }
        public DeadbandMode DeadbandMode { get; set; }
        public string DeadbandValue { get; set; }
        public bool DeadbandInPercentage { get; set; }
        public override event EventHandler<AlarmStateChangedEventArgs> AlarmStateChanged;

        public override void Refresh()
        {
            base.Refresh();
            if (TriggerTag != null &&
                TriggerTag.Quality == Quality.Good &&
                double.TryParse(Limit, out double limit) &&
                double.TryParse(TriggerTag.Value, out double triggerTagValue))
            {
                double deadBandValue = 0;
                if (DeadbandMode != DeadbandMode.Off)
                {
                    if (double.TryParse(DeadbandValue, out deadBandValue))
                    {
                        if (DeadbandInPercentage)
                            deadBandValue = limit * deadBandValue / 100;
                    }
                }

                if (IsNormal)
                {
                    
                    if (DeadbandMode == DeadbandMode.OnIncoming ||
                        DeadbandMode == DeadbandMode.OnIncomingAndOutgoing)
                    {
                        switch (LimitMode)
                        {
                            case LimitMode.Higher:
                                limit += deadBandValue;
                                break;
                            case LimitMode.Lower:
                                limit -= deadBandValue;
                                break;
                            default:
                                break;
                        }
                    }

                    switch (LimitMode)
                    {
                        case LimitMode.Higher:
                            if (triggerTagValue > limit)
                            {
                                IsNormal = false;
                                var oldState = AlarmState;
                                AlarmState = AlarmState.Incoming;
                                AlarmStateChanged?.Invoke(this, new AlarmStateChangedEventArgs(this, oldState, AlarmState, triggerTagValue.ToString(), Limit, "Higher"));
                            }
                            break;
                        case LimitMode.Lower:
                            if (triggerTagValue < limit)
                            {
                                IsNormal = false;
                                var oldState = AlarmState;
                                AlarmState = AlarmState.Incoming;
                                AlarmStateChanged?.Invoke(this, new AlarmStateChangedEventArgs(this, oldState, AlarmState, triggerTagValue.ToString(), Limit, "Lower"));
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (DeadbandMode == DeadbandMode.OnOutgoing ||
                        DeadbandMode == DeadbandMode.OnIncomingAndOutgoing)
                    {
                        switch (LimitMode)
                        {
                            case LimitMode.Higher:
                                limit -= deadBandValue;
                                break;
                            case LimitMode.Lower:
                                limit += deadBandValue;
                                break;
                            default:
                                break;
                        }
                    }

                    switch (LimitMode)
                    {
                        case LimitMode.Higher:
                            if (triggerTagValue < limit)
                            {
                                IsNormal = true;
                                var oldState = AlarmState;
                                AlarmState = AlarmState.Outgoing;
                                AlarmStateChanged?.Invoke(this, new AlarmStateChangedEventArgs(this, oldState, AlarmState, triggerTagValue.ToString(), Limit, "Higher"));
                            }
                            break;
                        case LimitMode.Lower:
                            if (triggerTagValue > limit)
                            {
                                IsNormal = true;
                                var oldState = AlarmState;
                                AlarmState = AlarmState.Outgoing;
                                AlarmStateChanged?.Invoke(this, new AlarmStateChangedEventArgs(this, oldState, AlarmState, triggerTagValue.ToString(), Limit, "Lower"));
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
