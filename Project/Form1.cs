using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class Form1 : Form
    {
        string plaintext;
        string key;
        bool error_in_keys = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        #region --Radio_buttons_checks--
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked == true)
            {
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                radioButton5.Enabled = true;
                radioButton6.Enabled = true;
                radioButton3.Enabled = false;
                radioButton4.Enabled = false;
                radioButton1.Checked = true;
            }
            else
            {
                radioButton3.Enabled = true;
                radioButton4.Enabled = true;
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked == true)
            {
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                radioButton3.Enabled = true;
                radioButton4.Enabled = true;
                radioButton5.Enabled = true;
                radioButton6.Enabled = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            richTextBox1.Text = "Enter Plain Text Here";
            richTextBox2.Text = "Enter Keyword Here";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            richTextBox1.Text = "please enter a valid 16-Bit plaintext";
            richTextBox2.Text = "please enter a valid 16-Bit Main Key";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            richTextBox1.Text = "Enter What You Need To Encrypt/decrypt Here";
            richTextBox2.Text = "Please Enter A 10-bit Key";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            richTextBox1.Text = "Please Enter Input here";
            richTextBox2.Text = "Please Enter A key here";
        }
        #endregion

        #region --buttons--
        //Start
        private void button1_Click(object sender, EventArgs e)
        {
            #region --Main Error Checking
            if (radioButton5.Checked==false&&radioButton6.Checked==false)
            {
                MessageBox.Show("Please Select An operation To perform");
            }
            else if (radioButton1.Checked == false && radioButton2.Checked == false && radioButton3.Checked == false && radioButton4.Checked == false)
            {
                MessageBox.Show("please Select An algorithm");
            }
            #endregion
            #region --Algorithms--
            else
            {
                #region --Playfair--
                #region --error checking--
                if (radioButton1.Checked == true)
                {
                    if (richTextBox1.Text == "Enter Plain Text Here")
                    {
                        MessageBox.Show("Please enter a text to Encrypt");
                    }
                    else if (richTextBox2.Text == "Enter Keyword Here")
                    {
                        MessageBox.Show("Please enter a Keyword");
                    }
                #endregion
                    #region --code--
                    else
                    {
                        plaintext = richTextBox1.Text;
                        key = richTextBox2.Text;
                        Playfair pl = new Playfair();
                        pl.start(plaintext, key, (radioButton6.Checked == true ? true : false));
                        pl.Show();
                    }
                    #endregion

                }
                #endregion
                #region --S-DES--
                else if (radioButton2.Checked == true)
                {
                    #region --Error Checking--
                    if (richTextBox1.Text == "Enter What You Need To Encrypt/decrypt Here" || !isokay(richTextBox1.Text) || richTextBox1.Text.Length != 8)
                    {
                        MessageBox.Show("Please Enter A Valid 8-bit Stream To Decrypt");
                    }
                    else if (richTextBox2.Text == "Please Enter A 10-bit Key" || !isokay(richTextBox2.Text) || richTextBox2.Text.Length != 10)
                    {
                        error_in_keys = true;
                        MessageBox.Show("Please Enter a Valid 10-bit Key");
                    }
                    #endregion
                    #region --code--
                    else
                    {
                        plaintext = richTextBox1.Text;
                        key = richTextBox2.Text;
                        MessageBox.Show(key);
                        S_DES s = new S_DES();
                        s.start(plaintext, key, (radioButton6.Checked == true ? true : false), error_in_keys);
                        s.Show();
                    }
                    #endregion
                }
                #endregion
                #region --S-AES--
                else if (radioButton3.Checked == true)
                {
                    #region --Error Checking--
                    if (!isokay(richTextBox1.Text) || richTextBox1.Text.Length != 16)
                    {
                        MessageBox.Show("please enter a valid 16-Bit plaintext");
                    }
                    else if (!isokay(richTextBox2.Text) || richTextBox2.Text.Length != 16)
                    {
                        MessageBox.Show("please enter a valid 16-Bit Main Key");
                    }
                    #endregion
                    #region --code--
                    else
                    {
                        plaintext = richTextBox1.Text;
                        key = richTextBox2.Text;
                        S_AES ss = new S_AES();
                        ss.start(plaintext, key);
                        ss.Show();
                    }
                    #endregion
                }
                #endregion
                #region --RC4--
                else
                {
                    #region --ErrorChecking--
                    if (richTextBox1.Text == "Please Enter Input here")
                    {
                        MessageBox.Show("please enter a valid 16-Bit plaintext");
                    }
                    else if (richTextBox2.Text == "Please Enter A key here")
                    {
                        MessageBox.Show("please enter a valid 16-Bit Main Key");
                    }
                    #endregion
                    #region --Code--
                    else
                    {
                        plaintext = richTextBox1.Text;
                        key = richTextBox2.Text;
                        RC4 r = new RC4();
                        r.start(plaintext, key);
                        r.Show();
                    }
                    #endregion
                }
                #endregion
            }
            #endregion
        }
        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            radioButton5.Checked = false;
            radioButton6.Checked = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            radioButton3.Enabled = false;
            radioButton4.Enabled = false;
            groupBox1.Enabled = false;
        }
        #endregion

        #region --Tab Menu--
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("*You need to Enter the opration you want first then enter tha algorithm then the data,\n*The specific data for each algorithm is specifed after selction,\n*If you want to convert form one data type to another use the converter above ( file->Converter )");
        }

        private void binaryToHexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Converter c = new Converter();
            c.start(false);
            c.Show();
        }

        private void hexToBinaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Converter c = new Converter();
            c.start(true);
            c.Show();
        }
        #endregion

        #region --helpers--
        bool isokay(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '1')
                    return false;
            }

            return true;
        }
        #endregion
        //=============================================================================================================
    }
}
