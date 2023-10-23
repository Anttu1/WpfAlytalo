using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAlytalo
{
    public class Lights
    {
        public Boolean Switched { get; set; }

        public void LightsOn() 
        {
            Switched = true;
        }

        public void LightsOff() 
        {
            Switched = false;
        }
    }
}
