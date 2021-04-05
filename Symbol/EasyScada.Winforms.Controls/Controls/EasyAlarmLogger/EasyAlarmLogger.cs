using EasyScada.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyScada.Winforms.Controls
{
    [Designer(typeof(EasyAlarmLoggerDesginer))]
    public partial class EasyAlarmLogger : Component, ISupportInitialize
    {
        #region Constructors
        public EasyAlarmLogger()
        {
            InitializeComponent();
        }

        public EasyAlarmLogger(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }
        #endregion

        #region Fields
        private bool enabled;
        private LogProfileCollection profiles = new LogProfileCollection();
        private LogColumnCollection columns = new LogColumnCollection();
        private AlarmSetting alarmSetting;
        private Task refreshTask;
        private bool isDisposed;
        #endregion

        #region Properties
        [Browsable(false)]
        [Category("Easy Scada"), TypeConverter(typeof(CollectionEditor)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public LogProfileCollection Databases { get => profiles; }

        [Category("Easy Scada")]
        public bool Enabled
        {
            get => enabled;
            set => enabled = value;
        }
        #endregion

        #region Events
        public event EventHandler<AlarmStateChangedEventArgs> AlarmStateChanged;
        #endregion

        #region ISupportInitialize
        public void BeginInit()
        {
        }

        public void EndInit()
        {
            if (!DesignMode)
            {
                if (refreshTask == null)
                {
                    columns = new LogColumnCollection();
                    columns.Add(new LogColumn() { ColumnName = "Name" });
                    columns.Add(new LogColumn() { ColumnName = "AlarmText" });
                    columns.Add(new LogColumn() { ColumnName = "AlarmClass" });
                    columns.Add(new LogColumn() { ColumnName = "AlarmGroup" });
                    columns.Add(new LogColumn() { ColumnName = "TriggerTag" });
                    columns.Add(new LogColumn() { ColumnName = "Value" });
                    columns.Add(new LogColumn() { ColumnName = "Limit" });
                    columns.Add(new LogColumn() { ColumnName = "CompareMode" });
                    columns.Add(new LogColumn() { ColumnName = "State" });
                    columns.Add(new LogColumn() { ColumnName = "OutgoingTime" });
                    columns.Add(new LogColumn() { ColumnName = "AckTime" });
                    columns.Add(new LogColumn() { ColumnName = "AlarmType" });

                    Disposed += OnDisposed;
                    alarmSetting = DesignerHelper.GetAlarmSetting(null);
                    if (alarmSetting != null)
                    {
                        alarmSetting.Enabled = enabled;
                        alarmSetting.AlarmStateChagned += OnAlarmStateChanged;
                        refreshTask = Task.Factory.StartNew(RefreshAlarm, TaskCreationOptions.LongRunning);
                    }
                }
            }
        }

        #endregion

        #region Methods
        private void OnDisposed(object sender, EventArgs e)
        {
            isDisposed = true;
        }

        private void OnAlarmStateChanged(object sender, AlarmStateChangedEventArgs e)
        {
            try
            {
                if (e.AlarmItem != null)
                {
                    if (e.AlarmItem.EmailSetting != null)
                    {

                    }

                    if (e.AlarmItem.SMSSetting != null)
                    {

                    }

                    string alarmTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string alarmType = "";
                    if (e.AlarmItem is DiscreteAlarm)
                    {
                        alarmType = "DiscreteAlarm";
                    }
                    else if (e.AlarmItem is AnalogAlarm)
                    {
                        alarmType = "AnalogAlarm";
                    }
                    else if (e.AlarmItem is QualityAlarm)
                    {
                        alarmType = "QualityAlarm";
                    }

                    foreach (LogProfile profile in profiles)
                    {
                        try
                        {
                            profile.GetCommand(out DbConnection conn, out DbCommand cmd, true);
                            conn.Open();

                            if (e.NewState == AlarmState.Incoming)
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append($"insert into {profile.TableName} ");
                                sb.Append("(`DateTime`, `Name`, `AlarmText`, `AlarmClass`, `AlarmGroup`, `TriggerTag`, `Value`, `Limit`, `CompareMode`, `State`, `AlarmType`) values (");
                                sb.Append($"'{alarmTime}', '{e.AlarmItem.Name}', '{e.AlarmItem.AlarmText}', '{e.AlarmItem.AlarmClassName}', '{e.AlarmItem.AlarmGroupName}', '{e.AlarmItem.TriggerTagPath}', ");
                                sb.Append($"'{e.Value}', '{e.Limit}', '{e.CompareMode}', 'In', '{alarmType}')");

                                cmd.CommandText = sb.ToString();
                                cmd.ExecuteNonQuery();
                            }
                            else if (e.NewState == AlarmState.Outgoing)
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append($"update {profile.TableName} ");
                                sb.Append($"set OutgoingTime = '{alarmTime}', State = 'Out' where ");
                                sb.Append($"Name = '{e.AlarmItem.Name}' and AlarmType = '{alarmType}' and State = 'In'");

                                cmd.CommandText = sb.ToString();
                                cmd.ExecuteNonQuery();
                            }

                            conn.Close();
                            conn.Dispose();
                            cmd.Dispose();
                        }
                        catch { }
                    }
                }
            }
            catch { }
            AlarmStateChanged?.Invoke(sender, e);
        }

        private void RefreshAlarm()
        {
            Dictionary<LogProfile, int> createSchemaResult = new Dictionary<LogProfile, int>();
            Dictionary<LogProfile, int> createTableResult = new Dictionary<LogProfile, int>();

            LogColumn[] columns = this.columns.ToArray();

            while (!isDisposed)
            {
                try
                {
                    foreach (LogProfile profile in profiles)
                    {
                        try
                        {
                            bool needCreateSchema = false;
                            if (!createSchemaResult.ContainsKey(profile))
                            {
                                needCreateSchema = true;
                            }
                            else
                            {
                                if (createSchemaResult[profile] != 1)
                                    needCreateSchema = true;
                            }

                            if (needCreateSchema)
                            {
                                // Create database schema if not exists
                                profile.GetCommand(out DbConnection conn, out DbCommand cmd, false);
                                conn.Open();
                                cmd.CommandText = profile.GetCreateSchemaQuery();
                                createSchemaResult[profile] = cmd.ExecuteNonQuery();
                                conn.Close();
                                conn.Dispose();
                                cmd.Dispose();
                            }

                            bool needCreateTable = false;
                            if (!createTableResult.ContainsKey(profile))
                            {
                                needCreateTable = true;
                            }
                            else
                            {
                                if (createTableResult[profile] != 1)
                                    needCreateTable = true;
                            }

                            if (needCreateTable)
                            {
                                // Create table if not exists
                                profile.GetCommand(out DbConnection conn, out DbCommand cmd, out DbDataAdapter adp, true);
                                conn.Open();
                                cmd.CommandText = profile.GetCreateTableQuery(columns);
                                int createTableRes = cmd.ExecuteNonQuery();
                                createTableResult[profile] = createTableRes;
                                // Create table result = 0. It means table already exists
                                if (createTableRes == 0)
                                {
                                    // We need to check the columns name
                                    cmd.CommandText = profile.GetSelectQuery(1);
                                    adp.SelectCommand = cmd;
                                    DataTable dt = new DataTable();
                                    adp.Fill(dt);
                                }
                            }
                        }
                        catch { }
                    }

                    if (Enabled)
                    {
                        foreach (var item in alarmSetting.DiscreteAlarms)
                        {
                            if (item.Enabled)
                                item.Refresh();
                        }

                        foreach (var item in alarmSetting.AnalogAlarms)
                        {
                            if (item.Enabled)
                                item.Refresh();
                        }

                        foreach (var item in alarmSetting.QualityAlarms)
                        {
                            if (item.Enabled)
                                item.Refresh();
                        }
                    }
                }
                catch { }
                finally
                {
                    Thread.Sleep(10);
                }
            }
        }
        #endregion
    }
}
