using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageEncryptorUI
{
    public partial class Loading : Form
    {
        double progress = 0;
        int time = 0;
        DateTime start = DateTime.Now;
        Thread thread;
        public object Data;

        public Loading(string title, int time, Func<object> longRunningFunction = null, Action longRunningAction = null)
        {            
            InitializeComponent();

            thread = new Thread(() =>
            {
                if (longRunningFunction != null)
                {
                    Data = longRunningFunction.Invoke();
                }
                else if(longRunningAction != null)
                {
                    longRunningAction.Invoke();
                }
                else
                {
                    throw new Exception("Need a long running function or action to run");
                }
            });
            this.thread.Start();
            this.time = time;
            Text = title;
        }

        private void tmrLoading_Tick(object sender, EventArgs e)
        {
            if (time > 0)
            {
                progress = ((DateTime.Now - start).TotalSeconds / (double)time) * prgLoading.Maximum;
                prgLoading.Value = Math.Min((int)progress, prgLoading.Maximum);

                if (thread != null && thread.ThreadState == ThreadState.Stopped)
                {
                    Close();
                }
            }
        }
    }
}
