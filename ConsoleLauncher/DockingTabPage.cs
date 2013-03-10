using System;
using System.Collections.Generic;
using System.Text;
using ConsoleLauncher;
using System.Windows.Forms;

namespace ConsoleLauncher
{
    #region Enums
    public enum TabMode { Dock, Float };
    #endregion

    public class DockingTabPage : TabPage
    {
        private TabMode m_tabMode;
//        private Button btnchangemode;
        private Form m_tabForm;
        private TabControl m_tabParent;

        ///
        /// Indicates if the current view is being utilized in the VS.NET IDE or not.
        /// DesignMode is not working 
        /// Without this Visual Studio will crash every time you open the Designer!
        ///
        public new bool DesignMode
        {
            get
            {
                return (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv");
            }
        }

        [System.ComponentModel.Browsable(false)]
        public TabMode TabMode
        {
            get
            {
                return m_tabMode;
            }
            set
            {
                /// this prevents errors from someone going 
                /// TabMode = TabMode.Dock;
                /// TabMode = TabMode.Dock;
                /// TabMode = TabMode.Dock;
                /// TabMode = TabMode.Dock;
                if (m_tabMode == value)
                    return;
                /// we always want to flip the arrow around when we chance back and forth. 
                /// it make it look nicer
//                btnchangemode.Image.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipXY);

                m_tabMode = value;
                if (value == TabMode.Float)
                {
                    /// resize the form prior to moving the controls to it to avoid position 
                    /// and anchor issues
                    m_tabForm.Size = this.Size;
                    /// put the text of the tab on the form as its title
                    m_tabForm.Text = Text;
                    m_tabForm.FormBorderStyle = FormBorderStyle.SizableToolWindow;

                    /// decide who your parent is and remember it since you probably want to re-dock 
                    /// eventually
                    m_tabParent = (TabControl)Parent;
                    m_tabParent.TabPages.Remove(this);

                    #region transfer controls
                    /// you can't remove from a collection inside a foreach over the same collection 
                    /// so you have to make a temp collection
                    List<Control> controls = new List<Control>();
                    foreach (Control c in this.Controls)
                        controls.Add(c);
                    foreach (Control c in controls)
                        this.Controls.Remove(c);
                    foreach (Control c in controls)
                        m_tabForm.Controls.Add(c); 
                    #endregion

                    /// not all of the form space is available to us
                    /// see the code project article
//                    btnchangemode.Location = new System.Drawing.Point(myform.Width - btnchangemode.Width - 16, 0);
                    
                    /// show the form and bring it to the front
                    m_tabForm.Show();                    
                    m_tabForm.Focus();
                }
                else if (value == TabMode.Dock)
                {
                    if (m_tabParent != null)
                    {
                        m_tabParent.TabPages.Add(this);
                        /// resize the form prior to moving the controls to it to avoid position 
                        /// and anchor issues
                        m_tabForm.Size = this.Size;
                    }

                    /// hide the form, don't get rid of it, we will want to reuse it later
                    m_tabForm.Hide();

                    #region transfer controls
                    /// you can't remove from a collection inside a foreach over the same collection 
                    /// so you have to make a temp collection
                    List<Control> controls = new List<Control>();
                    foreach (Control c in m_tabForm.Controls)
                        controls.Add(c);
                    foreach (Control c in controls)
                        m_tabForm.Controls.Remove(c);
                    foreach (Control c in controls)
                        this.Controls.Add(c);
                    #endregion

                }
                else
                    throw new ApplicationException("unknown TabMode enum");
            }
        }

        public DockingTabPage(String title)
        {
            base.Text = title;
            m_tabForm = new Form();
            m_tabForm.FormClosing += new FormClosingEventHandler(tabForm_FormClosing);
            m_tabForm.ShowInTaskbar = false;
            m_tabMode = TabMode.Dock;
        }

        public DockingTabPage()
        {
            m_tabForm = new Form();
            m_tabForm.FormClosing += new FormClosingEventHandler(tabForm_FormClosing);
            m_tabForm.ShowInTaskbar = false;
            m_tabMode = TabMode.Dock;
        }

        private void tabForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            /// you don't want the user to close the undocked form then not be 
            /// able to get to all the controls that were on the form. so cancel 
            /// the close and dock the tabpage. this hides the form too.
            e.Cancel = true;
            TabMode = TabMode.Dock;
        }

        public void SwitchMode()
        {
            if (TabMode == TabMode.Dock)
                TabMode = TabMode.Float;
            else
                TabMode = TabMode.Dock;
        }
    }
}
