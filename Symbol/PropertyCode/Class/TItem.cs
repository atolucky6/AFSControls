
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PropertyCode
{

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
