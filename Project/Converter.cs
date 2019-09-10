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
    public partial class Converter : Form
    {
        bool whichone = false;
        string data = "";
        public Converter()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void start(bool which)
        {
            whichone = which;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            data = richTextBox1.Text;
            #region --hex to binary--
            if (whichone)
            {
                try
                {
                    richTextBox2.Text = convert_to_binary(data);
                }
                catch
                {
                    MessageBox.Show("error converting check your input");
                }
            }
            #endregion
            #region --binary to hex--
            else
            {
                try
                {
                    richTextBox2.Text = convert_to_hex(Int32.Parse(data));
                }
                catch
                {
                    MessageBox.Show("error converting check your input");
                }
            }
            #endregion
        }

        #region --functions--
        string toChar(int n)
        {
            const string alpha = "0123456789ABCDEF";
            return alpha.Substring(n, 1);
        }

        string convert_to_decimal(string n)
        {
            int Index = 0;
            int Decimal = 0;
            foreach (char Char in n.Reverse())
            {
                if (Index != 0)
                {
                    Decimal += Index * 2 * Convert.ToInt32(Char.ToString());
                    Index = Index * 2;
                }
                else
                {
                    Decimal += Convert.ToInt32(Char.ToString());
                    Index++;
                }
            }
            return Decimal.ToString();
        }

        string convert_to_hex(int d)
        {
            string answer = "";
            var r = d % 16;
            if (d - r == 0)
            {
                answer = toChar(Convert.ToInt32(r));
            }
            else
            {
                answer = convert_to_hex((d - r) / 16) + toChar(Convert.ToInt32(r));
            }
            return answer;
        }

        string convert_to_binary(string hexvalue)
        {
            string answer = "";
            answer = Convert.ToString(Convert.ToInt32(hexvalue, 16), 2);
            return answer;
        }
        #endregion
    }
}
