using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CarCopm
{
    abstract class Car
    {
        public PointF pos;
        public Vector moving, acc, brakes;
        public float maxSpeed, maxAcc, rotateRation, maxRR, maxRSpeed, brakesPower;
        public float rd, rs; //rotate delta, rotate speed
        public bool leftS, rightS, brakesEnabled;

        public Brush color;

        public List<Vector> sensors;
        public float[] sensorsValues;
        private Bitmap map;

        private Random rnd;

        public float basicAcc = 1.5f;

        public Car(PointF p, Bitmap m)
        {
            rnd = new Random();

            moving = new Vector();
            //  moving.alpha = 3f;
            acc = new Vector();

            acc.length = 0f;

            maxSpeed = 0.8f;
            maxAcc = 0.05f;
            rotateRation = 0f;
            maxRR = 1.4f;
            maxRSpeed = 0.02f;

            brakesPower = 0.3f;
            brakesEnabled = false;

            rd = rs = 0f;

            leftS = false;
            rightS = false;

            pos = p;
            moving.startPoint = p;
            moving.endPoint = p;
            acc.startPoint = p;
            acc.endPoint = p;

            map = m;

            sensors = new List<Vector>();
            sensorsValues = new float[0];
        }

        abstract public void AfterTick();

        public void setSensValue(bool l, bool r)
        {
            leftS = l;
            rightS = r;            
        }

        public void setBrakes(bool enabled)
        {
            brakesEnabled = enabled;
        }

        public void createSensor(float length, float alpha)
        {
            Vector sv = new Vector(pos, length, moving.alpha);
            sv.ConstAlpha = alpha;
            sensors.Add(sv);

            sensorsValues = new float[sensors.Count];            
        }

        int index = 0;
        float brakesCurrP = 0f;
        public bool Tick()
        {            
            rotateRation += rd + (float)(rnd.NextDouble()-0.5)/100f;
            if (Math.Abs(rotateRation) > maxRR) rotateRation = maxRR * Math.Sign(rotateRation);

            acc.startPoint = pos;
            acc.alpha = moving.alpha;
            acc.setEndPoint();

            if (brakesEnabled)
            {
                brakesCurrP = brakesPower;
                if (brakesCurrP > Math.Abs(moving.length)) brakesCurrP = moving.length;
                moving.length -= brakesCurrP;
                acc.length = 0f;
                acc.setEndPoint();
                moving.setEndPoint();
            }

            moving.startPoint = pos;            
            moving = moving.GetSumWith(acc);
            moving.alpha += rotateRation;
            moving.setEndPoint();
            pos = moving.endPoint;

            moving.startPoint = pos;
            moving.setEndPoint();

            index = 0;
            Color c;
            foreach (Vector s in sensors)
            {
                s.startPoint = pos;
                s.alpha = s.ConstAlpha + moving.alpha;
                s.setEndPoint();

                try
                {
                    c = map.GetPixel((int)s.endPoint.X, (int)s.endPoint.Y);
                }
                catch (Exception e)
                {
                    return false;
                }
                sensorsValues[index] = Convert.ToSingle(!(((int)c.A + (int)c.G + (int)c.B) < 300));

                index++;
            }

            AfterTick();

            return true;
        }

        public void compenseRotation()
        {
            setRotateDelta(-rotateRation);
        }

        public void setRotateDelta(float dr)
        {
            rd = dr;
            if (Math.Abs(rd) > maxRSpeed) rd = maxRSpeed * Math.Sign(rd);
        }

        public void setAcc(float ac)
        {
            acc.length = ac;
            if (Math.Abs(acc.length) > maxAcc) acc.length = maxAcc * Math.Sign(acc.length);
        }
    }
}
