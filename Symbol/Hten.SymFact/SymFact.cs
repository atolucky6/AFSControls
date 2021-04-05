using System.IO;
using System.Xml;
using System.Windows.Markup;
//using XamlHelper;
using System.Reflection;

using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System;
using System.Drawing;
using vGraphic;
//using Hmi.SymbolLib;

using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Resources;
using System.Collections;
//using vSymbolfactory.XamlHelper;
using XamlHelper;
using PropertyCode;
using PropertyCode;
using Microsoft.VisualBasic.Compatibility.VB6;

namespace vGraphic
{
    [Designer(typeof(SymFactDesigner))]
    public partial class SymFact : UserControl//: GraphicComponent, IWarningNotify
    {
        string objectSelect = "Pumps___Cool_pump";
        Color fillColor = Color.Green;
        FlipType flip = FlipType.None;
        Rotate rotate = Rotate.None;
        FillMode fillMode = FillMode.Original;

        public void FireChanging()
        {
            IComponentChangeService service = GetComponentChangeService();
            if (service != null)
                service.OnComponentChanging(this, null);
        }

        public void FireChanged()
        {
            IComponentChangeService service = GetComponentChangeService();
            if (service != null)
                service.OnComponentChanged(this, null, null, null);
        }

        IComponentChangeService GetComponentChangeService()
        {
            return GetService(typeof(IComponentChangeService)) as IComponentChangeService;
        }


        private string scadaAutoCode = @"";
        [BrowsableAttribute(true)]
        [EditorAttribute(typeof(PropertyEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string ScadaAutoCode
        {
            get
            {
                return scadaAutoCode;
                //if (!this.DesignMode)
                //{
                //    return scadaAutoCode.Replace("[this.Name]", this.Name).Replace("[this.TagName]", this.TagName);
                //}
                //else
                //{
                //    return scadaAutoCode;
                //}
            }
            set { scadaAutoCode = value; }
        }


        public string ObjectSelect { get => objectSelect; set { objectSelect = value; Invalidate(); } }
        public Color FillColor { get => fillColor; set { fillColor = value; Invalidate(); } }
        public FlipType Flip { get => flip; set { flip = value; Invalidate(); } }
        public Rotate Rotate { get => rotate; set { rotate = value; Invalidate(); } }
        public FillMode FillMode { get => fillMode; set { fillMode = value; Invalidate(); } }

        public SymFact()
        {
            // Set the styles for drawing
            this.SetStyle(ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            this.BackColor = System.Drawing.Color.Transparent;
            this.Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            this.BackColor = System.Drawing.Color.Transparent;
            this.Region = XamlHelper.XamlHelper.BitmapShade(e.Graphics, objectSelect, this.Size, Helper.getdata(objectSelect), flip, rotate, fillMode, fillColor);
        }
    }

    public class SymFactDesigner : ControlDesigner
    {
        private DesignerActionListCollection actionLists;
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (actionLists == null)
                {
                    actionLists = new DesignerActionListCollection();
                    actionLists.Add(new SymFactListItem(this));
                }
                return actionLists;
            }
        }
    }
    public class SymFactListItem : DesignerActionList
    {
        private SymFact mycontrol;

        public SymFactListItem(SymFactDesigner owner): base(owner.Component)
        {
            mycontrol = (SymFact)owner.Component;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var items = new DesignerActionItemCollection();
            items.Add(new DesignerActionTextItem(".Net HMI", "Category1"));
            items.Add(new DesignerActionMethodItem(this, "frmProperty", "frmProperty", "Category1"));
            items.Add(new DesignerActionMethodItem(this, "frmGraphic", "frmGraphic", "Category1"));
            items.Add(new DesignerActionPropertyItem("Flip", "Flip", "Category2"));
            items.Add(new DesignerActionPropertyItem("Rotate", "Rotate", "Category2"));
            items.Add(new DesignerActionPropertyItem("FillMode", "FillMode", "Category2"));
            items.Add(new DesignerActionPropertyItem("FillColor", "FillColor", "Category2"));
            return items;
        }

        public Control Parent
        {
            get { return ((SymFact)base.Component).Parent; }
            set { ((SymFact)base.Component).Parent = value; }
        }

        public string Code
        {
            get { return ((SymFact)base.Component).ScadaAutoCode; }
            set { ((SymFact)base.Component).ScadaAutoCode = value; }
        }

        public void frmGraphic()
        {
            frm_Graphic frm = new frm_Graphic(ObjectSelect, Flip, Rotate, FillMode, FillColor);
            frm.OnSubmit += Frm_OnSubmit;
            frm.ShowDialog();
        }
        private void Frm_OnSubmit(string objectSelect, FlipType flip, Rotate rotate, FillMode fillMode, Color fillColor)
        {
            this.ObjectSelect = objectSelect;
            this.Flip = flip;
            this.Rotate = rotate;// Rotate.None; Rotate.Rot_90; Rotate.None; Color.Lime;
            this.FillMode = fillMode;
            this.FillColor = fillColor;
        }

        public void frmProperty()
        {
            
             frmProperties frm = new frmProperties(mycontrol, mycontrol.ScadaAutoCode);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                ((SymFact)base.Component).ScadaAutoCode = frmProperties.CodeReturn;
            }

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


        public FlipType Flip
        {
            get { return ((SymFact)base.Component).Flip; }
            set { ((SymFact)base.Component).Flip = value; }
        }

        public Rotate Rotate
        {
            get { return ((SymFact)base.Component).Rotate; }
            set { ((SymFact)base.Component).Rotate = value; }
        }

        public FillMode FillMode
        {
            get { return ((SymFact)base.Component).FillMode; }
            set { ((SymFact)base.Component).FillMode = value; }
        }

        public Color FillColor
        {
            get { return ((SymFact)base.Component).FillColor; }
            set { ((SymFact)base.Component).FillColor = value; }
        }

        public string ObjectSelect
        {
            get { return ((SymFact)base.Component).ObjectSelect; }
            set { ((SymFact)base.Component).ObjectSelect = value; }
        }

    }
    public class Dir
    {
        public string dirname;
        public Dictionary<string, string> dicsym = new Dictionary<string, string>();
    }

}
