using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldkortet
{
    class Överpanda : Guldkort
    {
        string cardNumber;
        string cardType = "Överpanda";
        //Konstruktor för Överpanda
        public Överpanda(string _cardNumber, string _cardType)
            : base(_cardNumber, _cardType)
        {
            cardNumber = _cardNumber;
        }
        public override string ToString(string customerName, string customerCity)
        {
            return "Grattis " + customerName + " Från " + customerCity
                + " du  har vunnit " + cardType + " kortet!";
        }
    }
}
