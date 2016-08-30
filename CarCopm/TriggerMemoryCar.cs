using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CarCopm
{
    class TriggerMemoryCar : Car
    {

        public TriggerMemoryCar(PointF pos, Bitmap m)
            : base(pos, m)
        {
            createSensor(20, -0.8f); //left
            createSensor(20, 0.8f);  //right

            color = Brushes.Green;
        }

        float weight = 1.3f;
        float dw = 0.0001f;
        float comboL = 1f;        
        float comboR = 1f;
        float maxCobmo = 0.2f;
        
        public override void AfterTick()
        {
            float ac = 0.1f;

            //checking for combo
            if (sensorsValues[0] == 1f)
            {
                comboL+=dw;
                if (comboL > maxCobmo) comboL = maxCobmo;
            }
            else comboL = 1;            

            if (sensorsValues[1] == 1f)
            {
                comboR+=dw;
                if (comboR > maxCobmo) comboR = maxCobmo;
            }
            else comboR = 1;

            float r = (sensorsValues[0] * comboL + sensorsValues[1] * (-1f) * comboR) * weight;
            setRotateDelta(r);
            if (r == 0) compenseRotation();
            if (moving.length > basicAcc) ac = -0.1f;
            setAcc(ac);
        }
    }
}
