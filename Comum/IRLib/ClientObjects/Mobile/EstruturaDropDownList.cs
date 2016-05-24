using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaDropDownList
    {
        private bool isDefaultValue;
        public bool IsDefaultValue
        {
            get { return isDefaultValue; }
            set { isDefaultValue = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string optionTitle;
        public string OptionTitle
        {
            get { return optionTitle; }
            set { optionTitle = value; }
        }

        private string valor;
        public string Valor
        {
            get { return valor; }
            set { valor = value; }
        }
    }
}
