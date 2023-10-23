using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace WpfAlytalo
{
    public class Sauna
        
    {   public Sauna() { Power = false; }
        public Boolean Power { get; set; }
        public Double SaunaTemp { get; set; }
        
       
        public void SaunaOn()
        
        {
            SaunaTemp = SaunaTemp + 0.5;
            
        }
        public void SaunaOff()
        {
            SaunaTemp = SaunaTemp - 1;

        }
        
    }
    
    
}
