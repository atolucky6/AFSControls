//using Hmi.SymbolLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using vSymbolfactory.XamlHelper;
using XamlHelper;

namespace vGraphic
{
    public partial class frm_Graphic : Form
    {
        public delegate void Submit(string objectSelect, FlipType flip, Rotate rotate, FillMode fillMode, Color fillColor);
        public event Submit OnSubmit;

        string objectSelect = string.Empty;
        Color fillColor = Color.Green;
        FlipType flip = FlipType.None;
        Rotate rotate = Rotate.None;
        FillMode fillMode = FillMode.Original;

        public frm_Graphic()
        {

            InitializeComponent();

            this.objectSelect = "Pumps___Cool_pump";


        }

        public frm_Graphic(string objectSelect, FlipType flip, Rotate rotate, FillMode fillMode, Color fillColor)
        {
            InitializeComponent();
            this.objectSelect = objectSelect;
            this.flip = flip;
            this.rotate = rotate;
            this.fillMode = fillMode;
            this.fillColor = fillColor;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Helper.listDir.Count == 1)
            {
                Helper.GetListResource();
            }

            foreach (Dir dir in Helper.listDir.OrderBy(p => p.dirname))
            {
                listBox1.Items.Add(dir.dirname.Replace("_", " "));
            }

            listBox1.SelectedItem = objectSelect.Split(new string[] { "___" }, StringSplitOptions.None)[0];

            Bitmap fbitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (System.Drawing.Graphics grs = System.Drawing.Graphics.FromImage(fbitmap))
            {
                XamlHelper.XamlHelper.BitmapShade(grs, objectSelect, new Size(pictureBox1.Width, pictureBox1.Height), Helper.getdata(objectSelect), flip, rotate, fillMode, fillColor);

            }
            pictureBox1.Image = fbitmap;
            comboBox1.Text = this.fillMode.ToString();
            comboBox2.Text = this.flip.ToString();
            comboBox3.Text = this.rotate.ToString();
            colorPicker1.Color = this.fillColor;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListObject.Items.Clear();

            string dir = listBox1.SelectedItem.ToString().Replace(" ", "_");
            Dir _dir = Helper.listDir.Where(p => p.dirname == dir).FirstOrDefault();
            ImageList il = new ImageList();
            il.ColorDepth = ColorDepth.Depth32Bit;
            il.ImageSize = new System.Drawing.Size(64, 64);
            List<string> rmv = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<string, string> dic in _dir.dicsym.OrderBy(p => p.Key))
            {
                string objname = dic.Key;
                string data = dic.Value;
                if (!string.IsNullOrEmpty(data))
                {

                    Bitmap fbitmap = new Bitmap(64, 64);
                    using (System.Drawing.Graphics grs = System.Drawing.Graphics.FromImage(fbitmap))
                    {
                        XamlHelper.XamlHelper.BitmapShade(grs, objname, new Size(64, 64), data, XamlHelper.FlipType.None, XamlHelper.Rotate.None, XamlHelper.FillMode.Original, Color.Black);
                    }
                    il.Images.Add(fbitmap);
                }
                else
                {
                    rmv.Add(dic.Key);
                }
            }
            foreach (string a in rmv)
            {
                _dir.dicsym.Remove(a);
            }
            int count = 0;

