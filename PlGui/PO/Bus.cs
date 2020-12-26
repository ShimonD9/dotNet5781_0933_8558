using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PO
{
    public class Bus : DependencyObject
    {
        static readonly DependencyProperty LicenseProperty = DependencyProperty.Register("License", typeof(int), typeof(Bus));
        static readonly DependencyProperty LicenseDateProperty = DependencyProperty.Register("LicenseDate", typeof(DateTime), typeof(Bus));
        static readonly DependencyProperty MileageProperty = DependencyProperty.Register("Mileage", typeof(double), typeof(Bus));
        static readonly DependencyProperty FuelProperty = DependencyProperty.Register("Fuel", typeof(double), typeof(Bus));
        static readonly DependencyProperty BusStatusProperty = DependencyProperty.Register("BusStatus", typeof(BO.Enums.BUS_STATUS), typeof(Bus));
        static readonly DependencyProperty LastTreatmentDateProperty = DependencyProperty.Register("LastTreatmentDate", typeof(DateTime), typeof(Bus));
        static readonly DependencyProperty MileageAtLastTreatProperty = DependencyProperty.Register("MileageAtLastTreat", typeof(double), typeof(Bus));

        public int License { get => (int)GetValue(LicenseProperty); set => SetValue(LicenseProperty, value); }
        public DateTime LicenseDate { get => (DateTime)GetValue(LicenseDateProperty); set => SetValue(LicenseDateProperty, value); }
        public double Mileage { get => (double)GetValue(MileageProperty); set => SetValue(MileageProperty, value); }
        public double Fuel { get => (double)GetValue(FuelProperty); set => SetValue(FuelProperty, value); }
        public PO.Enums.BUS_STATUS BusStatus { get => (PO.Enums.BUS_STATUS)GetValue(BusStatusProperty); set => SetValue(BusStatusProperty, value); }
        public DateTime LastTreatmentDate { get => (DateTime)GetValue(LastTreatmentDateProperty); set => SetValue(LastTreatmentDateProperty, value); }
        public double MileageAtLastTreat { get => (double)GetValue(MileageAtLastTreatProperty); set => SetValue(MileageAtLastTreatProperty, value); }
    }
}
