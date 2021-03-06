// *****************************************************************************
// 
//  © Component Factory Pty Ltd 2017. All rights reserved.
//	The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, 13 Swallows Close, 
//  Mornington, Vic 3931, Australia and are supplied subject to licence terms.
// 
//  Version 4.6.0.0 	www.ComponentFactory.com
// *****************************************************************************

using System;
using System.Text;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;

namespace EasyScada.Winforms.Controls
{
	/// <summary>
	/// Storage for palette ribbon group button text states.
	/// </summary>
    public class EasyPaletteRibbonGroupButtonText : EasyPaletteRibbonGroupBaseText
    {
        #region Identity
        /// <summary>
        /// Initialize a new instance of the EasyPaletteRibbonGroupButtonText class.
		/// </summary>
        /// <param name="redirect">Redirector to inherit values from.</param>
        /// <param name="needPaint">Delegate for notifying paint requests.</param>
        public EasyPaletteRibbonGroupButtonText(PaletteRedirect redirect,
                                                   NeedPaintHandler needPaint)
            : base(redirect, PaletteRibbonTextStyle.RibbonGroupButtonText, needPaint)
		{
        }
        #endregion
    }
}
