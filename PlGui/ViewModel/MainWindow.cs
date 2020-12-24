using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using BLApi;

namespace ViewModel
{
    public class MainWindow : DependencyObject
    {
        IBL bl = BLFactory.GetBL("1");

        static readonly DependencyProperty BusProperty = DependencyProperty.Register("Bus", typeof(PO.Bus), typeof(MainWindow));
        public PO.Bus Bus { get => (PO.Bus)GetValue(BusProperty); set => SetValue(BusProperty, value); }

        public BO.Bus BusBO
        {
            set
            {
                if (value == null)
                    Bus = new PO.Bus();
                else
                {
                    value.DeepCopyTo(Bus);
                    //Student.ID = value.ID;
                    ////...
                    //Student.ListOfCourses.Clear();
                    //foreach (var fromCourse in value.ListOfCourses)
                    //{
                    //    PO.StudentCourse toCourse = new PO.StudentCourse();
                    //    toCourse.Grade = fromCourse.Grade;
                    //    toCourse. Number = fromCourse.Number;
                    //    // ...
                    //    Student.ListOfCourses.Add(toCourse);
                    //}
                }
                // update more properties in Student if needed... That is, properties that don't appear as is in studentBO...
            }
        }

        public MainWindow() => Reset();

        BackgroundWorker getBusWorker;
        internal void blGetBus(int license)
        {
            if (getBusWorker != null)
                getBusWorker.CancelAsync();
            getBusWorker = new BackgroundWorker();
            getBusWorker.WorkerSupportsCancellation = true;
            getBusWorker.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs args) =>
            {
                if (!((BackgroundWorker)sender).CancellationPending)
                    BusBO = (BO.Bus)args.Result;
            };
            getBusWorker.DoWork += (object sender, DoWorkEventArgs args) =>
            {
                BackgroundWorker worker = (BackgroundWorker)sender;
                object bus = bl.GetBus((int)args.Argument);
                args.Result = worker.CancellationPending ? null : bus;
            };
            getBusWorker.RunWorkerAsync(license);
        }

        internal void Reset()
        {
            if (getBusWorker != null)
            {
                getBusWorker.CancelAsync();
                getBusWorker = null;
            }
            if (getBusIDsWorker != null)
            {
                getBusIDsWorker.CancelAsync();
                getBusIDsWorker = null;
            }
            Bus = new PO.Bus();
            //blGetBusIDs();
        }

        BackgroundWorker getBusIDsWorker;

        //public void blGetBusIDs()
        //{
        //    getBusIDsWorker = new BackgroundWorker();
        //    getBusIDsWorker.WorkerSupportsCancellation = true;
        //    getBusIDsWorker.WorkerReportsProgress = true;
        //    getBusIDsWorker.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs args) => getBusIDsWorker = null;
        //    getBusIDsWorker.ProgressChanged += (object sender, ProgressChangedEventArgs args) =>
        //    {
        //        if (!((BackgroundWorker)sender).CancellationPending)
        //            StudentIDs.Add(new PO.ListedPerson() { Person = (BO.ListedPerson)args.UserState });
        //    };
        //    getBusIDsWorker.DoWork += (object sender, DoWorkEventArgs args) =>
        //    {
        //        BackgroundWorker worker = (BackgroundWorker)sender;
        //        foreach (var item in bl.GetStudentIDNameList())
        //        {
        //            if (worker.CancellationPending) break;
        //            worker.ReportProgress(0, item);
        //        }
        //    };
        //    StudentIDs = new ObservableCollection<PO.ListedPerson>();
        //    getBusIDsWorker.RunWorkerAsync();
        //}
    }
}
