using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace PO
{

    // נסיונות של אליעזר המרצה. למחוק אם לא צריך.
    class Bus : INotifyPropertyChanged
    {
        BO.Bus bobus;
        public event PropertyChangedEventHandler PropertyChanged;

        public Bus(BO.Bus bobus)
        {
            this.bobus = bobus;
        }

        public int License
        {
            get => bobus.License;
            set
            {
                if (value != bobus.License)
                {
                    bobus.License = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Mileage
        {
            get => bobus.Mileage;
            set
            {
                if (value != bobus.Mileage)
                {
                    bobus.Mileage = value;
                    OnPropertyChanged();
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
    
