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
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using EasyScada.Winforms.Controls;

namespace EasyScada.Winforms.Controls.Navigator
{
    /// <summary>
    /// Dictionary lookup from unique name to the EasyPage.
    /// </summary>
    public class UniqueNameToPage : Dictionary<string, EasyPage> { };

    /// <summary>
    /// Specialise the generic collection event args with specific type.
    /// </summary>
    public class EasyPageEventArgs : TypedCollectionEventArgs<EasyPage> 
    {
        #region Public
        /// <summary>
        /// Initialize a new instance of the EasyPageEventArgs class.
        /// </summary>
        /// <param name="item">Page effected by event.</param>
        /// <param name="index">Index of page in the owning collection.</param>
        public EasyPageEventArgs(EasyPage item, int index)
            : base(item, index)
        {
        }
        #endregion
    }

    /// <summary>
    /// Specialise the generic collection with type specific rules for item accessor.
    /// </summary>
    public class EasyPageCollection : TypedCollection<EasyPage>
    {
        #region Public
        /// <summary>
        /// Gets the item with the provided unique name.
        /// </summary>
        /// <param name="name">Name of the ribbon tab instance.</param>
        /// <returns>Item at specified index.</returns>
        public override EasyPage this[string name]
        {
            get
            {
                // First priority is the UniqueName
                foreach (EasyPage page in this)
                    if (page.UniqueName == name)
                        return page;

                // Second priority is the design time Name
                foreach (EasyPage page in this)
                    if (page.Name == name)
                        return page;

                // Third priority is the Text of the page
                foreach (EasyPage page in this)
                    if (page.Text == name)
                        return page;

                // Let base class perform standard processing
                return base[name];
            }
        }

        /// <summary>
        /// Gets the number of visible pages in the collection.
        /// </summary>
        public int VisibleCount
        {
            get
            {
                int visibleCount = 0;

                // Count the number of pages that are visible
                foreach (EasyPage page in this)
                    if (page.LastVisibleSet)
                        visibleCount++;

                return visibleCount;
            }
        }
        #endregion
    }
}

