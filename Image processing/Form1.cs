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

namespace Image_processing
{
    public partial class Form1 : Form
    {
        enum operation { blackAndWhite, Sobel, Filter }
        static operation currentOperation = operation.blackAndWhite;
        static System.Threading.Timer timer;
        public Form1()
        {
            InitializeComponent();
            createTimer();
        }
        private void openButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
                }
                catch 
                {
                    MessageBox.Show("Cannot open this file", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void saveButton_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null) 
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pictureBox2.Image.Save(saveFileDialog1.FileName);
                    }
                    catch 
                    {
                        MessageBox.Show("Cannot save image", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void blackAndWhiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentOperation = operation.blackAndWhite;
            button_convert.Text = "Convert to black and white";
        }

        private void contureSobelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentOperation = operation.Sobel;
            button_convert.Text = "Select edge Sobel";
        }
        private void contureFilterDesignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentOperation = operation.Filter;
            button_convert.Text = "Select edge Filter";
        }

        private async void button_convert_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                button_convert.Enabled = false;
                progressBar1.Visible = true;
                Bitmap outputImage = null;
                timer.Change(0,500);
                switch (currentOperation)
                {
                    case operation.blackAndWhite:
                        {
                            progressBar1.Maximum = BlackWhiteFilter.totalProgress;
                            await Task.Run(() => 
                            {
                                outputImage = BlackWhiteFilter.convertToBlackAndWhite(new Bitmap(pictureBox1.Image));
                            }
                            );
                            break;
                        }
                    case operation.Sobel:
                        {
                            progressBar1.Maximum = Sobel.totalProgress;
                            await Task.Run(() =>
                            {
                            outputImage = Sobel.SobelEdgeDetection(new Bitmap(pictureBox1.Image));
                            });
                            break;
                        }
                    case operation.Filter:
                        {
                            progressBar1.Maximum = Filter.totalProgress;
                            await Task.Run(() =>
                            {
                                outputImage = Filter.ApplyEdgeDetection(new Bitmap(pictureBox1.Image));
                            });
                            break;
                        }
                }
                pictureBox2.Image = outputImage;
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                progressBar1.Visible = false;
                progressBar1.Value = 0;
                button_convert.Enabled = true;

            }
        }
        private void updateProgress(object obj)
        {
            int x = (int)obj;
            switch (currentOperation)
            {
                case operation.blackAndWhite:
                    {
                        progressBar1.Invoke((MethodInvoker)(() => progressBar1.Maximum = BlackWhiteFilter.totalProgress));
                        progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = BlackWhiteFilter.currentProgress));
                        break;
                    }
                case operation.Sobel:
                    {
                        progressBar1.Invoke((MethodInvoker)(() => progressBar1.Maximum = Sobel.totalProgress));
                        progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = Sobel.currentProgress));
                        break;
                    }
                case operation.Filter:
                    {
                        progressBar1.Invoke((MethodInvoker)(() => progressBar1.Maximum = Filter.totalProgress));
                        progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = Filter.currentProgress));
                        break;
                    }
            }
        }
        public void createTimer()
        {
            int num = 0;
            System.Threading.TimerCallback callback = new System.Threading.TimerCallback(updateProgress);
            timer = new System.Threading.Timer(callback, num, Timeout.Infinite, Timeout.Infinite);
        }

        
    }
}

