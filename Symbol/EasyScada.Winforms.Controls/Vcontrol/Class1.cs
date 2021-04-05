
using EasyScada.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using NCalc;
using Newtonsoft.Json;
using System.Security.AccessControl;

namespace EasyScada.Winforms.Controls
{
    [Designer(typeof(Class1Designer))]
    public class Class1 : TextBox, ISupportConnector, ISupportTag, ISupportWriteSingleTag, ISupportInitialize
    {
        static Control controlStatup = null;

        #region ISupportConnector
        [Description("Select driver connector for control")]
        [Browsable(true), Category(DesignerCategory.EASYSCADA)]
        public IEasyDriverConnector Connector => EasyDriverConnectorProvider.GetEasyDriverConnector();
        #endregion

        #region ISupportTag
        [Description("Select patah to tag for control")]
        [Browsable(true), Category(DesignerCategory.EASYSCADA)]
        [Editor(typeof(PathToTagPropertyEditor), typeof(UITypeEditor))]
        public string TagPath { get; set; }

        ITag linkedTag;
        [Browsable(false)]
        public ITag LinkedTag
        {
            get
            {
                if (linkedTag == null)
                {
                    linkedTag = Connector?.GetTag(TagPath);
                }
                else
                {
                    if (linkedTag.Path != TagPath)
                        linkedTag = Connector?.GetTag(TagPath);
                }
                return linkedTag;
            }
        }
        #endregion

        #region ISupportWriteTag

        [Browsable(true), Category(DesignerCategory.EASYSCADA)]
        public WriteTrigger WriteTrigger { get; set; }

        int writeDelay = 200;
        [Browsable(true), Category(DesignerCategory.EASYSCADA)]
        public int WriteDelay
        {
            get { return writeDelay < 0 ? 0 : writeDelay; }
            set
            {
                writeDelay = value;
                writeDelayTimer.Interval = WriteDelay;
            }
        }


        public event EventHandler<TagWritingEventArgs> TagWriting;
        public event EventHandler<TagWritedEventArgs> TagWrited;
        #endregion

        #region ISupportInitialize
        public void BeginInit()
        {
        }

        public void EndInit()
        {
            if (!DesignMode)
            {
                if (Connector.IsStarted)
                    OnConnectorStarted(null, EventArgs.Empty);
                else
                    Connector.Started += OnConnectorStarted;
            }
        }
        #endregion

        #region Constructors
        public Class1() : base()
        {
            controlStatup = this;
            writeDelayTimer.Elapsed += WriteDelayTimer_Elapsed;
            Text = "";
        }
        #endregion

        #region Fields
        private System.Timers.Timer writeDelayTimer = new System.Timers.Timer();
        private string writeValue;
        private string oldValue;
        #endregion

        #region Properties



        #endregion

        #region Event handlers
        private void OnConnectorStarted(object sender, EventArgs e)
        {
            ScadaAutoCode.Main(Connector,this);

            //if (LinkedTag != null)
            //{
            //    ////   OnTagValueChanged(LinkedTag, new TagValueChangedEventArgs(LinkedTag, null, LinkedTag.Value));
            //    ////   OnTagQualityChanged(LinkedTag, new TagQualityChangedEventArgs(LinkedTag, Quality.Uncertain, LinkedTag.Quality));


            //    //LinkedTag.ValueChanged += OnTagValueChanged;
            //    //LinkedTag.QualityChanged += OnTagQualityChanged;

            //    //this.SetInvoke((x) =>
            //    //{
            //    //    x.Text = LinkedTag.Value;
            //    //});
            //}
        }


        private void OnTagQualityChanged(object sender, TagQualityChangedEventArgs e)
        {

        }

        private void OnTagValueChanged(object sender, TagValueChangedEventArgs e)
        {
            this.SetInvoke((x) =>
            {
                if (x.Text != e.NewValue && !Focused)
                    x.Text = e.NewValue;
            });

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Enter)
            {
                if (WriteTrigger == WriteTrigger.OnEnter)
                {
                    writeDelayTimer.Stop();
                    writeDelayTimer.Start();
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (LinkedTag != null && LinkedTag.Value != Text)
                    Text = LinkedTag.Value;
            }
        }


        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            oldValue = Text;
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (WriteTrigger == WriteTrigger.LostFocus)
            {
                writeDelayTimer.Stop();
                writeDelayTimer.Start();
            }

