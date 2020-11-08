using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace dotNet5781_02_0933_8558
{
    class BusLinesCollectionIEnumartor : IEnumerator
    {
        int index = -1, counter;
        List<BusLine> busLineList;

        public BusLinesCollectionIEnumartor(List<BusLine> busLineList)
        {
            this.busLineList = busLineList;
            this.counter = busLineList.Count;
        }

        public object Current => (BusLine)busLineList[index];

        public bool MoveNext()
        {
            index++;
            if (index >= counter)
            {
                index = -1;
                return false;
            }
            return true;
        }

        public void Reset()
        {
            index = -1;
        }
    }
}
