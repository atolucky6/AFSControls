using PropertyCode;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;


    // This UITypeEditor can be associated with Int32, Double and Single
    // properties to provide a design-mode angle selection interface.
   [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class PropertyEditor : System.Drawing.Design.UITypeEditor
    {
        public PropertyEditor()
        {
        }

        // Indicates whether the UITypeEditor provides a form-based (modal) dialog,
        // drop down dialog, or no UI outside of the properties window.
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal; //O vuong canh property
        }

        // Displays the UI for value selection.
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {

            //if (value.GetType() != typeof(string))
            //    return value;

           // Control control = context.Instance as Control;

            // Uses the IWindowsFormsEditorService to display a
            // drop-down UI in the Properties window.
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                // Display an angle selection control and retrieve the value.
                frmProperties frm = new frmProperties();
                frm.ShowDialog();
                //if (value.GetType() == typeof(string))
                //    return (string)frm.Name;
            }
            return value;
        }
    }

