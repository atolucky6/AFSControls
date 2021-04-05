using EasyScada.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vGraphic;

namespace VietControl
{
    public class Class1 : SymFact, ISupportConnector, ISupportInitialize
    {
        #region ISupportConnector
        [Description("Select driver connector for control")]
        [Browsable(true), Category(DesignerCategory.EASYSCADA)]
        public IEasyDriverConnector Connector => EasyDriverConnectorProvider.GetEasyDriverConnector();
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

        private void OnConnectorStarted(object sender, EventArgs e)
        {
           PropertyCode.ScadaAutoCode.Main( this, this.ScadaAutoCode);
        }
    }
}
