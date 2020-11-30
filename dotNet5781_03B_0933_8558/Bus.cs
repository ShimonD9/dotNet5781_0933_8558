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
            KMLeftToTravel = 1200; // Assuming every added bus is filled with gas
            Update_Status();
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

        /////////////////////////////// Status:  ///////////////////////////////

        public bool IsReady { get { return Status == BUS_STATUS.READY_FOR_TRAVEL; } set { if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("IsReady")); } } }

        public bool NeedsTreatment { get { if ((Status == BUS_STATUS.DANGEROUS) || (Status == BUS_STATUS.READY_FOR_TRAVEL)) return true; return false; } set { if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("NeedsTreatment")); } } }

        private BUS_STATUS status;
        public BUS_STATUS Status { get { return status; } set { status = value; if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Status")); } } }

        public enum BUS_STATUS
        {
            READY_FOR_TRAVEL, NOT_READY_FOR_TRAVEL, DANGEROUS, AT_TRAVEL, AT_TREATMENT, AT_REFUEL
        }

        public void Update_Status()
        {
            if (MileageSinceLastTreat < 20000 && lastTreatmentDate.AddYears(1).CompareTo(DateTime.Now) > 0 && KMLeftToTravel > 0)
            {
                Status = BUS_STATUS.READY_FOR_TRAVEL;
                IsReady = true;
                NeedsTreatment = false;
            }
            else if (MileageSinceLastTreat > 20000 || lastTreatmentDate.AddYears(1).CompareTo(DateTime.Now) <= 0)
                Status = BUS_STATUS.DANGEROUS;
            else if (KMLeftToTravel < 0)
                Status = BUS_STATUS.NOT_READY_FOR_TRAVEL;
        }


        /////////////////////////////// Dates: ///////////////////////////////

        private DateTime dateOfAbsorption;
        public DateTime DateOfAbsorption
        {
            get { return dateOfAbsorption; }
            set
            {
                dateOfAbsorption = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("DateOfAbsorption"));
                }
            }
        }

        private DateTime lastTreatmentDate; // treatment date field
        /// <summary>
        /// The function stores the date of the treatment 
        /// </summary>
        public DateTime LastTreatmentDate
        {
            get { return lastTreatmentDate.Date; }
            set
            {
                lastTreatmentDate = value;
                Update_Status();
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("LastTreatmentDate")); }
            }
        }

        public int DaysUntilNextTreat
        {
            get { return (lastTreatmentDate.AddYears(1) - DateTime.Now).Days; }
            set { if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("DaysUntilNextTreat")); } }
        }

        /////////////////////////////// Mileage and km: ///////////////////////////////

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

        private double mileageAtLastTreat; // mileage at last treat field
        /// <summary>
        /// Property of mileageAtLastTreat field (simple get and set)
        /// </summary>
        public double MileageAtLastTreat
        {
            get { return mileageAtLastTreat; }
            set
            {
                mileageAtLastTreat = value;
                MileageSinceLastTreat = 0; // For invoking the MileageSinceLastTreat property
                Update_Status(); if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MileageAtLastTreat")); }
            }
        }

        public double MileageSinceLastTreat
        {
            get { return Math.Round(Mileage - MileageAtLastTreat, 2); }
            set
            {
                KMtoNextTreat = 0; // For invoking the KMtoNextTreat property
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MileageSinceLastTreat")); }
            }
        }

        public double KMtoNextTreat
        {
            get { return Math.Round(20000 - MileageSinceLastTreat, 2); }
            set { if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("KMtoNextTreat")); } }
        }

        private double kmLeftToTravel; // kmLeftToRide (equivalent to fuel condition)
        /// <summary>
        /// The property of the kmLeftToRide field
        /// </summary>
        public double KMLeftToTravel
        {
            get { return Math.Round(kmLeftToTravel, 2); }
            set
            {
                kmLeftToTravel = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("KMLeftToTravel")); }
            }
        }


        /////////////////////////////// Background workers: ///////////////////////////////

        private readonly BackgroundWorker refuelWorker = new BackgroundWorker();
        /// <summary>
        /// The function restarts the kmLeftToRide with 1200 km
        /// </summary>
        public void Refuel()
        {
            double part = (1200 - kmLeftToTravel) / 12;

            refuelWorker.WorkerReportsProgress = true;

            refuelWorker.ProgressChanged += (sender, args) =>
            {
                KMLeftToTravel += part;
            };

            refuelWorker.DoWork += (sender, args) =>
            {
                Status = BUS_STATUS.AT_REFUEL;
                IsReady = false;
                for (int i = 0; i < 12; ++i)
                {
                    if (refuelWorker.CancellationPending == true)
                    {

                    }
                    else
                    {
                        refuelWorker.ReportProgress(i);
                        try { Thread.Sleep(1000); } catch (Exception) { }
                    }
                }
            };

            if (refuelWorker.IsBusy != true)
                refuelWorker.RunWorkerAsync();

            refuelWorker.RunWorkerCompleted += (sender, args) =>
            {
                if (args.Cancelled == true)
                {

                }
                else
                {
                    KMLeftToTravel = 1200; // Rounding it to 1200 in case the sum up is 1999.5 for example (it might happen because of the rounding in the KMLeftToRide getter)
                    Update_Status();
                }
            };
        }

        private readonly BackgroundWorker treatmentWorker = new BackgroundWorker();

        public void Treatment()
        {

            treatmentWorker.WorkerReportsProgress = true;

            treatmentWorker.ProgressChanged += (sender, args) =>
            {

            };

            treatmentWorker.DoWork += (sender, args) =>
            {
                Status = BUS_STATUS.AT_TREATMENT;
                IsReady = false;
                NeedsTreatment = false;
                if (treatmentWorker.CancellationPending == true)
                {

                }
                else
                {
                    for (int i = 5; i > 0; i--)
                    {
                        try { Thread.Sleep(1000); } catch (Exception) { }
                        treatmentWorker.ReportProgress(i);
                    }
                }
            };

            if (treatmentWorker.IsBusy != true)
                treatmentWorker.RunWorkerAsync();

            treatmentWorker.RunWorkerCompleted += (sender, args) =>
            {
                if (args.Cancelled == true)
                {

                }
                else
                {
                    if (this.KMLeftToTravel < 1200)
                        Refuel();
                    MileageAtLastTreat = Mileage;
                    LastTreatmentDate = DateTime.Now;
                    DaysUntilNextTreat = 365;
                }
            };
        }

        /////////////////////////////// Other methods: ///////////////////////////////

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

        /// <summary>
        /// Formats a string which represents the Bus object
        /// </summary>
        /// <returns> Returns the string to print the object </returns>
        public override string ToString()
        {
            return string.Format("License = {0}, Date = {1}, Last treatment date = {2}, KM left to ride = {3} km, Total mileage = {4} km, Mileage since last treatment = {5} km", License, DateOfAbsorption.ToShortDateString(), lastTreatmentDate.ToShortDateString(), KMLeftToTravel, Mileage, MileageSinceLastTreat);
        }

    }
}
