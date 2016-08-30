using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CarCopm
{
    class Vector
    {
        public float length, alpha, dx, dy, ConstAlpha;
        public PointF startPoint, endPoint;

        public Vector()
        {
            length = 0;
            alpha = 0;
            startPoint = new PointF(0f, 0f);
            endPoint = new PointF(0f, 0f);
        }

        public Vector(PointF start, PointF end)
        {
            startPoint = start;
            endPoint = end;
            SetLength();
            SetAlpha();
        }

        public Vector(PointF start, float l, float a)
        {
            startPoint = start;
            length = l;
            alpha = a;
            setEndPoint();
        }

        public void SetLength()
        {
            length = (float)Math.Sqrt(Math.Pow(Math.Abs(startPoint.X - endPoint.X),2.0) + Math.Pow(Math.Abs(startPoint.Y - endPoint.Y),2));
            dx = endPoint.X - startPoint.X;
            dy = endPoint.Y - startPoint.Y;
        }

        public void setEndPoint()
        {
            PointF pf = new PointF((float)(this.length * Math.Cos(alpha) + this.startPoint.X), (float)(this.length * Math.Sin(alpha) + this.startPoint.Y));
            endPoint = pf;
        }

        public void SetAlpha()
        {
            float dx = this.endPoint.X - this.startPoint.X;
            float dy = this.endPoint.Y - this.startPoint.Y;
            if (dx == 0f)
            {
                alpha = 0f;
                return;
            }
            float angle = (float)Math.Atan(dy / dx);
            if ((dx < 0 && dy < 0) || (dx < 0 && dy >= 0)) angle = (float)(angle - Math.PI);
            if (angle == null)
            {
                alpha = 0f;
                return;
            }
            alpha = angle;
        }

        public Vector GetSumWith(Vector v2)
        {
            float dx = v2.endPoint.X - v2.startPoint.X;
            float dy = v2.endPoint.Y - v2.startPoint.Y;
            Vector v = new Vector();
            v.startPoint = startPoint;
            v.endPoint = new PointF(endPoint.X+dx,endPoint.Y+dy);
            v.SetLength();
            v.SetAlpha();

            return v;
        }
    }
}
