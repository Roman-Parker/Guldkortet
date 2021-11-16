using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldkortet
{
    class CustomerData
    {
        public string customerRegNumber;
        public string customerName;
        public string customerCity;
        //Konstruktor för CustomerData
        public CustomerData(string _customerRegNumber, string _customerName, string _customerCity)
        {
            customerRegNumber = _customerRegNumber;
            customerName = _customerName;
            customerCity = _customerCity;
        }
    }
}