            ListObject.LargeImageList = il;
            foreach (System.Collections.Generic.KeyValuePair<string, string> dic in _dir.dicsym.OrderBy(p => p.Key))
            {
                ListViewItem lst1 = new ListViewItem();
                lst1.Text = dic.Key.Replace("_", " ");
                lst1.ImageIndex = count++;
                lst1.SubItems.Add(_dir.dirname + "___" + dic.Key);
                ListObject.Items.Add(lst1);
            }
            ListObject.View = View.LargeIcon;
            this.Invalidate();
        }

        private void ListObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                objectSelect = ListObject.SelectedItems[0].SubItems[1].Text;

                Bitmap fbitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                using (System.Drawing.Graphics grs = System.Drawing.Graphics.FromImage(fbitmap))
                {
                    XamlHelper.XamlHelper.BitmapShade(grs, objectSelect, new Size(pictureBox1.Width, pictureBox1.Height), Helper.getdata(objectSelect), flip, rotate, fillMode, fillColor);

                }
                pictureBox1.Image = fbitmap;
            }
            catch { }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillMode = (FillMode)Enum.Parse(typeof(FillMode), comboBox1.Text);
            Bitmap fbitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (System.Drawing.Graphics grs = System.Drawing.Graphics.FromImage(fbitmap))
            {
                XamlHelper.XamlHelper.BitmapShade(grs, objectSelect, new Size(pictureBox1.Width, pictureBox1.Height), Helper.getdata(objectSelect), flip, rotate, fillMode, fillColor);

            }
            pictureBox1.Image = fbitmap;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            flip = (FlipType)Enum.Parse(typeof(FlipType), comboBox2.Text);
            Bitmap fbitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (System.Drawing.Graphics grs = System.Drawing.Graphics.FromImage(fbitmap))
            {
                XamlHelper.XamlHelper.BitmapShade(grs, objectSelect, new Size(pictureBox1.Width, pictureBox1.Height), Helper.getdata(objectSelect), flip, rotate, fillMode, fillColor);

            }
            pictureBox1.Image = fbitmap;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            rotate = (Rotate)Enum.Parse(typeof(Rotate), comboBox3.Text);
            Bitmap fbitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (System.Drawing.Graphics grs = System.Drawing.Graphics.FromImage(fbitmap))
            {
                XamlHelper.XamlHelper.BitmapShade(grs, objectSelect, new Size(pictureBox1.Width, pictureBox1.Height), Helper.getdata(objectSelect), flip, rotate, fillMode, fillColor);

            }
            pictureBox1.Image = fbitmap;
        }

        private void colorEdit1_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
        }

        private void AddSymbol_Click(object sender, EventArgs e)
        {
            string dir = listBox1.SelectedItem.ToString().Replace(" ", "_");
            if (dir == "Graphics")
            {
                openFileDialog1.Filter = "xaml files (*.xaml)|*.xaml";
                DialogResult result = openFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string filefullpath = openFileDialog1.FileName;
                    string filename = System.IO.Path.GetFileName(filefullpath).Replace(" ", "_").Replace(".xaml", "").Replace(".XAML", "");
                    string text = File.ReadAllText(filefullpath, Encoding.UTF8);
                    Helper.AddOrUpdateResource(Helper.ResxFile, "Graphics___" + filename, text, 1);
                    Helper.GetListResource();
                    listBox1_SelectedIndexChanged(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Function only support Graphics Category.");
                return;
            }
        }

        private void RemoteSymbol_CLick(object sender, EventArgs e)
        {
            string dir = listBox1.SelectedItem.ToString().Replace(" ", "_");

            if (dir == "Graphics")
            {
                string obj = ListObject.SelectedItems[0].SubItems[1].Text;
                try
                {
                    Helper.RemoveResource(Helper.ResxFile, obj);
                    Helper.listDir = new List<Dir>()
                    {
                        new Dir(){dirname="Graphics",dicsym=new Dictionary<string, string>()}
                    };
                    Helper.GetListResource();
                    listBox1_SelectedIndexChanged(sender, e);
                }
                catch { }
            }
            else
            {
                MessageBox.Show("Function only support Graphics Category.");
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (OnSubmit != null)
            {
                OnSubmit(objectSelect, flip, rotate, fillMode, fillColor); this.Close();
            }

        }


        private void colorPicker1_ColorChanged(object sender, EventArgs e)
        {
           fillColor = colorPicker1.Color;
            Bitmap fbitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (System.Drawing.Graphics grs = System.Drawing.Graphics.FromImage(fbitmap))
            {
                XamlHelper.XamlHelper.BitmapShade(grs, objectSelect, new Size(pictureBox1.Width, pictureBox1.Height), Helper.getdata(objectSelect), flip, rotate, fillMode, fillColor);

            }
            pictureBox1.Image = fbitmap;

        }
    }

}
