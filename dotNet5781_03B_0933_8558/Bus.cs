using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace dotNet5781_03B_0933_8558
{
    public class Bus : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private BackgroundWorker refuelWorker = new BackgroundWorker();
        private Thread myThread = null;

        /// <summary>
        /// Bus constructor, initializing 6 fields in the ctor, based on 3 paramters
        /// </summary>
        /// <param name="dateInput"></param>
        /// <param name="licenseInput"></param>
        /// <param name="km" - equals 0 if not initialized by user></param>
        public Bus(string licenseInput, double km, double kmAtLastTreat, DateTime dateEntry, DateTime dateOfLastTreat) // Bus constructor
        {
            DateOfAbsorption = dateEntry;
            License = licenseInput;
            Mileage = km;
            LastTreatmentDate = dateOfLastTreat;
            MileageAtLastTreat = kmAtLastTreat;
            KMLeftToRide = 1200; // Assuming every added bus is filled with gas
            Update_Status();
        }

        public bool IsReady { get { return Status == BUS_STATUS.READY_FOR_TRAVEL;  } set { } }

        private BUS_STATUS status;
        public BUS_STATUS Status { get { return status; } set { status = value; if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Status")); } } }

        public enum BUS_STATUS
        {
            READY_FOR_TRAVEL, NOT_READY_FOR_TRAVEL, DANGEROUS, AT_TRAVEL, AT_TREATMENT, AT_REFUEL 
        }

        public void Update_Status()
        {
            if (MileageSinceLastTreat < 20000 && lastTreatmentDate.AddYears(1).CompareTo(DateTime.Now) > 0 && KMLeftToRide > 0)
                Status = BUS_STATUS.READY_FOR_TRAVEL;
            else if (MileageSinceLastTreat > 20000 || lastTreatmentDate.AddYears(1).CompareTo(DateTime.Now) <= 0)
                Status = BUS_STATUS.DANGEROUS;
            else if (KMLeftToRide < 0)
                Status = BUS_STATUS.NOT_READY_FOR_TRAVEL;
        }

        private String license;
        /// <summary>
        /// License property.
        /// The getter returns string of right formatted license in accordance to the year and the number of digits.
        /// The setter puts the value in the license field, not before checking if the input is appropriate
        /// </summary>
        public String License
        {
            get
            {
                // Firstly - cutting the string to 3 parts, and then formatting them into a new one, adding dashes
                string prefix, middle, suffix, result;
                if (license.Length == 7)
                {
                    prefix = license.Substring(0, 2);
                    middle = license.Substring(2, 3);
                    suffix = license.Substring(5, 2);
                    result = string.Format("{0}-{1}-{2}", prefix, middle, suffix);
                }
                else
                {
                    prefix = license.Substring(0, 3);
                    middle = license.Substring(3, 2);
                    suffix = license.Substring(5, 3);
                    result = string.Format("{0}-{1}-{2}", prefix, middle, suffix);
                }
                return result;
            }

            private set
            {
                if (DateOfAbsorption.Year >= 2018 && value.Length == 8) // 8 digits only after 2018
                {
                    license = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("License")); }
                }
                else if (DateOfAbsorption.Year < 2018 && value.Length == 7) // 7 digits only before 2018
                {
                    license = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("License")); }
                }
                else
                {
                    throw new Exception("The license or year number is incorrect."); // Throws message if the input is incorrect
                }
            }
        }

        private double mileage; // Total mileage (kilometraj) field
        /// <summary>
        /// The mileage property - returns the mileage, or sets it with the value (only if it not less than zero)
        /// </summary>
        public double Mileage
        {
            get { return Math.Round(mileage, 2); }

            set
            {
                // if (value < 0) throw new Exception("The mileage input is incorrect."); // Throws message if the input is incorrect
                mileage = value;
                Update_Status();
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Mileage")); }
            }
        }

        private double kmLeftToRide; // kmLeftToRide (equivalent to fuel condition)
        /// <summary>
        /// The property of the kmLeftToRide field -
        /// the getters returns the field,
        /// and the setter reduces the kilometers by the value, if the ride is possible
        /// if the km left is not enough for the new ride, it throws appropriate message
        /// </summary>
        public double KMLeftToRide
        {
            get { return Math.Round(kmLeftToRide, 2); }
            set
            {
                kmLeftToRide = value;                
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("KMLeftToRide")); }
            }
        }

        /// <summary>
        /// The function restarts the kmLeftToRide with 1200 km
        /// </summary>
        public void Refuel()
        {
            Status = BUS_STATUS.AT_REFUEL;
            double part = (1200 - KMLeftToRide) / 12;
            //refuelWorker.RunWorkerCompleted += (sender, args) => accountClosedHandler();
            refuelWorker.WorkerReportsProgress = true;
            refuelWorker.ProgressChanged += (sender, args) =>
            {            
                KMLeftToRide += part;
            };

            refuelWorker.DoWork += (sender, args) =>
            {
                myThread = Thread.CurrentThread;          
                //while (!worker.CancellationPending) //(!_shouldStop)
                //{
                //    worker.ReportProgress(1);
                //    try { Thread.Sleep(3000); } catch (Exception) { } // 3 secs
                //}
                for (int i = 0; i < 12; ++i) // 5 secs delay
                {
                    refuelWorker.ReportProgress(i);
                    Thread.Sleep(1000);
                }
            };
            refuelWorker.RunWorkerAsync();
            Update_Status();
        }


        private DateTime dateOfAbsorption;
        public DateTime DateOfAbsorption { get { return dateOfAbsorption; } 
            set { dateOfAbsorption = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("DateOfAbsorption"));
                } } }


        private DateTime lastTreatmentDate; // treatment date field
        /// <summary>
        /// The function stores the date of the treatment 
        /// </summary>
        public DateTime LastTreatmentDate
        {
            get { return lastTreatmentDate.Date; }
            set { lastTreatmentDate = value;
                Update_Status(); 
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("LastTreatmentDate")); }
            }
        }

        public int DaysUntilNextTreat
        {
            get { return (lastTreatmentDate.AddYears(1) - DateTime.Now).Days; }
            set { if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("DaysUntilNextTreat")); } }
        }


        private double mileageAtLastTreat; // mileage at last treat field
        /// <summary>
        /// Property of mileageAtLastTreat field (simple get and set)
        /// </summary>
        public double MileageAtLastTreat
        {
            get { return mileageAtLastTreat; }
            set { mileageAtLastTreat = value; Update_Status(); if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MileageAtLastTreat")); } }
        }

        /// <summary>
        /// The function decides if the bus can travel, based on the mileage or the time passed since the last treatment
        /// </summary>
        /// <param name="kmForNextRide" - used to decide if the bus can travel the requested km></param>
        /// <returns></returns>
        public bool CheckIfCannotTravel(double kmForNextRide = 0)
        {
            if ((MileageSinceLastTreat + kmForNextRide > 20000)         // The bus cannot travel more than 20,000 km since the last treatment
                ||                                                       // OR, if the last treatment happened more than a year a ago 
            (lastTreatmentDate.AddYears(1).CompareTo(DateTime.Now) <= 0))    // To check this, we add a year to the last treatment date, and compare it to the current date. If the value returned is 0 or -1, it means a year (or more) passed since the last treatment
                return true;                                             // Returns true, meaning the bus cannot travel
            return false;                                                // Else, returns false
        }

        public double MileageSinceLastTreat
        {
            get { return Math.Round(Mileage - MileageAtLastTreat, 2); }
            set { if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MileageSinceLastTreat")); } }
        }

        public double KMtoNextTreat
        {
            get { return Math.Round(20000 - MileageSinceLastTreat, 2); }
            set { if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("KMtoNextTreat")); } }
        }

        /// <summary>
        /// The functions compares two license numbers
        /// </summary>
        /// <param name="str"> the given license for comprison</param>
        /// <returns></returns>
        public bool CompareLicenses(String str)
        {
            return (this.license == str);
        }

        /// <summary>
        /// Formats a string which represents the Bus object
        /// </summary>
        /// <returns> Returns the string to print the object </returns>
        public override string ToString()
        {
            return string.Format("License = {0}, Date = {1}, Last treatment date = {2}, KM left to ride = {3} km, Total mileage = {4} km, Mileage since last treatment = {5} km", License, DateOfAbsorption.ToShortDateString(), lastTreatmentDate.ToShortDateString(), KMLeftToRide, Mileage, MileageSinceLastTreat);
        }

    }
}
