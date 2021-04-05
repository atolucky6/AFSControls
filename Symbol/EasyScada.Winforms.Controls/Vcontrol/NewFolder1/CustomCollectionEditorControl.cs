using FastColoredTextBoxNS;
using NCalc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyScada.Winforms.Controls
{
    public partial class CustomCollectionEditorControl : UserControl
    {
        public bool IsScreen = true;

        public delegate void EventSelect(string expression, IList listResult);
        public event EventSelect OnSelect;
        public Type _Type = typeof(object);

        #region "Variables"
        public object RefObject;
        public delegate void InstanceEventHandler(object sender, object instance);
        public event InstanceEventHandler InstanceCreated;
        public event InstanceEventHandler DestroyingInstance;
        public event InstanceEventHandler ItemRemoved;
        public event InstanceEventHandler ItemAdded;
        private IList _Collection = null;
        private Type lastItemType = null;
        private ArrayList backupList = null;
        //
        List<string> ListTag = new List<string>();
        string[] keywords = { "Sqrt()", "Abs()", "Acos()", "Asin()", "Atan()",
                              "Ceiling()", "Cos()", "Log(,)", "Log10()", "Max(,)",
                              "Min(,)", "Pow(,)", "Round(,)", "Truncate()",
                              "Sin()", "Tan()", "Sign()", "Floor()"};
        public IList Collection
        {
            get { return _Collection; }
            set
            {
                _Collection = value;
                backupList = new ArrayList(value);
                RefreshValues();
            }
        }
        #endregion;

        public CustomCollectionEditorControl(bool IsScreen)
        {
            InitializeComponent();
            this.IsScreen = IsScreen;
        }

        /// <summary>
		/// Gets the data type of each item in the collection.
		/// </summary>
		/// <param name="coll"> The collection for which to get the item's type</param>
		/// <returns>The data type of the collection items.</returns>
		protected virtual Type GetItemType(IList coll)
        {
            PropertyInfo pi = coll.GetType().GetProperty("Item", new Type[] { typeof(int) });
            return pi.PropertyType;
        }

        /// <summary>
        /// Gets the data types that this collection editor can contain
        /// </summary>
        /// <param name="coll">The collection for which to return the available types</param>
        /// <returns>An array of data types that this collection can contain.</returns>
        protected virtual Type[] CreateNewItemTypes(IList coll)
        {
            return new Type[] { GetItemType(coll) };
        }

        /// <summary>
        /// Creates a new instance of the specified collection item type.
        /// </summary>
        /// <param name="itemType">The type of item to create. </param>
        /// <returns>A new instance of the specified object.</returns>
        protected virtual object CreateInstance(Type itemType)
        {

            /* 
			//This is just another way of how to remotely  create an object
			
			// Try to get a parameterless constructor
			ConstructorInfo ci = itemType.GetConstructor(new Type[0]);
			InstanceDescriptor id =  new InstanceDescriptor(ci,null,false);
			return id.Invoke();
			*/


            object instance = Activator.CreateInstance(itemType, true);
            OnInstanceCreated(instance);
            return instance;
        }

        /// <summary>
        /// Destroys the specified instance of the object.
        /// </summary>
        /// <param name="instance">The object to destroy. </param>
        protected virtual void DestroyInstance(object instance)
        {
            OnDestroyingInstance(instance);
            if (instance is IDisposable) { ((IDisposable)instance).Dispose(); }
            instance = null;
        }

        protected virtual void OnDestroyingInstance(object instance)
        {
            if (DestroyingInstance != null)
            {
                DestroyingInstance(this, instance);
            }
        }

        protected virtual void OnInstanceCreated(object instance)
        {
            if (InstanceCreated != null)
            {
                InstanceCreated(this, instance);
            }
        }

        protected virtual void OnItemRemoved(object item)
        {
            if (ItemRemoved != null)
            {
                ItemRemoved(this, item);
            }
        }

        protected virtual void OnItemAdded(object Item)
        {
            if (ItemAdded != null)
            {
                ItemAdded(this, Item);
            }
        }

        protected internal TItem[] GenerateTItemArray(IList collection)
        {
            TItem[] ti = new TItem[0];

            if (collection != null && collection.Count > 0)
            {
                ti = new TItem[collection.Count];

                for (int i = 0; i < collection.Count; i++)
                {
                    ti[i] = CreateTItem(collection[i]);
                }
            }
            return ti;
        }

        /// <summary>
        /// Creates a new TItem objectfor a collection item.
        /// </summary>
        /// <param name="reffObject">The collection item for wich to create an TItem object.</param>
        /// <returns>A TItem object referencing the collection item received as parameter.</returns>
        protected virtual TItem CreateTItem(object reffObject)
        {
            TItem ti = new TItem(this, reffObject);
            SetProperties(ti, reffObject);
            return ti;
        }

        /// <summary>
		/// When implemented by a class, customize a TItem object in respect to it's corresponding collection item.
		/// </summary>
		/// <param name="titem">The TItem object to be customized in respect to it's corresponding collection item.</param>
		/// <param name="reffObject">The collection item for which it customizes the TItem object.</param>
		protected virtual void SetProperties(TItem titem, object reffObject)
        {
            // set the display name 
            PropertyInfo pi = titem.Value.GetType().GetProperty("Value");

            if (pi != null)
            {
                titem.Text = ""+ (titem.Value == null?"null":pi.GetValue(titem.Value, null)==null?"null": pi.GetValue(titem.Value, null).ToString());
            }
            else
            {
                titem.Text = titem.Value.GetType().Name;
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            tv_Items.BeginUpdate();
            if (Collection != null)
            {
                //create a new item to add to the Collection and a corespondent TItem to add to the treeview nodes
                Type type = _Type;
                object newCollItem = CreateInstance(type);
                TItem newTItem = CreateTItem(newCollItem);

                //get the current  possition  and the parent collections to insert into
                TItem selTItem = (TItem)tv_Items.SelectedNode;

                //if (selTItem != null)
                //{
                //    int position = selTItem.Index + 1;

                //    IList coll;
                //    TreeNodeCollection TItemColl;

                //    if (selTItem.Parent != null)
                //    {
                //        coll = (((TItem)selTItem.Parent).SubItems);
                //        TItemColl = selTItem.Parent.Nodes;
                //    }
                //    else
                //    {
                //        coll = Collection;
                //        TItemColl = tv_Items.Nodes;
                //    }


                //    coll.Insert(position, newCollItem);
                //    TItemColl.Insert(position, newTItem);


                //}
                //else //empty collection
                //{
                    Collection.Add(newCollItem);
                    tv_Items.Nodes.Add(newTItem);

                //}

                OnItemAdded(newCollItem);
                tv_Items.SelectedNode = newTItem;

            }
            tv_Items.EndUpdate();
            if (OnSelect != null)
            {
                OnSelect(SettingText.Text, _Collection);
            }
        }

        protected virtual void RefreshValues()
        {
            tv_Items.BeginUpdate();
            tv_Items.Nodes.Clear();
            tv_Items.Nodes.AddRange(GenerateTItemArray(this.Collection));

            tv_Items.EndUpdate();
        }

        private void tv_Items_BeforeSelect(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
        {
            pg_PropGrid.SelectedObject = ((TItem)e.Node).Value;
        }

        private void tv_Items_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            TItem ti = (TItem)e.Node;
            if (ti.Value.GetType() != lastItemType)
            {
                lastItemType = ti.Value.GetType();
                IList coll;
                if (ti.Parent != null) { coll = ((TItem)ti.Parent).SubItems; }
                else { coll = Collection; }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //show tag
            //frmTag frm = new frmTag();
            //frm.OnSubmit += Frm_OnSelect;
            //frm.ShowDialog();
        }

        private void Frm_OnSelect(string tagfullpath)
        {
            SettingText.Text += "{" + tagfullpath.Replace("\"","") + "}";
            if (OnSelect != null)
            {
                OnSelect(SettingText.Text, _Collection);
            }
        }

        private void button5_Click(object sender, EventArgs ex)
        {
            List<TagSetting> ListTagSetting = ExtractFromString(SettingText.Text, "{", "}");
            string Ncalc = SettingText.Text;
            if (string.IsNullOrEmpty(Ncalc) || string.IsNullOrWhiteSpace(Ncalc))
            {
                MessageBox.Show("Error--> Expression can not be null or empty.");
                return;
            }
            foreach (TagSetting tag in ListTagSetting)
            {
                Ncalc = Ncalc.Replace("{" + tag.TagFullPath + "}", tag.TagValue);
            }

            NCalc.Expression e = new NCalc.Expression(Ncalc, (NCalc.EvaluateOptions)EvaluateOptions.NoCache);
            if (!e.HasErrors() && !Ncalc.Contains("{") && !Ncalc.Contains("}"))
            {
                MessageBox.Show("No Error!");
            }
            else
            {
                if (e.Error.Contains("missing EOF at ':'"))
                {
                    string a = e.Error;
                    a = e.Error.Split(':')[1];
                    MessageBox.Show("Error--> Format {TagName} " + a);
                }
                else
                {
                    MessageBox.Show("Error--> " + e.Error.Split(':')[0]);
                }
            }
        }

        public List<TagSetting> ExtractFromString(string text, string startString, string endString)
        {
            List<TagSetting> matched = new List<TagSetting>();
            int indexStart = 0, indexEnd = 0;
            bool exit = false;
            while (!exit)
            {
                indexStart = text.IndexOf(startString);
                indexEnd = text.IndexOf(endString);
                if (indexStart != -1 && indexEnd != -1)
                {
                    TagSetting tag = new TagSetting();
                    tag.TagFullPath = text.Substring(indexStart + startString.Length,
                        indexEnd - indexStart - startString.Length);
                    tag.TagValue = "0";
                    matched.Add(tag);
                    text = text.Substring(indexEnd + endString.Length);
                }
                else
                    exit = true;
            }
            return matched;
        }

        private void CustomCollectionEditorForm_Load(object sender, EventArgs e)
        {
            //if (!IsScreen)
            //{
            //    button4.Enabled = false;
            //}
            RefreshValues();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            tv_Items.BeginUpdate();
            TItem selTItem = (TItem)tv_Items.SelectedNode;
            if (selTItem != null)
            {
                int selIndex = selTItem.Index;
                TItem parentTitem = (TItem)selTItem.Parent;
                if (parentTitem != null)
                {
                    parentTitem.Nodes.Remove(selTItem);
                    parentTitem.SubItems.Remove(selTItem.Value);
                    if (parentTitem.Nodes.Count > selIndex) { tv_Items.SelectedNode = parentTitem.Nodes[selIndex]; }
                    else if (parentTitem.Nodes.Count > 0) { tv_Items.SelectedNode = parentTitem.Nodes[selIndex - 1]; }
                    else { tv_Items.SelectedNode = parentTitem; }
                }
                else
                {
                    tv_Items.Nodes.Remove(selTItem);
                    Collection.Remove(selTItem.Value);
                    if (tv_Items.Nodes.Count > selIndex) { tv_Items.SelectedNode = tv_Items.Nodes[selIndex]; }
                    else if (tv_Items.Nodes.Count > 0) { tv_Items.SelectedNode = tv_Items.Nodes[selIndex - 1]; }
                    else { this.pg_PropGrid.SelectedObject = null; }
                }

                OnItemRemoved(selTItem.Value);
            }
            tv_Items.EndUpdate();

            if (OnSelect != null)
            {
                OnSelect(SettingText.Text, _Collection);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (OnSelect != null)
            {
                OnSelect(SettingText.Text, _Collection);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UndoChanges(backupList, Collection);
            RefreshValues();
        }

        private void UndoChanges(IList source, IList dest)
        {
            dest.Clear();
            CopyItems(source, dest);
        }

        private void CopyItems(IList source, IList dest)
        {
            foreach (object o in source)
            {
                dest.Add(o);
                OnItemAdded(o);
            }
        }

        private void pg_PropGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if(e.ChangedItem.PropertyDescriptor.Name == "Value")
            {
                tv_Items.SelectedNode.Text = e.ChangedItem.Value.ToString();
            }

            if (OnSelect != null)
            {
                OnSelect(SettingText.Text, _Collection);
            }
        }

        private void SettingText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (OnSelect != null)
            {
                OnSelect(SettingText.Text, _Collection);
            }
        }
    }


    public class TagSetting
    {
        public string TagFullPath;
        public string TagValue;
    }
    public class TItem : TreeNode
    {
        private object _Value;
        private CustomCollectionEditorControl cedt = null;
        private IList _SubItems = null;


        public object Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        public IList SubItems
        {
            get { return _SubItems; }
            set
            {
                _SubItems = value;
                this.Nodes.Clear();
            }
        }

        public TItem(CustomCollectionEditorControl cedt, object Value)
        {
            this.cedt = cedt;
            this._Value = Value;
        }


        public TItem(CustomCollectionEditorControl cedt, object Value, int ImageIndex)
        {
            this.cedt = cedt;
            this._Value = Value;
            this.ImageIndex = ImageIndex;
        }
        public TItem(CustomCollectionEditorControl cedt, object Value, int ImageIndex, int SelectedImageIndex)
        {
            this.cedt = cedt;
            this._Value = Value;
            this.ImageIndex = ImageIndex;
            this.SelectedImageIndex = SelectedImageIndex;
        }
    }

}
