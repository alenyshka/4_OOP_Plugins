using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XML.serializaton;
using XML.serializaton.Decorations;
//using System.Windows.Forms;

namespace Necklace
{
    public class Necklace : DecorationClass
    {
        public string Briliant {get; set;}
        public string Disign { get; set; }

        public Necklace()
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
            Disign = FieldList[i];
        }

        public override void SetLabels(Form1 form1)
        {
            base.SetLabels(form1);
            form1.label4.Text = "Briliant:";
            form1.label5.Text = "Disign:";
        }

    }
}
