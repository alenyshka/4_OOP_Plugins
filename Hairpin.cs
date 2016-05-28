using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XML.serializaton;
using XML.serializaton.Decorations;
//using System.Windows.Forms;

namespace Hairpin
{
    public class Hairpin : DecorationClass
    {
        public string Briliant { get; set; }
        public string Form { get; set; }

        public Hairpin()
        { }

        public override void GetValues(List<string> FieldList)
        {
            base.GetValues(FieldList);
            FieldList.Add(Convert.ToString(Briliant));
        }

        public override void SetValues(List<string> FieldList)
        {
            base.SetValues(FieldList);
            int i = 4;
            Briliant = FieldList[i++];
            Form = FieldList[i++];
        }

        public override void SetLabels(Form1 form1)
        {
            base.SetLabels(form1);
            form1.label4.Text = "Briliant:";
            form1.label5.Text = "Form:";
        }

    }
}
