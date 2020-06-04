using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Datum : INotifyPropertyChanged
    {
        private string datumBaza;
        [DataMember]
        public string DatumBaza
        {
            get { return datumBaza; }
            set
            {
                datumBaza = value;
                OnPropertyChanged("DatumBaza");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Datum(string datumBaza)
        {
            DatumBaza = datumBaza;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
