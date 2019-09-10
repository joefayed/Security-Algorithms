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
    public partial class Playfair : Form
    {
        #region --Variables--
        private string Plain_text;
        private List<string> stage1 = new List<string>();
        private string Cipher_Text;
        private List<string> stage11 = new List<string>();
        private string keyword;
        private char[,] playfair = new char[5, 5];
        private int[] XS = new int[2];
        private int[] YS = new int[2];
        private bool found = false;
        private bool skip = false;
        #endregion
        public Playfair()
        {
            InitializeComponent();
        }

        private void Playfair_Load(object sender, EventArgs e)
        {

        }

        public void start(string plain,string k, bool dec)
        {
            #region --encryption--
            if (!dec)
            {
                #region --First stage--
                Cipher_Text = "";
                Plain_text = plain;
                Plain_text.ToLower();
                string pl = "";
                for (int i = 0; i < Plain_text.Length; i++)
                {
                    if (Plain_text[i] != ' ')
                    {
                        pl += Plain_text[i];
                    }
                }
                Plain_text = pl;
                for (int i = 0; i < Plain_text.Length; i += 2)
                {
                    //if still has characters
                    if ((i + 1) < Plain_text.Length)
                    {
                        if (Plain_text[i] != Plain_text[i + 1])
                        {
                            string pnn = "";
                            pnn += Plain_text[i];
                            pnn += Plain_text[i + 1];
                            stage1.Add(pnn);
                        }
                        else
                        {
                            string pnn = "";
                            pnn += Plain_text[i];
                            pnn += 'x';
                            i -= 1;
                            stage1.Add(pnn);
                        }
                    }
                    //if in the end of the string
                    else
                    {
                        string pnn = "";
                        pnn += Plain_text[i];
                        pnn += 'x';
                        stage1.Add(pnn);
                    }
                }
                #endregion
                //========================================================
                #region --Second Stage--

                create_playfair_matrix(k);
                for (int i = 0; i < stage1.Count; i++)
                {
                    encrypting(stage1[i].ToLower());
                }
                MessageBox.Show("Successfully Encrypted");
                this.richTextBox2.Text = Cipher_Text.ToUpper();
                #endregion
                //========================================================
            }
            #endregion
            #region --decryption--
            else
            {
                #region --First stage--
                Plain_text = "";
                Cipher_Text = plain;
                Cipher_Text.ToUpper();
                string pl = "";
                for (int i = 0; i < Cipher_Text.Length; i++)
                {
                    if (Cipher_Text[i] != ' ')
                    {
                        pl += Cipher_Text[i];
                    }
                }
                Cipher_Text = pl;
                for (int i = 0; i < Cipher_Text.Length; i += 2)
                {
                    //if still has characters
                    if ((i + 1) < Cipher_Text.Length)
                    {
                        string pnn = "";
                        pnn += Cipher_Text[i];
                        pnn += Cipher_Text[i + 1];
                        stage11.Add(pnn);
                    }
                }
                #endregion
                //======================================================================
                #region --Second stage--
                create_playfair_matrix(k);
                for (int i = 0; i < stage11.Count; i++)
                {
                    decrypting(stage11[i].ToLower());
                }
                string p = "";
                for (int i = 0; i < Plain_text.Length; i++)
                {
                    //Middle x
                    if (i > 0 && i < Plain_text.Length - 1)
                    {
                        if (Plain_text[i] == 'x' && Plain_text[i - 1] == Plain_text[i + 1])
                        {
                            skip = true;
                        }
                    }
                    else if (i == Plain_text.Length - 1 && Plain_text[i] == 'x')
                    {
                        skip = true;
                    }
                    if (!skip)
                    {
                        p += Plain_text[i];
                    }
                    else
                    {
                        skip = false;
                    }
                }
                Plain_text = p;
                MessageBox.Show("Successfully Decrypted");
                this.richTextBox2.Text = "The Text With Normal i/j: \n" + Plain_text.ToLower() + "\nThe Text With Reversed i/j: \n";
                for (int i = 0; i < Plain_text.Length; i++)
                {
                    if (Plain_text[i] == 'i')
                    {
                        this.richTextBox2.Text += 'j';
                    }
                    else if (Plain_text[i] == 'j')
                    {
                        this.richTextBox2.Text += 'i';
                    }
                    else
                    {
                        this.richTextBox2.Text += Plain_text[i];
                    }
                }
                #endregion
            }
            #endregion
        }

        #region --My functions--
        public void create_playfair_matrix(string kal)
        {
            IDictionary<int, char> dict = new Dictionary<int, char>();
            int l = 97;
            for (int i = 0; i < 26; i++)
            {
                dict.Add(i, (char)l);
                l++;
            }
            keyword = "";
            foreach (char c in kal)
            {
                if (c != ' ')
                {
                    if (c != 'i' && c != 'j')
                    {
                        if (!keyword.ToLower().Contains(c) && !keyword.ToUpper().Contains(c))
                        {
                            keyword += c;
                        }
                    }
                    else
                    {
                        if (!keyword.ToLower().Contains('i') && !keyword.ToUpper().Contains('I') &&
                            !keyword.ToLower().Contains('j') && !keyword.ToUpper().Contains('J'))
                        {
                            keyword += c;
                        }
                    }
                }
            }
            keyword.ToLower();
            l = 0;
            int x = 0;
            int y = 0;
            string t = "";
            for (int i = 0; i < 5; i++)
            {
                for (int k = 0; k < 5; k++)
                {
                    if (l < keyword.Length)
                    {
                        t += keyword[l];
                        playfair[x, y] = keyword[l];
                        y++;
                        l++;
                    }
                    else
                    {
                        foreach (KeyValuePair<int, char> item in dict)
                        {
                            if (item.Value != 'i' && item.Value != 'j')
                            {
                                if (!t.ToLower().Contains(item.Value))
                                {
                                    playfair[x, y] = item.Value;
                                    t += item.Value;
                                    y++;
                                    break;
                                }
                            }
                            else
                            {
                                if (!t.ToLower().Contains('i') && !t.ToLower().Contains('j'))
                                {
                                    playfair[x, y] = item.Value;
                                    t += item.Value;
                                    y++;
                                    break;
                                }
                            }
                        }
                    }
                }
                x++;
                y = 0;
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    this.richTextBox1.Text += playfair[i, j].ToString().ToUpper() + ",\t";
                }
                this.richTextBox1.Text += "\n";
            }
        }

        public void encrypting(string txt)
        {
            #region --locating position in playfair--
            for (int l = 0; l < txt.Length; l++)
            {
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 5; y++)
                    {
                        if (txt[l] != 'i' && txt[l] != 'j')
                        {
                            if (txt[l] == playfair[x, y])
                            {
                                XS[l] = x;
                                YS[l] = y;
                                found = true;
                                break;
                            }
                        }
                        else
                        {
                            if (playfair[x, y] == 'i' || playfair[x, y] == 'j')
                            {
                                XS[l] = x;
                                YS[l] = y;
                                found = true;
                                break;
                            }
                        }
                    }
                    if (found)
                    {
                        found = false;
                        break;
                    }
                }
            }
            #endregion
            //======================================================================
            //same row
            if (XS[0] == XS[1])
            {
                if (YS[0] + 1 < 5)
                {
                    Cipher_Text += playfair[XS[0], YS[0] + 1];
                }
                else
                {
                    YS[0] += 1;
                    YS[0] = (YS[0] - 5) * (-1);
                    Cipher_Text += playfair[XS[0], YS[0]];
                }
                if (YS[1] + 1 < 5)
                {
                    Cipher_Text += playfair[XS[1], YS[1] + 1];
                }
                else
                {
                    YS[1] += 1;
                    YS[1] = (YS[1] - 5) * (-1);
                    Cipher_Text += playfair[XS[1], YS[1]];
                }
            }
            //======================================================================
            //same column
            else if (YS[0] == YS[1])
            {
                if (XS[0] + 1 < 5)
                {
                    Cipher_Text += playfair[XS[0] + 1, YS[0]];
                }
                else
                {
                    XS[0] += 1;
                    XS[0] = (XS[0] - 5) * (-1);
                    Cipher_Text += playfair[XS[0], YS[0]];
                }
                if (XS[1] + 1 < 5)
                {
                    Cipher_Text += playfair[XS[1] + 1, YS[1]];
                }
                else
                {
                    XS[1] += 1;
                    XS[1] = (XS[1] - 5) * (-1);
                    Cipher_Text += playfair[XS[1], YS[1]];
                }
            }
            //======================================================================
            //else
            else
            {
                Cipher_Text += playfair[XS[0], YS[1]];
                Cipher_Text += playfair[XS[1], YS[0]];
            }
            //======================================================================
        }

        public void decrypting(string txt)
        {
            #region --locating position in playfair--
            for (int l = 0; l < txt.Length; l++)
            {
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 5; y++)
                    {
                        if (txt[l] != 'i' && txt[l] != 'j')
                        {
                            if (txt[l] == playfair[x, y])
                            {
                                XS[l] = x;
                                YS[l] = y;
                                found = true;
                                break;
                            }
                        }
                        else
                        {
                            if (playfair[x, y] == 'i' || playfair[x, y] == 'j')
                            {
                                XS[l] = x;
                                YS[l] = y;
                                found = true;
                                break;
                            }
                        }
                    }
                    if (found)
                    {
                        found = false;
                        break;
                    }
                }
            }
            #endregion
            //======================================================================
            //same row
            if (XS[0] == XS[1])
            {
                if (YS[0] - 1 >= 0)
                {
                    Plain_text += playfair[XS[0], YS[0] - 1];
                }
                else
                {
                    YS[0] -= 1;
                    YS[0] = (YS[0] + 5);
                    Plain_text += playfair[XS[0], YS[0]];
                }
                if (YS[1] - 1 >= 0)
                {
                    Plain_text += playfair[XS[1], YS[1] - 1];
                }
                else
                {
                    YS[1] -= 1;
                    YS[1] = (YS[1] + 5);
                    Plain_text += playfair[XS[1], YS[1]];
                }
            }
            //======================================================================
            //same column
            else if (YS[0] == YS[1])
            {
                if (XS[0] - 1 >= 0)
                {
                    Plain_text += playfair[XS[0] - 1, YS[0]];
                }
                else
                {
                    XS[0] -= 1;
                    XS[0] = (XS[0] + 5);
                    Plain_text += playfair[XS[0], YS[0]];
                }
                if (XS[1] - 1 >= 0)
                {
                    Plain_text += playfair[XS[1] - 1, YS[1]];
                }
                else
                {
                    XS[1] -= 1;
                    XS[1] = (XS[1] + 5);
                    Plain_text += playfair[XS[1], YS[1]];
                }
            }
            //======================================================================
            //else
            else
            {
                Plain_text += playfair[XS[0], YS[1]];
                Plain_text += playfair[XS[1], YS[0]];
            }
            //======================================================================
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
