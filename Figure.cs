using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace LabR4
{
    [Serializable]
    public abstract class Figure
    {
        public Point P1 = new Point(0, 0);
        public Point P2 = new Point(0, 0);
        public int LineSize;    // толщина линии
        public Color LineColor; // цвет линии
        public Color SolidColor; // цвет фона для передачи в brush
        [NonSerialized] public SolidBrush FillColor; // цвет фона фигуры

        Figure(Point point1, Point point2, int LineSize, Color LineColor, Color SolidColor)
        {
            P1 = new Point(point1.X, point1.Y);
            P2 = new Point(point2.X, point2.Y);
            this.LineSize = LineSize;
            this.LineColor = LineColor;
            this.SolidColor = SolidColor;            
        }

        public abstract void Draw(Graphics g);
        public abstract void Dash(Graphics g);
        public abstract void Hide(Graphics g);

        [Serializable]
        public class Rect : Figure
        {
            public Rect(Point point1, Point point2, int LineSize, Color LineColor, Color SolidColor) : base(point1, point2, LineSize, LineColor, SolidColor)
            {
                P1.X = point1.X;
                P1.Y = point1.Y;
                P2.X = point2.X;
                P2.Y = point2.Y;
            }
            public override void Draw(Graphics g)
            {
                Point P3, P4;
                NormalizeRect(P1, P2, out P3, out P4);
                Rectangle rect = Rectangle.FromLTRB(P3.X, P3.Y, P4.X, P4.Y);
                Pen pen = new Pen(LineColor, LineSize);
                SolidBrush FillColor = new SolidBrush(SolidColor);
                if (FillColor != null)
                {
                    g.FillRectangle(FillColor, rect);                    
                }
                g.DrawRectangle(pen, rect);
            }

            public override void Dash(Graphics g)
            {
                Point P3, P4;
                NormalizeRect(P1, P2, out P3, out P4);
                Rectangle rect = Rectangle.FromLTRB(P3.X, P3.Y, P4.X, P4.Y);
                Pen pen = new Pen(LineColor, LineSize);
                SolidBrush FillColor = new SolidBrush(SolidColor);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                if (FillColor != null)
                {
                    g.FillRectangle(FillColor, rect);
                }
                g.DrawRectangle(pen, rect);
            }

            public override void Hide(Graphics g)
            {
                Point P3, P4;
                NormalizeRect(P1, P2, out P3, out P4);
                Rectangle rect = Rectangle.FromLTRB(P3.X, P3.Y, P4.X, P4.Y);
                Pen pen = new Pen(Color.White, LineSize);
                g.DrawRectangle(pen, rect);
            }
        }

        [Serializable]
        public class Elips : Figure
        {
            public Elips(Point point1, Point point2, int LineSize, Color LineColor, Color SolidColor) : base(point1, point2, LineSize, LineColor, SolidColor)
            {
                P1.X = point1.X;
                P1.Y = point1.Y;
                P2.X = point2.X;
                P2.Y = point2.Y;
            }

            public override void Draw(Graphics g)
            {
                Point P3, P4;
                NormalizeRect(P1, P2, out P3, out P4);
                Rectangle rect = Rectangle.FromLTRB(P3.X, P3.Y, P4.X, P4.Y);
                Pen pen = new Pen(LineColor, LineSize);
                SolidBrush FillColor = new SolidBrush(SolidColor);
                if (FillColor != null)
                {
                    g.FillEllipse(FillColor, rect);
                }
                g.DrawEllipse(pen, rect);
            }

            public override void Dash(Graphics g)
            {
                Point P3, P4;
                NormalizeRect(P1, P2, out P3, out P4);
                Rectangle rect = Rectangle.FromLTRB(P3.X, P3.Y, P4.X, P4.Y);
                Pen pen = new Pen(LineColor, LineSize);
                SolidBrush FillColor = new SolidBrush(SolidColor);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                if (FillColor != null)
                {
                    g.FillEllipse(FillColor, rect);
                }
                g.DrawEllipse(pen, rect);
            }

            public override void Hide(Graphics g)
            {
                Point P3, P4;
                NormalizeRect(P1, P2, out P3, out P4);
                Rectangle rect = Rectangle.FromLTRB(P3.X, P3.Y, P4.X, P4.Y);
                Pen pen = new Pen(Color.White, LineSize);
                g.DrawEllipse(pen, rect);
            }
        }

        public void NormalizeRect(Point P1, Point P2, out Point P3, out Point P4)
        {
            P3 = P1;
            P4 = P2;
            if (P1.X > P2.X && P1.Y < P2.Y)
            {
                P3.X = P2.X;
                P3.Y = P1.Y;
                P4.X = P1.X;
                P4.Y = P2.Y;

            }
            if (P1.X > P2.X && P1.Y > P2.Y)
            {
                P3.X = P2.X;
                P3.Y = P2.Y;
                P4.X = P1.X;
                P4.Y = P1.Y;

            }
            if (P1.X < P2.X && P1.Y > P2.Y)
            {
                P3.X = P1.X;
                P3.Y = P2.Y;
                P4.X = P2.X;
                P4.Y = P1.Y;

                if (P1.X < P2.X && P1.Y < P2.Y)
                {
                    P3.X = P1.X;
                    P3.Y = P1.Y;
                    P4.X = P2.X;
                    P4.Y = P2.Y;

                }
            }
        }

        public void SetPoint(int x2, int y2)
        {
            P2.X = x2;
            P2.Y = y2;
        }
    }
}

/*
public class Line : Figure
{
    public Line(Point point1, Point point2) : base(point1, point2)
    {
        P1.X = point1.X;
        P1.Y = point1.Y;
        P2.X = point2.X;
        P2.Y = point2.Y;
    }

    public override void Draw(Graphics g)
    {
        Point P3, P4;
        NormalizeRect(P1, P2, out P3, out P4);
        g.DrawLine(new Pen(Color.Black), P3, P4);
    }
    public override void Dash(Graphics g)
    {
        Point P3, P4;
        NormalizeRect(P1, P2, out P3, out P4);
        Pen pen = new Pen(LineColor, LineSize);
        pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
        pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        g.DrawLine(pen, P3, P4);
    }
    public override void Hide(Graphics g)
    {
        Point P3, P4;
        NormalizeRect(P1, P2, out P3, out P4);
        Pen pen = new Pen(Color.White, 1);
        g.DrawLine(pen, P3, P4);
    }
}
*/