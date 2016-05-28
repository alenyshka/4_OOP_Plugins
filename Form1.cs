using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using XML.serializaton.Factories;
using XML.serializaton.Decorations;
using System.Xml.Serialization;
using System.Reflection;


namespace XML.serializaton
{

    public partial class Form1 : Form
    {
        //const int count_of_types = 6;
        public List<Factory> factory = new List<Factory>();
        public List<Type> extraTypes = new List<Type>();
        public List<DecorationClass> labels = new List<DecorationClass>();

        List<DecorationClass> DecorationList = new List<DecorationClass>();        
        List<string> FieldList = new List<string>();
        List<TextBox> TextBoxList = new List<TextBox>();
        int i;
        bool flagEdit = false, flagDelete = false;

        public Form1()
        {
            InitializeComponent();

            InitializeFabrika();
            InitializeTypes();
            InitializeLabels();
           
            TextBoxList.Add(textBox1);
            TextBoxList.Add(textBox2);
            TextBoxList.Add(textBox3);
            TextBoxList.Add(textBox4);
            TextBoxList.Add(textBox5);

            ICollection<IPlugin> plugins = XML.serializaton.PluginLoader.LoadPlugins(Constants.PluginsPath);
            if (plugins != null)
            {
                foreach (IPlugin plugin in plugins)
                {
                    plugin.Run(this);
                }
                textBoxInfo.Text = "The plugins were loaded successful!";
                
            }
            else
            {
                textBoxInfo.Text = "The plugins don't include.";
            }
            
        }
        void InitializeFabrika()
        {
            factory.Add(new EarringsFactory());
            factory.Add(new RingFactory());
            factory.Add(new ChainFactory());
            factory.Add(new CoulombFactory());
            factory.Add(new WatchesFactory());
            factory.Add(new PinFactory());
        }
        void InitializeTypes()
        {
            extraTypes.Add(typeof(Earrings));
            extraTypes.Add(typeof(Ring));
            extraTypes.Add(typeof(Chain));
            extraTypes.Add(typeof(Coulomb));
            extraTypes.Add(typeof(Watches));
            extraTypes.Add(typeof(Pin));
        }

        void InitializeLabels()
        {
            labels.Add(new Earrings());
            labels.Add(new Ring());
            labels.Add(new Chain());
            labels.Add(new Coulomb());
            labels.Add(new Watches());
            labels.Add(new Pin());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if ((DecorationList.Count > 0) && (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    XmlSerializer mySerializer = new XmlSerializer(typeof(List<DecorationClass>), extraTypes.ToArray());

                    using (FileStream file = new FileStream("decoration.xml", FileMode.Truncate))
                    {
                        mySerializer.Serialize(file, DecorationList);
                        file.Close();
                    }
                    textBoxInfo.Text = "Serialization completed successfully.\r\n";
                }
                else
                    textBoxInfo.Text = "The list doesn't contain objects\r\n";
            }
            catch
            {
                textBoxInfo.Text = "Check the object(s) in list\r\n";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FileStream fs = new FileStream("decoration.xml", FileMode.Open);
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<DecorationClass>), extraTypes.ToArray());
                    DecorationList.Clear();
                    List<DecorationClass> Decoration = (List<DecorationClass>)xmlSerializer.Deserialize(fs);

                    foreach (DecorationClass element in Decoration)
                    {
                        listBox1.Items.Add(enter_item(Convert.ToString(element.Object), Convert.ToString(element.Name),
                            Convert.ToString(element.Weight), Convert.ToString(element.Material)));
                        DecorationList.Add(element);
                    }
                    fs.Close();

