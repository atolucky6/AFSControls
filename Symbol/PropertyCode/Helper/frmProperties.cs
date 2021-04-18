
//using Hten.DataHelper;
using Newtonsoft.Json;
//using Hten.DesignSurfaceExt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PropertyCode
{
    public partial class frmProperties : Form
    {
        public bool IsScreen = true;
        public EventHandler ScreenChanged;

        private object refCOntrol = null;
        private string code = null;

        //public Control RefCOntrol { get => refCOntrol; set => refCOntrol = value; }

        Dictionary<string, object> FuncList = new Dictionary<string, object>();
        public frmProperties(object RefCOntrol1, string Code)
        {

            InitializeComponent();
            refCOntrol = RefCOntrol1;
            code = Code;

        }


        private void Properties_Load(object sender, EventArgs e)
        {
            TreeNode treeNode = new TreeNode();

            if (code == null)
            {
                MessageBox.Show("Tag nULL");
                FuncList = new Dictionary<string, object>();
            }
            else
            {
                FuncList = new Dictionary<string, object>();
                List<string> listSt = ExtractFromString(code, "[BEGIN]", "[END]");
                foreach (string s in listSt)
                {
                    string key = s.Split(new string[] { "[KEY]" }, StringSplitOptions.None)[0];
                    string valu = s.Split(new string[] { "[KEY]" }, StringSplitOptions.None)[1];

                    if (key.Contains("ColorSetting"))
                    {
                        ColorSetting obj = JsonConvert.DeserializeObject<ColorSetting>(valu);
                        FuncList.Add(key.Split('_')[0], obj);
                    }
                    if (key.Contains("StringSetting"))
                    {
                        StringSetting obj = JsonConvert.DeserializeObject<StringSetting>(valu);
                        FuncList.Add(key.Split('_')[0], obj);
                    }
                    if (key.Contains("BoolSetting"))
                    {
                        BoolSetting obj = JsonConvert.DeserializeObject<BoolSetting>(valu);
                        FuncList.Add(key.Split('_')[0], obj);
                    }
                    if (key.Contains("IntSetting"))
                    {
                        IntSetting obj = JsonConvert.DeserializeObject<IntSetting>(valu);
                        FuncList.Add(key.Split('_')[0], obj);
                    }
                }
            }


            Attribute[] propertyAttributes = new Attribute[] { DesignOnlyAttribute.No };
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(refCOntrol, propertyAttributes);
            // PropertyDescriptor controlProp = properties["Controls"];
            //// PropertyDescriptor controlProp2 = properties["Properties"];
            // if (controlProp != null)
            // {
            //     PropertyDescriptor[] propArray = new PropertyDescriptor[properties.Count - 1];
            //     int idx = 0;
            //     foreach (PropertyDescriptor p in properties)
            //     {
            //         if (p != controlProp)
            //         {
            //             propArray[idx++] = p;
            //         }
            //     }
            //     properties = new PropertyDescriptorCollection(propArray);
            // }

            List<string> category = new List<string>();
            List<TreeNode> categoryNode = new List<TreeNode>();
            foreach (PropertyDescriptor prop in properties)
            {
                if (!category.Contains(prop.Category)
                    && prop.Category != "Accessibility"
                    && prop.Category != "Data"
                    && prop.Category != "Focus"
                    && prop.Category != "Layout")
                {
                    category.Add(prop.Category);
                }
            }
            foreach (string cat in category)
            {
                categoryNode.Add(new TreeNode(cat));
            }

            foreach (PropertyDescriptor prop in properties)
            {
                TreeNode node = categoryNode.Where(p => p.Text == prop.Category).FirstOrDefault();
                if (node == null) { continue; }

                if (node.Text == "Properties")
                {

                }
                DesignerSerializationVisibilityAttribute visibility = (DesignerSerializationVisibilityAttribute)prop.Attributes[typeof(DesignerSerializationVisibilityAttribute)];

                if (visibility.Visibility == DesignerSerializationVisibility.Visible || visibility.Visibility == DesignerSerializationVisibility.Content || visibility.Visibility == DesignerSerializationVisibility.Hidden)
                {
                    if (!(prop.IsReadOnly))
                    {
                        if (listIgn.Any(p => p == prop.Name)) { continue; }
                        if (prop.PropertyType == typeof(Color))
                        {
                            TreeNode childnode = node.Nodes.Add(prop.Name);
                            childnode.ImageIndex = 108;
                            childnode.SelectedImageIndex = 108;
                            if (!FuncList.ContainsKey(prop.Name))
                            {
                                FuncList.Add(prop.Name, new ColorSetting());
                            }
                            //Không null tô màu
                            if (FuncList.ContainsKey(prop.Name))
                            {

                                string expression = (FuncList[prop.Name] as ColorSetting).Expression;
                                if (!string.IsNullOrEmpty(expression))
                                {
                                    childnode.BackColor = Color.Silver;
                                }
                            }
                        }
                        if (prop.PropertyType == typeof(string))
                        {
                            TreeNode childnode = node.Nodes.Add(prop.Name);
                            childnode.ImageIndex = 108;
                            childnode.SelectedImageIndex = 108;
                            if (!FuncList.ContainsKey(prop.Name))
                            {
                                FuncList.Add(prop.Name, new StringSetting());
                            }
                            if (FuncList.ContainsKey(prop.Name))
                            {
                                string expression = (FuncList[prop.Name] as StringSetting).Expression;
                                if (!string.IsNullOrEmpty(expression))
                                {
                                    childnode.BackColor = Color.Silver;
                                }
                            }



                        }
                        if (prop.PropertyType == typeof(bool))
                        {
                            TreeNode childnode = node.Nodes.Add(prop.Name);
                            childnode.ImageIndex = 108;
                            childnode.SelectedImageIndex = 108;
                            if (!FuncList.ContainsKey(prop.Name))
                            {
                                FuncList.Add(prop.Name, new BoolSetting());
                            }
                            if (FuncList.ContainsKey(prop.Name))
                            {

                                string expression = (FuncList[prop.Name] as BoolSetting).Expression;
                                if (!string.IsNullOrEmpty(expression))
                                {
                                    childnode.BackColor = Color.Silver;
                                }
                            }

                        }
                        if (prop.PropertyType == typeof(int))
                        {
                            TreeNode childnode = node.Nodes.Add(prop.Name);
                            childnode.ImageIndex = 108;
                            childnode.SelectedImageIndex = 108;
                            if (!FuncList.ContainsKey(prop.Name))
                            {
                                FuncList.Add(prop.Name, new IntSetting());
                            }

                            if (FuncList.ContainsKey(prop.Name))
                            {
                                string expression = (FuncList[prop.Name] as IntSetting).Expression;
                                if (!string.IsNullOrEmpty(expression))
                                {
                                    childnode.BackColor = Color.Silver;
                                }
                            }
                        }
                    }
                }

            }

            foreach (TreeNode node in categoryNode.OrderBy(p => p.Text))
            {
                treeRoot.Nodes.Add(node);
            }

            foreach (TreeNode _node in treeNode.Nodes)
            {
                treeNode.Nodes.Add(_node);
            }
            treeRoot.ExpandAll();
        }

        private void treeRoot_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.Text == "Appearance" || e.Node.Text == "Behavior" || e.Node.Text == "Misc")
                {
                    Label.Text = e.Node.Text;
                    MainPanel1.Controls.Clear(); return;
                }
                Label.Text = e.Node.Text;
                CustomCollectionEditorControl uc = null;

                if (FuncList[e.Node.Text] is ColorSetting)
                {
                    uc = new CustomCollectionEditorControl(IsScreen);
                    uc._Type = typeof(ColorObject);
                    uc.SettingText.Text = (FuncList[e.Node.Text] as ColorSetting).Expression;
                    uc.Collection = ColorConvert(FuncList[e.Node.Text] as ColorSetting);
                    uc.OnSelect += (exp, l) =>
                    {
                        SimpleList sim = l as SimpleList;
                        List<ColorObject> ColorObjects = new List<ColorObject>();
                        foreach (object obj in sim._contents)
                        {
                            if (obj is ColorObject)
                            {
                                ColorObjects.Add(obj as ColorObject);
                            }
                        }
                        FuncList[e.Node.Text] = new ColorSetting()
                        {
                            Expression = exp,
                            ColorObjects = ColorObjects
                        };
                    };
                }
                if (FuncList[e.Node.Text] is StringSetting)
                {
                    uc = new CustomCollectionEditorControl(IsScreen);
                    uc._Type = typeof(StringObject);
                    uc.SettingText.Text = (FuncList[e.Node.Text] as StringSetting).Expression;
                    uc.Collection = TextConvert(FuncList[e.Node.Text] as StringSetting);
                    uc.OnSelect += (exp, l) =>
                    {
                        SimpleList sim = l as SimpleList;
                        List<StringObject> TextObjects = new List<StringObject>();
                        foreach (object obj in sim._contents)
                        {
                            if (obj is StringObject)
                            {
                                TextObjects.Add(obj as StringObject);
                            }
                        }
                        FuncList[e.Node.Text] = new StringSetting()
                        {
                            Expression = exp,
                            StringObjects = TextObjects
                        };
                    };
                }
                if (FuncList[e.Node.Text] is BoolSetting)
                {
                    uc = new CustomCollectionEditorControl(IsScreen);
                    uc._Type = typeof(BoolObject);
                    uc.SettingText.Text = (FuncList[e.Node.Text] as BoolSetting).Expression;
                    uc.Collection = VisibleConvert(FuncList[e.Node.Text] as BoolSetting);
                    uc.OnSelect += (exp, l) =>
                    {
                        SimpleList sim = l as SimpleList;
                        List<BoolObject> VisibleObjects = new List<BoolObject>();
                        foreach (object obj in sim._contents)
                        {
                            if (obj is BoolObject)
                            {
                                VisibleObjects.Add(obj as BoolObject);
                            }
                        }
                        FuncList[e.Node.Text] = new BoolSetting()
                        {
                            Expression = exp,
                            BoolObjects = VisibleObjects
                        };
                    };
                }
                if (FuncList[e.Node.Text] is IntSetting)
                {
                    uc = new CustomCollectionEditorControl(IsScreen);
                    uc._Type = typeof(IntObject);
                    uc.SettingText.Text = (FuncList[e.Node.Text] as IntSetting).Expression;
                    uc.Collection = IntConvert(FuncList[e.Node.Text] as IntSetting);
                    uc.OnSelect += (exp, l) =>
                    {
                        SimpleList sim = l as SimpleList;
                        List<IntObject> VisibleObjects = new List<IntObject>();
                        foreach (object obj in sim._contents)
                        {
                            if (obj is IntObject)
                            {
                                VisibleObjects.Add(obj as IntObject);
                            }
                        }
                        FuncList[e.Node.Text] = new IntSetting()
                        {
                            Expression = exp,
                            IntObjects = VisibleObjects
                        };
                    };
                }
                MainPanel1.Controls.Clear();
                if (uc != null)
                {
                    MainPanel1.Controls.Add(uc);
                    uc.Dock = DockStyle.Fill;
                }
            }
            catch { }
        }

        public SimpleList ColorConvert(ColorSetting ColorSetting)
        {
            SimpleList listResult = new SimpleList();
            if (ColorSetting != null)
            {
                foreach (ColorObject o in ColorSetting.ColorObjects)
                {
                    listResult.Add(o);
                }
            }
            return listResult;
        }

        public SimpleList TextConvert(StringSetting TextSetting)
        {
            SimpleList listResult = new SimpleList();
            if (TextSetting != null)
            {
                foreach (StringObject o in TextSetting.StringObjects)
                {
                    listResult.Add(o);
                }
            }
            return listResult;
        }

        public SimpleList VisibleConvert(BoolSetting VisibleSetting)
        {
            SimpleList listResult = new SimpleList();
            if (VisibleSetting != null)
            {
                foreach (BoolObject o in VisibleSetting.BoolObjects)
                {
                    listResult.Add(o);
                }
            }
            return listResult;
        }

        public SimpleList IntConvert(IntSetting IntSetting)
        {
            SimpleList listResult = new SimpleList();
            if (IntSetting != null)
            {
                foreach (IntObject o in IntSetting.IntObjects)
                {
                    listResult.Add(o);
                }
            }
            return listResult;
        }


        private void frmProperties_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        public string[] listIgn = new string[]
       {
             "Name",
             "TabIndex",
             "ImageIndex",
             "AccessibleDescription",
             "AccessibleName",
             "UseWaitCursor",
"AllowDrop",
"TabStop",
"Code",
"ObjectSelect",
"DrawBoundMode",
"Authorization",
"ImageKey",
"UseMnemonic",
"UseVisualStyleBackColor",
"WordWrap",
"AutoEllipsis",
"UseCompatibleTextRendering",
"AcceptsReturn",
"Multiline",
"AcceptsTab",
"ShortcutsEnabled",
"TagName",
"Lock",
"BorderWidth",
"FillColorSetting1",
"FillColorSetting2",
"FillColorSetting3",
       };

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static string CodeReturn;
        private void OK_CLick(object sender, EventArgs e)
        {
            CodeReturn = string.Empty;
            List<string> tags = new List<string>();
            foreach (string key in FuncList.Keys)
            {
                if (FuncList[key] is ColorSetting)
                {
                    if (!string.IsNullOrEmpty((FuncList[key] as ColorSetting).Expression))
                    {
                        CodeReturn += Environment.NewLine + "[BEGIN]" + key + "_ColorSetting[KEY]" + JsonConvert.SerializeObject((FuncList[key] as ColorSetting)) + "[END]";
                        tags.AddRange(ExtractFromString((FuncList[key] as ColorSetting).Expression, "[", "]"));
                    }
                }
                if (FuncList[key] is StringSetting)
                {
                    if (!string.IsNullOrEmpty((FuncList[key] as StringSetting).Expression))
                    {
                        CodeReturn += Environment.NewLine + "[BEGIN]" + key + "_StringSetting[KEY]" + JsonConvert.SerializeObject((FuncList[key] as StringSetting)) + "[END]";
                        tags.AddRange(ExtractFromString((FuncList[key] as StringSetting).Expression, "[", "]"));
                    }
                }
                if (FuncList[key] is BoolSetting)
                {
                    if (!string.IsNullOrEmpty((FuncList[key] as BoolSetting).Expression))
                    {
                        CodeReturn += Environment.NewLine + "[BEGIN]" + key + "_BoolSetting[KEY]" + JsonConvert.SerializeObject((FuncList[key] as BoolSetting)) + "[END]";
                        tags.AddRange(ExtractFromString((FuncList[key] as BoolSetting).Expression, "[", "]"));
                    }
                }
                if (FuncList[key] is IntSetting)
                {
                    if (!string.IsNullOrEmpty((FuncList[key] as IntSetting).Expression))
                    {
                        CodeReturn += Environment.NewLine + "[BEGIN]" + key + "_IntSetting[KEY]" + JsonConvert.SerializeObject((FuncList[key] as IntSetting)) + "[END]";
                        tags.AddRange(ExtractFromString((FuncList[key] as IntSetting).Expression, "[", "]"));
                    }
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public static List<string> ExtractFromString(string text, string startString, string endString)
        {
            List<string> matched = new List<string>();
            int indexStart = 0, indexEnd = 0;
            bool exit = false;
            while (!exit)
            {
                indexStart = text.IndexOf(startString);
                indexEnd = text.IndexOf(endString);
                if (indexEnd < indexStart)
                {
                    string textkk = text.Substring(indexStart);
                    indexEnd = indexStart + textkk.IndexOf(endString);

                }
                if (indexStart != -1 && indexEnd != -1)
                {
                    matched.Add(text.Substring(indexStart + startString.Length,
                        indexEnd - indexStart - startString.Length));
                    text = text.Substring(indexEnd + endString.Length);
                }
                else
                    exit = true;
            }
            return matched;
        }

    }
}
