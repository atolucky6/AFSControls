using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyScada.Core
{
    public class AlarmSetting
    {
        [Category("Easy Scada")]
        public bool Enabled { get; set; } = true;

        [Category("Easy Scada"), TypeConverter(typeof(CollectionEditor)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public UniqueItemCollection<AlarmClass> AlarmClasses { get; set; }

        [Category("Easy Scada"), TypeConverter(typeof(CollectionEditor)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public UniqueItemCollection<AlarmGroup> AlarmGroups { get; set; }

        [Category("Easy Scada"), TypeConverter(typeof(CollectionEditor)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public AlarmItemCollection<DiscreteAlarm> DiscreteAlarms { get; set; }

        [Category("Easy Scada"), TypeConverter(typeof(CollectionEditor)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public AlarmItemCollection<AnalogAlarm> AnalogAlarms { get; set; }

        [Category("Easy Scada"), TypeConverter(typeof(CollectionEditor)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public AlarmItemCollection<QualityAlarm> QualityAlarms { get; set; }

        [Category("Easy Scada"), TypeConverter(typeof(CollectionEditor)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public UniqueItemCollection<EmailSetting> EmailSettings { get; set; }

        [Category("Easy Scada"), TypeConverter(typeof(CollectionEditor)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public UniqueItemCollection<SMSSetting> SMSSettings { get; set; }

        [JsonConstructor]
        public AlarmSetting()
        {
            AlarmClasses = new UniqueItemCollection<AlarmClass>();
            if (!AlarmClasses.Any(x => x.Name == "Errors"))
                AlarmClasses.Add(new AlarmClass() { ReadOnly = true, Enabled = true, Name = "Errors" });
            if (!AlarmClasses.Any(x => x.Name == "Warnings"))
                AlarmClasses.Add(new AlarmClass() { ReadOnly = true, Enabled = true, Name = "Warnings", BackColorIncoming = "#FF7C4D" });
            if (!AlarmClasses.Any(x => x.Name == "Systems"))
                AlarmClasses.Add(new AlarmClass() { ReadOnly = true, Enabled = true, Name = "Systems" });


            AlarmGroups = new UniqueItemCollection<AlarmGroup>();
            if (!AlarmGroups.Any(x => x.Name == "Default"))
                AlarmGroups.Add(new AlarmGroup() { ReadOnly = true, Enabled = true, Name = "Default" });


            DiscreteAlarms = new AlarmItemCollection<DiscreteAlarm>(this);
            AnalogAlarms = new AlarmItemCollection<AnalogAlarm>(this);
            QualityAlarms = new AlarmItemCollection<QualityAlarm>(this);

            EmailSettings = new UniqueItemCollection<EmailSetting>();
            if (!EmailSettings.Any(x => x.Name == "Default"))
                EmailSettings.Add(new EmailSetting() { ReadOnly = true, Enabled = true, Name = "Default" });
            SMSSettings = new UniqueItemCollection<SMSSetting>();
            if (!SMSSettings.Any(x => x.Name == "Default"))
                SMSSettings.Add(new SMSSetting() { ReadOnly = true, Enabled = true, Name = "Default" });
        }

        public event EventHandler<AlarmStateChangedEventArgs> AlarmStateChagned;

        internal void RaiseAlarmStateChanged(object sender, AlarmStateChangedEventArgs e)
        {
            AlarmStateChagned?.Invoke(sender, e);
        }
    }
}
