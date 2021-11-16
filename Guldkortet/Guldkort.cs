using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Guldkortet
{
    public class Guldkort
    {
        public string cardNumber;
        public string cardType;
        //Konstruktor för guldkortet
        public Guldkort(string _cardNumber, string _cardType)
        {
            cardNumber = _cardNumber;
            cardType = _cardType;
        }
       
        public virtual string ToString(string customerName, string customerCity)
        {
            return "Tyvär det här kortet är inte ett guldkort." ;
        }
    }
}
