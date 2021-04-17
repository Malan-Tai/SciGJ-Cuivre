using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cuivre.Code
{
    class Quad
    {
        private List<Point> points; //upper left, upper right, lower left, lower right

        public Quad(List<Point> points)
        {
            this.points = points;
        }

        private float Trace(Point p1, Point p2, Point p3)
        {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
        }

        private bool PointInTriangle(Point mousePoint, Point v1, Point v2, Point v3)
        {
            float d1, d2, d3;
            bool has_neg, has_pos;

            d1 = Trace(mousePoint, v1, v2);
            d2 = Trace(mousePoint, v2, v3);
            d3 = Trace(mousePoint, v3, v1);

            has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(has_neg && has_pos);
        }


        public bool Contains(int MouseX, int MouseY)
        {
            Point upperTriangleLowerYPoint;
            Point upperTriangleOtherLowPoint;
            Point upperTriangleHigherPoint;

            Point lowerTriangleHigherYPoint;
            Point lowerTriangleOtherHigherPoint;
            Point lowerTriangleLowerPoint;


            int maxHeightUpperTriangle;//Pour le triangle du haut
            maxHeightUpperTriangle = Math.Min(points[0].Y, points[1].Y);

            int minHeightUpperTriangle;//Pour le triangle du haut
            minHeightUpperTriangle = Math.Max(points[0].Y, points[1].Y);

            int maxHeightLowerTriangle; //Pour le triangle du bas
            maxHeightLowerTriangle = Math.Max(points[2].Y, points[3].Y);

            int minHeightLowerTriangle; //Pour le triangle du bas
            minHeightLowerTriangle = Math.Min(points[2].Y, points[3].Y);

            if ((MouseX >= points[0].X && MouseX <= points[1].X)) //Si la souris est bien contenue dans la largeur du quad
            {
                if ((MouseY >= maxHeightUpperTriangle) && (MouseY <= minHeightLowerTriangle)) //On regarde déjà si on est bien dans le rectangle principal (là où les bords gauches et droites sont parallèles)
                {
                    return true;
                }
                else if (points[0].Y != points[1].Y) //On vérifie qu'on est bien en situation d'avoir un triangle en haut
                {
                    if (maxHeightUpperTriangle == points[0].Y) //Si le point le plus haut du quad est celui de gauche
                    {
                        upperTriangleLowerYPoint = new Point(points[0].X, points[1].Y); //On set le côté inférieur du triangle au Y du point UpperRight, mais au X de UpperLeft 
                        upperTriangleOtherLowPoint = points[1];
                        upperTriangleHigherPoint = points[0];
                    }
                    else
                    {
                        upperTriangleLowerYPoint = new Point(points[1].X, points[0].Y); //Sinon on le set sur le bord droit, au Y du point UpperLeft
                        upperTriangleOtherLowPoint = points[0];
                        upperTriangleHigherPoint = points[1];
                    }

                    if (PointInTriangle(new Point(MouseX, MouseY), upperTriangleHigherPoint, upperTriangleLowerYPoint, upperTriangleOtherLowPoint))
                    {
                        return true;
                    }
                }
                else if (points[2].Y != points[3].Y) //On vérifie qu'on est bien en situation d'avoir un triangle en bas
                {
                    if (maxHeightUpperTriangle == points[2].Y) //Si le point le plus bas du quad est celui de gauche
                    {
                        lowerTriangleHigherYPoint = new Point(points[2].X, points[3].Y); //On set le côté supérieur du triangle au Y du point LowerRight, mais au X de LowerLeft
                        lowerTriangleOtherHigherPoint = points[3];
                        lowerTriangleLowerPoint = points[2];
                    }
                    else
                    {
                        lowerTriangleHigherYPoint = new Point(points[3].X, points[2].Y); //Sinon on le set sur le bord droit, au Y du point LowerLeft
                        lowerTriangleOtherHigherPoint = points[2];
                        lowerTriangleLowerPoint = points[3];
                    }

                    if (PointInTriangle(new Point(MouseX, MouseY), lowerTriangleLowerPoint, lowerTriangleHigherYPoint, lowerTriangleOtherHigherPoint))
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return false;
        }
    }
}