            if (LinkedTag != null && LinkedTag.Value != Text)
                Text = LinkedTag.Value;
        }

        #endregion

        #region Methods
        private void WriteDelayTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            writeDelayTimer.Stop();
            writeValue = Text;
            WriteTag(writeValue);
        }

        private async void WriteTag(string writeValue)
        {
            if (Connector != null && Connector.IsStarted && LinkedTag != null && LinkedTag.Value != writeValue)//&& IsNumber(writeValue))
            {
                await Task.Delay(WriteDelay);

                TagWriting?.Invoke(this, new TagWritingEventArgs(LinkedTag, writeValue));
                var res = await LinkedTag.WriteAsync(writeValue);
                TagWrited?.Invoke(this, new TagWritedEventArgs(LinkedTag, res.IsSuccess ? Quality.Good : Quality.Bad, writeValue));
            }
        }

        #endregion

        //protected override void OnCreateControl()
        //{
        //    base.OnCreateControl();
        //    if (DesignMode) return;
        //    Form f = GetParentForm(this);
        //    if (f != null)
        //    {
        //        f.Load += ParentForm_FormLoad;

        //    }
        //}

        //private void ParentForm_FormLoad(object sender, EventArgs e)
        //{
        //    MessageBox.Show("fffffff");

        //}
        //private Form GetParentForm(Control parent)
        //{
        //    Form Result = parent as Form;
        //    if (Result == null)
        //    {
        //        if (parent != null)
        //        {
        //            // Recursive is cool
        //            return GetParentForm(parent.Parent);
        //        }
        //    }
        //    return Result;
        //}

    }

    internal class Class1Designer : ControlDesigner
    {
        //   private Class1 control;
        private DesignerActionListCollection actionLists;
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (actionLists == null)
                {
                    actionLists = new DesignerActionListCollection();
                    actionLists.Add(new Class1ListItem(this));
                }
                return actionLists;
            }
        }
    }

    internal class Class1ListItem : DesignerActionList
    {
        private Class1 control;

        public Class1ListItem(Class1Designer owner)
            : base(owner.Component)
        {
            control = (Class1)owner.Component;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var items = new DesignerActionItemCollection();
            items.Add(new DesignerActionTextItem(".Net HMI", "Category1"));
            //   items.Add(new DesignerActionMethodItem(this, "frmAddCode", "Assign Tag", "Category1"));
            items.Add(new DesignerActionMethodItem(this, "frmAutho", "Authorization", "Category1"));

            return items;
        }

        public Control Parent
        {
            get { return ((Class1)base.Component).Parent; }
            set { ((Class1)base.Component).Parent = value; }
        }

        //public string Code
        //{
        //    get { return ((Class1)base.Component).CodeRender; }
        //    set { ((Class1)base.Component).CodeRender = value; }
        //}

        //   public string Authorization { get => ((Class1)base.Component).Authorization; set => ((Class1)base.Component).Authorization = value; }

        public void frmAutho()
        {
            frmProperties frm = new frmProperties(control);
            frm.ShowDialog();
            //  control.Tag = frm.RefCOntrol.Tag;
            //   control.CodeRender = frmProperties.CodeRender;
        }

        private void Frm_OnSubmit(string Autho)
        {
            //  SetProperty(control, "Authorization", Autho);
        }

        public void SetProperty(Control control, string propertyName, object value)
        {
            PropertyDescriptor pd = TypeDescriptor.GetProperties(control)[propertyName];
            pd.SetValue(control, value);
        }

        public object GetProperty(Control control, string propertyName)
        {
            PropertyDescriptor pd = TypeDescriptor.GetProperties(control)[propertyName];
            return pd.GetValue(control);
        }
    }
}