                    textBoxInfo.Text = "Deserialization completed successfully\r\n";
                }
            }
            
            catch
            {
                textBoxInfo.Text = "Check the file decoration.xml\r\n";
            }  
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            labels[comboBox1.SelectedIndex].SetLabels(this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                FieldList.Clear();
                if(CheckInput())
                {
                    FieldList.Add(comboBox1.GetItemText(comboBox1.SelectedItem));
                    for (i = 1; i <= TextBoxList.Count; i++)
                       if (TextBoxList[i - 1].Text != "")
                            FieldList.Add(TextBoxList[i - 1].Text);
                    if ((radioButton1.Visible == true) && (radioButton2.Visible == true) && ((radioButton1.Checked == true) || (radioButton2.Checked == true)))
                    {
                        if (radioButton1.Checked == true)
                            FieldList.Add(radioButton1.Text);
                        if (radioButton2.Checked == true)
                            FieldList.Add(radioButton2.Text);
                    }
                    DecorationClass decoration = factory[comboBox1.SelectedIndex].FactoryMethod();
                    decoration.SetValues(FieldList);
                    DecorationList.Add(decoration);
                    listBox1.Items.Add(enter_item(Convert.ToString(decoration.Object), Convert.ToString(decoration.Name),
                        Convert.ToString(decoration.Weight), Convert.ToString(decoration.Material)));
                    textBoxInfo.Text = "The object added.\r\n";
                }

            }
            catch 
            {
                textBoxInfo.Text = "The object isn't added.\r\n";
            }
        }
        bool CheckInput()
        {
            if ((textBox1.Text != "") && (textBox2.Text != "") && (textBox3.Text != "") && (textBox4.Text != ""))
            {
                if ((textBox5.Visible == true) && (textBox5.Text != ""))
                    return true;
                else if ((radioButton1.Visible == true) && (radioButton2.Visible == true) && ((radioButton1.Checked == true) || (radioButton2.Checked == true)))
                    return true;
                else
                    return false;
            }
            else
            {
                textBoxInfo.Text = "Uncorrected input.";
                return false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex != -1)
                {
                    DecorationList.Remove(DecorationList[listBox1.SelectedIndex]);
                    flagDelete = true;
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                    flagDelete = false;
                    textBoxInfo.Text = "Object deleted.\r\n";
                }
                else
                    textBoxInfo.Text = "Choose the object.\r\n";
            }
            catch
            {
                textBoxInfo.Text = "Choose the object.\r\n";
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FieldList.Clear();
                if ((!flagEdit) && (!flagDelete))
                {
                    comboBox1.Text = DecorationList[listBox1.SelectedIndex].Object;
                    DecorationList[listBox1.SelectedIndex].GetValues(FieldList);
                    for (i = 0; i < FieldList.Count-1; i++)
                        TextBoxList[i].Text = FieldList[i+1];
                    if ((radioButton1.Visible == true) && (radioButton2.Visible == true))
                    {
                        if (Convert.ToString(FieldList[i]) == "True")
                        {
                            radioButton1.Checked = true;
                            radioButton2.Checked = false;
                        }
                        if (Convert.ToString(FieldList[i]) == "False")
                        {
                            radioButton1.Checked = false;
                            radioButton2.Checked = true;
                        }
                    }
               }
            }
            catch
            {
                textBoxInfo.Text = "You didn't choose the object.\r\n";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (CheckInput())
                {
                    FieldList.Clear();
                    FieldList.Add(comboBox1.GetItemText(comboBox1.SelectedItem));
                    for (i = 1; i <= TextBoxList.Count - 1; i++)
                        if (TextBoxList[i - 1].Text != "")
                            FieldList.Add(TextBoxList[i - 1].Text);
                    if ((radioButton1.Visible == true) && (radioButton2.Visible == true) && ((radioButton1.Checked == true) || (radioButton2.Checked == true)))
                    {
                        if (radioButton1.Checked == true)
                            FieldList.Add(radioButton1.Text);
                        if (radioButton2.Checked == true)
                            FieldList.Add(radioButton2.Text);
                    }
                    DecorationList[listBox1.SelectedIndex].Object = comboBox1.GetItemText(comboBox1.SelectedItem);
                    DecorationList[listBox1.SelectedIndex].SetValues(FieldList);
                    flagEdit = true;
                    listBox1.Items[listBox1.SelectedIndex] = enter_item(
                        Convert.ToString(DecorationList[listBox1.SelectedIndex].Object),
                        Convert.ToString(DecorationList[listBox1.SelectedIndex].Name),
                        Convert.ToString(DecorationList[listBox1.SelectedIndex].Weight),
                        Convert.ToString(DecorationList[listBox1.SelectedIndex].Material)
                        );
                    flagEdit = false;
                    textBoxInfo.Text = "The object edited.\r\n";
                }
                else
                {
                    textBoxInfo.Text = "Check the input values\r\n";
                }
            }
            else
            {
                textBoxInfo.Text = "Add the object to list.\r\n";
            }
        }

        string enter_item(string type_object, string name, string weight, string material)
        {
            string result_string = "Object = " + type_object + "    Name = " + name + "   Weight = " + weight + "   Material = " + material;
            return result_string;
        }
    }
}
