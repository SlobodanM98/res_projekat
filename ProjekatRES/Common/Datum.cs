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
            if (datumBaza == null)
            {
                throw new ArgumentNullException("Datum nesme biti null.");
            }
            if(!proveri(datumBaza)) {
                throw new ArgumentException("Nije pravilan format datuma.");
            }

            DatumBaza = datumBaza.Trim();
        }

        private bool proveri(string datum)
        {
            bool rez = false;
            string datum2 = datum.Trim();
            string[] niz = datum.Split('/');
            if(niz.Count() == 3)
            {
                try
                {
                    if (int.Parse(niz[0]) >= 1 && int.Parse(niz[0]) <= 31 && int.Parse(niz[1]) >= 1 && int.Parse(niz[1]) <= 12)
                    {
                        rez = true;
                    }
                }
                catch
                {
                    rez = false;
                }
            }

            return rez;
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
