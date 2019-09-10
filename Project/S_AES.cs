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
    public partial class S_AES : Form
    {
        #region --Variables--
        string mainkey;
        string Plain_text;
        string encrypted_text;
        string[] w = new string[6];
        string[] wt = new string[] { "10000000", "00110000" };
        string[] sbox = new string[] { "1001", "0100", "1010", "1011", "1101", "0001", "1000", "0101",
                                       "0110", "0010", "0000", "0011", "1100", "1110", "1111", "0111" };
        string[] sbox_refrence = new string[] { "0000", "0001", "0010", "0011", "0100", "0101", "0110", "0111",
                                                "1000", "1001", "1010", "1011", "1100", "1101", "1110", "1111" };
        string[] addition_table = new string[] { "0123456789abcdef", 
                                                 "1032547698badcfe", 
                                                 "23016745ab89efcd", 
                                                 "32107654ba98fedc", 
                                                 "45670123cdef89ab", 
                                                 "54761032dcfe98ba", 
                                                 "67452301efcdab89", 
                                                 "76543210fedcba98", 
                                                 "89abcdef01234567", 
                                                 "98badcfe10325476", 
                                                 "ab89efcd23016745", 
                                                 "ba98fedc32107654", 
                                                 "cdef89ab45670123", 
                                                 "dcfe98ba54761032", 
                                                 "efcdab8967452301", 
                                                 "fedcba9876543210" };
        string[] multiblication_table = new string[] {"0000000000000000"
                                                     ,"0123456789abcdef" 
                                                     ,"02468ace3175b9fd" 
                                                     ,"0365cfa9b8de7412" 
                                                     ,"048c37bf62ea51d9" 
                                                     ,"05af72d8eb419c36" 
                                                     ,"06cabd71539fe824" 
                                                     ,"07e9f816da3425cb" 
                                                     ,"083b6e5dc4f7a291" 
                                                     ,"09182b3a4d5c6f7e" 
                                                     ,"0a7de493f5821b6c" 
                                                     ,"0b5ea1f47c29d683" 
                                                     ,"0cb759e2a61df348" 
                                                     ,"0d941c852fb63ea7" 
                                                     ,"0ef1d32c97684ab5" 
                                                     ,"0fd2964b1ec3875a" };
        string[,] mix_matrix = new string[2, 2] { { "1", "4" }, 
                                                  { "4", "1" } };
        string[] keys = new string[3];
        string[] s = new string[4];
        string[] s_hex = new string[4];
        string[] s_dash = new string[4];
        #endregion
        public S_AES()
        {
            InitializeComponent();
        }

        public void start(string plain,string k)
        {
            Plain_text = plain;
            mainkey = k;
            generate_keys();
            #region --output--
            for (int i = 0; i < w.Length; i++)
            {
                richTextBox1.Text += "w" + i + "=" + w[i] + "\t";
                if (i % 2 == 0 && i != 0 && i != 4)
                {
                    richTextBox1.Text += '\n';
                }
            }
            textBox1.Text = keys[0];
            textBox2.Text = keys[1];
            textBox3.Text = keys[2];
            #endregion
            #region --encryption--
            encrypt();
            richTextBox2.Text = encrypted_text;
            #endregion
        }


        #region --My Functions--
        #region --Helpers--
        bool isokay(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '1')
                    return false;
            }

            return true;
        }

        string XOR(string n1, string n2)
        {
            string answer = "";
            for (int i = 0; i < n1.Length; i++)
            {
                if (n1[i] == n2[i])
                {
                    answer += "0";
                }
                else
                {
                    answer += "1";
                }
            }
            return answer;
        }

        string rotatenib(string s)
        {
            string answer = "";
            string temp = "";
            for (int i = 0; i < s.Length / 2; i++)
            {
                answer += s[i + (s.Length / 2)];
                temp += s[i];
            }
            answer += temp;
            return answer;
        }

        string subnib(string nibble)
        {
            string answer = "";
            for (int i = 0; i < sbox_refrence.Length; i++)
            {
                if (nibble == sbox_refrence[i])
                {
                    answer = sbox[i];
                }
            }
            return answer;
        }

        string swap(string s)
        {
            string answer = "";
            for (int i = 0; i < 4; i++)
            {
                answer += s[i];
            }
            for (int i = 12; i < s.Length; i++)
            {
                answer += s[i];
            }
            for (int i = 8; i < 12; i++)
            {
                answer += s[i];
            }
            for (int i = 4; i < 8; i++)
            {
                answer += s[i];
            }
            return answer;
        }

        string mix(string s1, string s2, string m1, string m2)
        {
            string answer = "";
            s_hex[0] = convert_to_hex(Convert.ToInt32(convert_to_decimal(s1)));
            s_hex[1] = convert_to_hex(Convert.ToInt32(convert_to_decimal(s2)));
            for (int i = 0; i < 16; i++)
            {
                if (multiblication_table[i][1].ToString().ToLower() == s_hex[0].ToLower())
                {
                    for (int k = 0; k < 16; k++)
                    {
                        if (multiblication_table[1][k].ToString().ToLower() == m1.ToLower())
                        {
                            s_hex[0] = multiblication_table[i][k].ToString().ToLower();
                            break;
                        }
                    }
                    break;
                }
            }
            for (int i = 0; i < 16; i++)
            {
                if (multiblication_table[i][1].ToString().ToLower() == s_hex[1].ToLower())
                {
                    for (int k = 0; k < 16; k++)
                    {
                        if (multiblication_table[1][k].ToString().ToLower() == m2.ToLower())
                        {
                            s_hex[1] = multiblication_table[i][k].ToString().ToLower();
                            break;
                        }
                    }
                    break;
                }
            }
            //=============================================================================================
            for (int i = 0; i < 16; i++)
            {
                if (addition_table[i][0].ToString().ToLower() == s_hex[0].ToLower())
                {
                    for (int k = 0; k < 16; k++)
                    {
                        if (addition_table[0][k].ToString().ToLower() == s_hex[1].ToLower())
                        {
                            s_hex[0] = addition_table[i][k].ToString().ToLower();
                            break;
                        }
                    }
                    break;
                }
            }
            //==============================================================================================
            string tt = convert_to_binary(s_hex[0]);
            if (tt.Length < 4)
            {
                for (int i = 0; i < 4 - tt.Length; i++)
                {
                    answer += "0";
                }
            }
            answer += tt;
            return answer;
        }

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

        #region --Algorithm--

        void generate_keys()
        {
            #region --W's--
            //w0,w1
            for (int i = 0; i < 8; i++)
            {
                w[0] += mainkey[i];
                w[1] += mainkey[i + 8];
            }
            //======================================================
            //w2,w3,w4,w5
            int q1 = 0;
            for (int i = 2; i < 6; i++)
            {
                //w2,w4
                if (i % 2 == 0)
                {
                    string tempw, tempw2, tempw3 = "";
                    tempw = XOR(w[i - 2], wt[q1]);
                    tempw2 = rotatenib(w[i - 1]);
                    string t = "";
                    for (int z = 0; z < tempw2.Length; z++)
                    {
                        if (z == tempw2.Length / 2)
                        {
                            tempw3 += subnib(t);
                            t = "";
                        }
                        t += tempw2[z];
                    }
                    tempw3 += subnib(t);
                    w[i] = XOR(tempw, tempw3);
                    q1++;
                }
                //w3,w5
                else
                {
                    w[i] = XOR(w[i - 1], w[i - 2]);
                }
            }
            #endregion
            #region --keys--
            int q = 0;
            //k1,k2,k3
            for (int i = 0; i < 3; i++)
            {
                keys[i] = w[i + q] + w[i + q + 1];
                q++;
            }
            #endregion
        }

        void encrypt()
        {
            #region --round 0--
            Plain_text = XOR(Plain_text, keys[0]);
            #endregion
            #region --round 1--
            string t = "";
            string modified = "";
            //subnib
            for (int i = 0; i < Plain_text.Length; i++)
            {
                if (i == 4 || i == 8 || i == 12)
                {
                    modified += subnib(t);
                    t = "";
                }
                t += Plain_text[i];
            }
            modified += subnib(t);
            //swapping
            modified = swap(modified);
            textBox4.Text = modified;
            //mix Columns
            int q = 0;
            for (int i = 0; i < modified.Length; i++)
            {
                if (i == 4 || i == 8 || i == 12)
                {
                    q++;
                }
                s[q] += modified[i];
            }
            s_dash[0] = mix(s[0], s[1], mix_matrix[0, 0], mix_matrix[0, 1]);
            s_dash[1] = mix(s[0], s[1], mix_matrix[0, 1], mix_matrix[0, 0]);
            s_dash[2] = mix(s[2], s[3], mix_matrix[0, 0], mix_matrix[0, 1]);
            s_dash[3] = mix(s[2], s[3], mix_matrix[0, 1], mix_matrix[0, 0]);
            textBox5.Text = s_dash[0];
            textBox7.Text = s_dash[1];
            textBox6.Text = s_dash[2];
            textBox8.Text = s_dash[3];
            modified = "";
            modified = s_dash[0] + s_dash[1] + s_dash[2] + s_dash[3];
            //adding k1
            modified = XOR(modified, keys[1]);
            textBox9.Text = modified;
            #endregion
            #region --round 2--
            //nibble subistitution
            t = "";
            string temp = "";
            for (int i = 0; i < modified.Length; i++)
            {
                if (i == 4 || i == 8 || i == 12)
                {
                    temp += subnib(t);
                    t = "";
                }
                t += modified[i];
            }
            temp += subnib(t);
            modified = temp;
            //swap nibbles
            modified = swap(modified);
            //add k2
            modified = XOR(modified, keys[2]);
            textBox10.Text = modified;
            encrypted_text = modified;
            #endregion
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }





        #endregion
    }
}
