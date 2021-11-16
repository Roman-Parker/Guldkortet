using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldkortet
{
    class PictureNotFoundException : Exception
    {
        /*Felhantering som berättar för användaren att
        * bilden till det vunna kortet saknas. */
        public PictureNotFoundException() :base()
        {

        }
        public PictureNotFoundException(string message):base(message)
        {

        }
    }
}
