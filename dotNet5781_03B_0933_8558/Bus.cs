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
            DateOfAbsorption = dateEntry;           //date of starting to work
            License = licenseInput;             
            KMLeftToTravel = 1200; // Assuming every added bus is filled with gas
            Mileage = km;
            MileageAtLastTreat = kmAtLastTreat;
            LastTreatmentDate = dateOfLastTreat;
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
                if (license.Length == 7)            //in case the bus is before 2018
                {
                    prefix = license.Substring(0, 2);       //two first digits
                    middle = license.Substring(2, 3);       //three middle digits
                    suffix = license.Substring(5, 2);       //two last digits
                    result = string.Format("{0}-{1}-{2}", prefix, middle, suffix);
                }
                else
                {                                           //in case the bus is after 2018
                    prefix = license.Substring(0, 3);       //three first digits
                    middle = license.Substring(3, 2);       //two middle digits
                    suffix = license.Substring(5, 3);       //three last digits
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
        //in case the status of the bus is "ready for travel"
        public bool IsReady { get { return Status == BUS_STATUS.READY_FOR_TRAVEL; } set { if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("IsReady")); } } }
        //in case the status of the bus is "dangerous"
        public bool NeedsTreatment { get { if ((Status == BUS_STATUS.DANGEROUS) || (Status == BUS_STATUS.READY_FOR_TRAVEL)) return true; return false; } set { if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("NeedsTreatment")); } } }
        //in case the status of the bus is "need refuel"
        public bool NeedsRefuel { get { if ((Status == BUS_STATUS.NEEDS_REFUEL) || (Status == BUS_STATUS.READY_FOR_TRAVEL)) return true; return false; } set { if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("NeedsRefuel")); } } }

        /// <summary>
        /// the status of the bus situation
        /// </summary>
        private BUS_STATUS status;
        public BUS_STATUS Status { get { return status; } set { status = value; if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Status")); } } }

        public enum BUS_STATUS          //enum for the bus status
        {
            READY_FOR_TRAVEL, NEEDS_REFUEL, DANGEROUS, AT_TRAVEL, AT_TREATMENT, AT_REFUEL
        }

        /// <summary>
        /// update the color status of the bus
        /// </summary>
        private string statusColor;     
        public string StatusColor { get { return statusColor; } set { statusColor = value; if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("StatusColor")); } } }

        public void Update_Status()         //function update the status of the bus after is change

        {       //in case the bus did a treatment so update is status for redy to travel
            if (MileageSinceLastTreat < 20000 && lastTreatmentDate.AddYears(1).CompareTo(MainWindow.useMyRunningDate) > 0 && KMLeftToTravel > 0)
            {
                Status = BUS_STATUS.READY_FOR_TRAVEL;       //update status
                StatusColor = "LightGreen";                 //update status color
                // For the is_enabled buttons:
                IsReady = true;
                NeedsRefuel = true;
                NeedsTreatment = false;
            }
            //in case the bus need a treatment so update is status
            else if (MileageSinceLastTreat >= 20000 || lastTreatmentDate.AddYears(1).CompareTo(MainWindow.useMyRunningDate) <= 0)
            {
                Status = BUS_STATUS.DANGEROUS;      //update status
                StatusColor = "OrangeRed";          //update status color 
                // For the is_enabled buttons:
                NeedsTreatment = true;
                IsReady = false;
                NeedsRefuel = false;
            }
            //in case the bus need a refuel 
            else if (KMLeftToTravel <= 0)
            {
                Status = BUS_STATUS.NEEDS_REFUEL;
                StatusColor = "OrangeRed";
                IsReady = false;
                NeedsRefuel = true;
                NeedsTreatment = false;
            }
        }


        /////////////////////////////// Dates: ///////////////////////////////

        //for showing the date short
        public string AbsorptionShortDate
        {
            get { return dateOfAbsorption.ToShortDateString(); }
            set
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AbsorptionShortDate"));
                }
            }
        }

        private DateTime dateOfAbsorption;
        public DateTime DateOfAbsorption
        {
            get { return dateOfAbsorption; }
            set
            {
                dateOfAbsorption = value;
                AbsorptionShortDate = "0"; // To invoke the property changed of AbsorptionShortDate
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("DateOfAbsorption"));
                }
            }
        }

        //return the short date of the last treatment
        public string TreatmentShortDate
        {
            get { return lastTreatmentDate.ToShortDateString(); }
            set
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("TreatmentShortDate"));
                }
            }
        }

        private DateTime lastTreatmentDate; // treatment date field
        /// <summary>
        /// The function stores the date of the treatment 
        /// </summary>
        public DateTime LastTreatmentDate
        {
            get { return lastTreatmentDate; }
            set
            {
                lastTreatmentDate = value;
                TreatmentShortDate = "0"; // To invoke the property changed of TreatmentShortDate
                Update_Status();
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("LastTreatmentDate")); }
            }
        }
        //return the days until next treatment in a short model
        private int daysUntilNextTreat;
        public int DaysUntilNextTreat
        {
            get { return daysUntilNextTreat; }
            set { daysUntilNextTreat = value; if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("DaysUntilNextTreat")); } }
        }

        /////////////////////////////// Mileage and km: ///////////////////////////////

        private double mileage; // Total mileage (kilometraj) field
        /// <summary>
        /// The mileage property - returns the mileage, or sets it with the value (only if it not less than zero)
        /// </summary>
        public double Mileage
        {
            get { return Math.Round(mileage,2); }

            set
            {
                // if (value < 0) throw new Exception("The mileage input is incorrect."); // Throws message if the input is incorrect
                mileage = value;
               
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Mileage")); }
            }
        }

        private double mileageAtLastTreat; // mileage at last treat field
        /// <summary>
        /// Property of mileageAtLastTreat field (simple get and set)
        /// </summary>
        public double MileageAtLastTreat
        {
            //return the mileage at last trearment rounding in to digits after the point
            get { return Math.Round(mileageAtLastTreat,2); }
            set
            {
                mileageAtLastTreat = value;
                MileageSinceLastTreat = 0; // For invoking the MileageSinceLastTreat property
                Update_Status(); if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MileageAtLastTreat")); }
            }
        }

        public double MileageSinceLastTreat
        {
            get { return Math.Round(Mileage - MileageAtLastTreat,2); }
            set
            {
                KMtoNextTreat = 0; // For invoking the KMtoNextTreat property
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MileageSinceLastTreat")); }
            }
        }
        //set and get for thr km to next treatmernt
        public double KMtoNextTreat
        {
            get { if (20000 - MileageSinceLastTreat >= 0) return Math.Round(20000 - MileageSinceLastTreat, 2); return 0; }
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

        private int workEndsIn;
        public int WorkEndsIn
        {
            get { return workEndsIn; }
            set
            {
                workEndsIn = value; // For invoking the KMtoNextTreat property
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("WorkEndsIn")); }
            }
        }

        private BackgroundWorker refuelWorker;  //create a background worker for the refule
        /// <summary>
        /// The function restarts the kmLeftToRide with 1200 km
        /// </summary>
        public void Refuel()            //function fo refuel the buses
        {
            refuelWorker = new BackgroundWorker();              //initialize the background worker
            double part = (1200 - kmLeftToTravel) / 120;        // creatig part of km to add in the progress bar

            refuelWorker.WorkerReportsProgress = true;          

            refuelWorker.ProgressChanged += (sender, args) =>   //the progress changed function
            {
                KMLeftToTravel += part;                         //add fuel to the bus in parts
            };

            refuelWorker.DoWork += (sender, args) =>            //the DoWork function 
            {
                Status = BUS_STATUS.AT_REFUEL;                  //update status "at refule" as long the bus is stiil fueling
                StatusColor = "OrangeRed";
                //disabled the controlers until the refuel is done
                IsReady = false;
                NeedsRefuel = false;
                NeedsTreatment = false;
                for (int i = 0; i < 120; ++i) // 2 hours = 120 minutes
                {
                    if (refuelWorker.CancellationPending == true)
                    {

                    }
                    else
                    {
                        refuelWorker.ReportProgress(0);                     //report progress
                        try { Thread.Sleep(100); } catch (Exception) { }    //sleep 100 milliseconds 120 times (for 2 hours)
                    }
                }
            };

            if (refuelWorker.IsBusy != true)                        //in case the background worker not busy
                refuelWorker.RunWorkerAsync();

            refuelWorker.RunWorkerCompleted += (sender, args) =>        //the Run Worker Completed function
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

        private BackgroundWorker treatmentWorker;       //create a background worker for the treatment

        public void Treatment()                         //function for background worker treatment
        {
            treatmentWorker = new BackgroundWorker();           //initialize the background worker
            treatmentWorker.WorkerReportsProgress = true;

            treatmentWorker.ProgressChanged += (sender, args) =>    //the progress changed function
            {
                WorkEndsIn = 100 * args.ProgressPercentage / 1440;  //treatment takes 24 hours
            };

            treatmentWorker.DoWork += (sender, args) =>         //do work function
            {
                Status = BUS_STATUS.AT_TREATMENT;               //update status to "at treatment" as long the bus is in treatment
                //disabled the controlers until the treatment is done
                IsReady = false;
                NeedsRefuel = false;
                NeedsTreatment = false;
                if (treatmentWorker.CancellationPending == true)
                {

                }
                else
                {
                    for (int i = 1; i < 1441; i++) // 24 hours = 1440 minutes
                    {
                        try { Thread.Sleep(100); } catch (Exception) { }        //sleeps 100 miliseconds for 1441 times(24 hours)
                        treatmentWorker.ReportProgress(i);
                    }
                }
            };

            if (treatmentWorker.IsBusy != true)             //in case worker is busy
                treatmentWorker.RunWorkerAsync();

            treatmentWorker.RunWorkerCompleted += (sender, args) =>     //Run Worker Completed function
            {
                if (args.Cancelled == true)
                {

                }
                else
                {                                     
                    MileageAtLastTreat = Mileage;                           //update mileage after treatment
                    LastTreatmentDate = MainWindow.useMyRunningDate;       //update date
                    DaysUntilNextTreat = 365;                               //update date until next treatment
                    WorkEndsIn = 0;
                    if (this.KMLeftToTravel < 1200)                         //in case the bus needs a refuel also
                        Refuel();
                    else
                        Update_Status();                                    //update status of the bus
                }
            };
        }

        private BackgroundWorker travelWorker;                      //create a background worker for the travel

        public void Travel(double travel, double travelTime)        //function for taking bus to a ride
        {
            travelWorker = new BackgroundWorker();                  //initialize the background worker
            travelWorker.WorkerReportsProgress = true;              //allow background worker to work
            //keep the details of the bus for update after the ride
            double prev1 = Mileage;                             
            double prev2 = MileageSinceLastTreat;
            double prev3 = KMLeftToTravel;

            double minutes = (travel / travelTime) * 60;            //calculate the total minutes of the ride
            double part = travel / minutes;                         //divide the minutes for parts to add in the progras changed func
            double integer = Math.Floor(minutes);                   //the time that the worker need to work

            travelWorker.ProgressChanged += (sender, args) =>   //Progress Changed function    
            {
                Mileage += part;                                //add part to the milage
                MileageSinceLastTreat += part;                  //add part to the milage since last treatment
                if (KMLeftToTravel - part >= 0)                 //in case the bus have enough km to ride
                    KMLeftToTravel -= part;                     //reduce the km left to travel of the bus
                WorkEndsIn = (int)(100 * args.ProgressPercentage / integer);
            };

            travelWorker.DoWork += (sender, args) =>                //do work function
            {
                Status = BUS_STATUS.AT_TRAVEL;                      //update bus status for "at travel"
                StatusColor = "OrangeRed";
                //disabled the controlers until the ride is done
                IsReady = false;
                NeedsTreatment = false;
                NeedsRefuel = false;
                if (travelWorker.CancellationPending == true)
                {

                }
                else
                {
                    for (int i = 1; i < integer + 1; i++)           
                    {
                        try { Thread.Sleep(100); } catch (Exception) { }    //sleeps 100 miliseconds until the ride is done (as long as the minutes of the ride)
                        travelWorker.ReportProgress(i);
                    }
                }
            };

            if (travelWorker.IsBusy != true)                            //in case worker is busy
                travelWorker.RunWorkerAsync();

            travelWorker.RunWorkerCompleted += (sender, args) =>    //Run Worker Completed function
            {
                if (args.Cancelled == true)
                {

                }
                else
                //update the bus
                {
                    Mileage = prev1 + travel;                       //update the mileage
                    MileageSinceLastTreat = prev2 + travel;         //update the mileage since last treatment
                    KMLeftToTravel = prev3 - travel;                //update the km left to travel
                    WorkEndsIn = 0;
                    Update_Status();          
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
        public bool CheckIfCannotTravel(double kmForNextRide)
        {
            if ((MileageSinceLastTreat + kmForNextRide > 20000)         // The bus cannot travel more than 20,000 km since the last treatment
                ||                                                       // OR, if the last treatment happened more than a year a ago 
            (lastTreatmentDate.AddYears(1).CompareTo(DateTime.Now) <= 0)
            ||
            KMLeftToTravel < kmForNextRide)                       // To check this, we add a year to the last treatment date, and compare it to the current date. If the value returned is 0 or -1, it means a year (or more) passed since the last treatment
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
