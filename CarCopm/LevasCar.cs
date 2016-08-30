using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CarCopm
{
    class LevasCar : Car
    {
        public LevasCar(PointF pos, Bitmap m) : base (pos, m)
        {
            setAcc(0.1f);
            //setRotateDelta(0.001f);

            createSensor(20, -0.8f); //left
            createSensor(20, 0.8f);  //right

            color = Brushes.Coral;
        }

        float weight = 1.2f;
        public override void AfterTick()
        {
            float ac = 0.01f;
            /*if (!sensorsValues[0])
            {
                if (moving.length > 0.5f) setBrakes(true);
                else setBrakes(false);
                ac = -0.5f;
            }
            else ac = 0.2f;*/

            float r = (sensorsValues[0] + sensorsValues[1] * (-1f)) * weight;
            setRotateDelta(r);
            if (r == 0) compenseRotation();
            if (moving.length > basicAcc) ac = -0.1f;
            setAcc(ac);
        }
    }
}
