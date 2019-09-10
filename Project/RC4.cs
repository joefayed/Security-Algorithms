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
    public partial class RC4 : Form
    {
        #region --Variables--
        string plaintext;
        string key;
        string cipher = "";
        string cipher_binary = "";
        int[] s = new int[8];
        int[] t = new int[8];
        #endregion
        public RC4()
        {
            InitializeComponent();
        }

        public void start(string plain,string k)
        {
            plaintext = plain;
            key = k;
            for (int i = 0; i < 8; i++)
            {
                s[i] = i;
                if (i < key.Length)
                {
                    t[i] = Convert.ToInt32(char.GetNumericValue(key[i]));
                }
                else
                {
                    t[i] = Convert.ToInt32(char.GetNumericValue(key[(i - key.Length)]));
                }
                textBox1.Text += s[i].ToString();
                textBox2.Text += t[i].ToString();
            }
            initial_permutation();
            encryption();
            richTextBox2.Text = cipher + "\n" + cipher_binary;
        }



        #region --My Functions--
        #region --helpers--
        void swap(int n1, int n2)
        {
            int temp = s[n1];
            s[n1] = s[n2];
            s[n2] = temp;
        }
        int XOR(int n1, int n2)
        {
            int answer;
            string n11, n22, temp = "";
            n11 = convert_to_binary(n1);
            n22 = convert_to_binary(n2);
            for (int i = 0; i < n11.Length; i++)
            {
                if (n11[i] == n22[i])
                {
                    temp += "0";
                }
                else
                {
                    temp += "1";
                }
            }
            answer = Convert.ToInt32(convert_to_hex(temp));
            return answer;
        }

        string convert_to_hex(string d)
        {
            string answer = "";
            string[] dict = new string[] { "000", "001", "010", "011", "100", "101", "110", "111" };
            for (int i = 0; i < 8; i++)
            {
                if (d == dict[i])
                {
                    answer = i.ToString();
                }
            }
            return answer;
        }

        string convert_to_binary(int d)
        {
            string answer = "";
            string[] dict = new string[] { "000", "001", "010", "011", "100", "101", "110", "111" };
            for (int i = 0; i < 8; i++)
            {
                if (d == i)
                {
                    answer = dict[i];
                }
            }
            return answer;
        }

        #endregion
        void initial_permutation()
        {
            int j = 0;
            for (int i = 0; i < 8; i++)
            {
                j = (j + s[i] + t[i]) % 8;
                swap(i, j);
                richTextBox1.Text += "iteration " + i.ToString() + ":\n S=";
                for (int q = 0; q < s.Length; q++)
                {
                    richTextBox1.Text += s[q].ToString() + ",";
                }
                richTextBox1.Text += "\n";
            }
        }
        void encryption()
        {
            int i = 0, j = 0, which = 0;
            int f, k;
            while (which < plaintext.Length)
            {
                i = (i + 1) % 8;
                j = (j + s[i]) % 8;
                swap(i, j);
                f = (s[i] + s[j]) % 8;
                k = s[f];
                cipher += XOR(k, Convert.ToInt32(char.GetNumericValue(plaintext[which])));
                cipher_binary += convert_to_binary(Convert.ToInt32(char.GetNumericValue(cipher[which])));
                which++;
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
