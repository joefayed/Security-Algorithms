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
    public partial class S_DES : Form
    {
        #region --Variables--
        //Sdes Algorithm Data
        private static int[] p10 = new int[] { 3, 5, 2, 7, 4, 10, 1, 9, 8, 6 };
        private static int[] p8 = new int[] { 6, 3, 7, 4, 8, 5, 10, 9 };
        private static int[] p4 = new int[] { 2, 4, 3, 1 };
        private static int[] ip = new int[] { 2, 6, 3, 1, 4, 8, 5, 7 };
        private static int[] ipneg = new int[] { 4, 1, 3, 5, 7, 2, 8, 6 };
        private static int[] ep = new int[] { 4, 1, 2, 3, 2, 3, 4, 1 };
        private static int[,] s0 = new int[4, 4] { { 1, 0, 3, 2 }, 
                                                   { 3, 2, 1, 0 }, 
                                                   { 0, 2, 1, 3 }, 
                                                   { 3, 1, 3, 2 } 
                                                   };
        private static int[,] s1 = new int[4, 4] { { 0, 1, 2, 3 }, 
                                                   { 2, 0, 1, 3 }, 
                                                   { 3, 0, 1, 0 }, 
                                                   { 2, 1, 0, 3 } 
                                                   };
        //=================================================================================
        //Variables
        int[] plaintext = new int[8];
        int[] ciphertext = new int[8];
        int[] left = new int[4];
        int[] right = new int[4];
        int[] righttemp = new int[8];
        int[] key = new int[10];
        List<int[]> subkeys = new List<int[]>();
        #endregion
        public S_DES()
        {
            InitializeComponent();
        }

        private void S_DES_Load(object sender, EventArgs e)
        {

        }

        public void start(string plain,string k,bool dec,bool error_in_keys)
        {
            for (int i = 0; i < 2; i++)
            {
                int[] pnn = new int[8];
                subkeys.Add(pnn);
            }
            #region --encryption--
            if (!dec)
            {
                MessageBox.Show(k);
                Generate_subkeys(k);
                if (!error_in_keys)
                {
                    int z = 0;
                    int[] plaintemp = new int[8];
                    foreach (char c in plain)
                    {
                        plaintext[z] = (int)char.GetNumericValue(c);
                        z++;
                    }
                    //IP
                    for (int i = 0; i < ip.Length; i++)
                    {
                        plaintemp[i] = plaintext[(ip[i] - 1)];
                    }
                    for (int q = 0; q < 8; q++)
                    {
                        textBox3.Text += plaintemp[q] + ",";
                    }
                    //===============================================
                    //spliting
                    for (int i = 0; i < plaintemp.Length; i++)
                    {
                        if (i < 4)
                        {
                            left[i] = plaintemp[i];
                        }
                        else
                        {
                            right[i - 4] = plaintemp[i];
                        }
                    }
                    //===============================================
                    //Iterations
                    for (int i = 0; i < 2; i++)
                    {
                        //function F
                        F(i);
                        //===============================================
                        //XOR Left
                        left = XOR(left, righttemp, left.Length);
                        //===============================================
                        //swapping
                        if (i == 0)
                        {
                            int t;
                            for (int q = 0; q < 4; q++)
                            {
                                t = right[q];
                                right[q] = left[q];
                                left[q] = t;
                            }
                            for (int q = 0; q < 4; q++)
                            {
                                textBox6.Text += left[q] + ",";
                            }
                            for (int q = 0; q < 4; q++)
                            {
                                textBox6.Text += right[q] + ",";
                            }
                        }
                    }
                    //===============================================
                    //Ip-1
                    for (int i = 0; i < 8; i++)
                    {
                        if (i < 4)
                        {
                            plaintemp[i] = left[i];
                        }
                        else
                        {
                            plaintemp[i] = right[i - 4];
                        }
                    }
                    for (int i = 0; i < ipneg.Length; i++)
                    {
                        ciphertext[i] = plaintemp[ipneg[i] - 1];
                    }
                    //===============================================
                    for (int i = 0; i < ciphertext.Length; i++)
                    {
                        richTextBox2.Text += ciphertext[i] + ",";
                    }
                }
                else
                {
                    MessageBox.Show("Error in your Key");
                    error_in_keys = false;
                }
            }
            #endregion
            #region --decryption--
            else
            {
                int q = 1;
                Generate_subkeys(k);
                if (!error_in_keys)
                {
                    int z = 0;
                    int[] ciphertemp = new int[8];
                    foreach (char c in plain)
                    {
                        ciphertext[z] = (int)char.GetNumericValue(c);
                        z++;
                    }
                    //IP
                    for (int i = 0; i < ip.Length; i++)
                    {
                        ciphertemp[i] = ciphertext[(ip[i] - 1)];
                    }
                    for (int d = 0; d < 8; d++)
                    {
                        textBox3.Text += ciphertemp[d] + ",";
                    }
                    //===============================================
                    //spliting
                    for (int i = 0; i < ciphertemp.Length; i++)
                    {
                        if (i < 4)
                        {
                            left[i] = ciphertemp[i];
                        }
                        else
                        {
                            right[i - 4] = ciphertemp[i];
                        }
                    }
                    //===============================================
                    //Iterations
                    for (int i = 0; i < 2; i++)
                    {
                        //function F
                        F(q);
                        q--;
                        //===============================================
                        //XOR Left
                        left = XOR(left, righttemp, left.Length);
                        //===============================================
                        //swapping
                        if (i == 0)
                        {
                            int t;
                            for (int d = 0; d < 4; d++)
                            {
                                t = right[d];
                                right[d] = left[d];
                                left[d] = t;
                            }
                            for (int d = 0; d < 4; d++)
                            {
                                textBox6.Text += left[d] + ",";
                            }
                            for (int d = 0; d < 4; d++)
                            {
                                textBox6.Text += right[d] + ",";
                            }
                        }
                    }
                    //===============================================
                    //Ip-1
                    for (int i = 0; i < 8; i++)
                    {
                        if (i < 4)
                        {
                            ciphertemp[i] = left[i];
                        }
                        else
                        {
                            ciphertemp[i] = right[i - 4];
                        }
                    }
                    for (int i = 0; i < ipneg.Length; i++)
                    {
                        plaintext[i] = ciphertemp[ipneg[i] - 1];
                    }
                    //===============================================
                    for (int i = 0; i < plaintext.Length; i++)
                    {
                        richTextBox2.Text += plaintext[i] + ",";
                    }
                }
                else
                {
                    MessageBox.Show("Error in your Key");
                    error_in_keys = false;
                }
            }
            #endregion
        }


        #region --My Functions--
        #region --Helpers--
        int convdecimal(int n1, int n2)
        {
            int dec;
            if (n1 == 0)
            {
                if (n2 == 0)
                {
                    dec = 0;
                }
                else
                {
                    dec = 1;
                }
            }
            else
            {
                if (n2 == 0)
                {
                    dec = 2;
                }
                else
                {
                    dec = 3;
                }
            }
            return dec;
        }

        int[] convbinary(int n)
        {
            int[] bin = new int[2];
            if (n == 0)
            {
                bin[0] = 0;
                bin[1] = 0;
            }
            else if (n == 1)
            {
                bin[0] = 0;
                bin[1] = 1;
            }
            else if (n == 2)
            {
                bin[0] = 1;
                bin[1] = 0;
            }
            else if (n == 3)
            {
                bin[0] = 1;
                bin[1] = 1;
            }
            return bin;
        }
        #endregion
        private void Generate_subkeys(string kal)
        {
            MessageBox.Show(kal);
            int z = 0;
            int temp1 = 0, temp2 = 0, temp3 = 0, temp4 = 0;
            int[] keytemp1 = new int[10];
            foreach (char c in kal)
            {
                key[z] = (int)char.GetNumericValue(c);
                z++;
            }
            //p10
            for (int i = 0; i < p10.Length; i++)
            {
                keytemp1[i] = key[(p10[i] - 1)];
            }
            //===============================================
            //ls1
            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                {
                    temp1 = keytemp1[0];
                    temp2 = keytemp1[5];
                }
                if (i < 4)
                {
                    keytemp1[i] = keytemp1[i + 1];
                    keytemp1[i + 5] = keytemp1[i + 6];
                }
                else
                {
                    keytemp1[i] = temp1;
                    keytemp1[i + 5] = temp2;
                }
            }
            //===============================================
            //p8
            for (int i = 0; i < p8.Length; i++)
            {
                subkeys[0][i] = keytemp1[(p8[i] - 1)];
            }
            //===============================================
            //ls2
            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                {
                    temp1 = keytemp1[0];
                    temp2 = keytemp1[5];
                    temp3 = keytemp1[1];
                    temp4 = keytemp1[6];
                }
                if (i < 3)
                {
                    keytemp1[i] = keytemp1[i + 2];
                    keytemp1[i + 5] = keytemp1[i + 7];
                }
                else if (i == 3)
                {
                    keytemp1[i] = temp1;
                    keytemp1[i + 5] = temp2;
                }
                else if (i == 4)
                {
                    keytemp1[i] = temp3;
                    keytemp1[i + 5] = temp4;
                }
            }
            //===============================================
            //p8
            for (int i = 0; i < p8.Length; i++)
            {
                subkeys[1][i] = keytemp1[(p8[i] - 1)];
            }
            //===============================================
            for (int i = 0; i < 2; i++)
            {
                for (int k = 0; k < 8; k++)
                {
                    if (i == 0)
                    {
                        textBox1.Text += subkeys[i][k] + ",";
                    }
                    else
                    {
                        textBox2.Text += subkeys[i][k] + ",";
                    }
                }
            }
        }

        private void F(int keyno)
        {
            righttemp = new int[8];
            //EP
            for (int i = 0; i < ep.Length; i++)
            {
                righttemp[i] = right[(ep[i] - 1)];
            }
            for (int k = 0; k < 8; k++)
            {
                if (textBox4.Text.Length < 16)
                {
                    textBox4.Text += righttemp[k] + ",";
                }
                else
                {
                    textBox5.Text += righttemp[k] + ",";
                }
            }
            //===============================================
            //XOR K1
            righttemp = XOR(righttemp, subkeys[keyno], righttemp.Length);
            //===============================================
            //Sboxes
            int[] t = new int[2];
            int row, col;
            for (int i = 0; i < righttemp.Length; i += 4)
            {
                row = convdecimal(righttemp[i], righttemp[i + 3]);
                col = convdecimal(righttemp[i + 1], righttemp[i + 2]);
                if (i == 0)
                {
                    t[0] = s0[row, col];
                }
                else
                {
                    t[1] = s1[row, col];
                }
            }
            int[] tt = new int[4];
            tt[0] = convbinary(t[0])[0];
            tt[1] = convbinary(t[0])[1];
            tt[2] = convbinary(t[1])[0];
            tt[3] = convbinary(t[1])[1];
            //===============================================
            //P4
            righttemp = new int[4];
            for (int i = 0; i < p4.Length; i++)
            {
                righttemp[i] = tt[(p4[i] - 1)];
            }
            //===============================================
            richTextBox1.Text += "\n";
            for (int k = 0; k < righttemp.Length; k++)
            {
                richTextBox1.Text += righttemp[k] + ",";
            }
        }

        private int[] XOR(int[] st, int[] k, int size)
        {
            int[] t = new int[size];
            for (int i = 0; i < st.Length; i++)
            {
                if (st[i] == k[i])
                {
                    t[i] = 0;
                }
                else
                {
                    t[i] = 1;
                }
            }
            return t;
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
