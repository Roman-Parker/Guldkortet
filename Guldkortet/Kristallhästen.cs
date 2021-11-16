using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldkortet
{
    class Kristallhäst : Guldkort
    {
        string cardNumber;
        string cardType = "Kristallhäst";
        //Konstruktor för Kristallhäst
        public Kristallhäst(string _cardNumber, string _cardType)
            : base(_cardNumber, _cardType)
        {
            cardNumber = _cardNumber;
        }
        public override string ToString(string customerName, string customerCity)
        {
            return "Grattis " + customerName + " Från " + customerCity + " du " +
                "har vunnit " + cardType + " kortet!";
        }
    }
}
