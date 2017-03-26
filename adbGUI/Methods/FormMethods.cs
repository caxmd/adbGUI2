﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace adbGUI
{
    public class FormMethods
    {
        private MainForm frm;

        AdbOps adb = new AdbOps();

        public FormMethods(MainForm f)
        {
            //Pass the MainForm instance
            frm = f;
        }

        public string SelectedDevice()
        {
            if (frm.cbx_connectedDevices.Items.Count == 0)
            {
                return "";
            }
            else
            {
                return frm.cbx_connectedDevices.SelectedItem.ToString();

            }
        }

        public void RefreshSerialsInCombobox(List<string> devices)
        {
            frm.cbx_connectedDevices.Items.Clear();

            foreach (var item in devices)
            {
                frm.cbx_connectedDevices.Items.Add(item);
            }

            frm.cbx_connectedDevices.SelectedIndex = frm.cbx_connectedDevices.Items.Count - 1;

        }

        public void RefreshInstalledAppsInCombobox()
        {
            frm.cbx_installedApps.Items.Clear();

            frm.cbx_installedApps.Enabled = false;

            string output = adb.StartProcessingInThread("shell \"pm list packages -3 | cut -c9- | sort\"", SelectedDevice());


            foreach (var item in output.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                frm.cbx_installedApps.Items.Add(item);
            }

            if (frm.cbx_installedApps.Items.Count > 0)
            {
                frm.cbx_installedApps.SelectedIndex = 0;

            }


            frm.cbx_installedApps.Enabled = true;

        }

        public void KillServer()
        {
            try
            {
                foreach (var pr in Process.GetProcessesByName("adb"))
                {
                    pr.Kill();
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        
        public void ShowMboxAborted()
        {
            MessageBox.Show("Processing aborted succesfully", "Abortion succesfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


    }
}