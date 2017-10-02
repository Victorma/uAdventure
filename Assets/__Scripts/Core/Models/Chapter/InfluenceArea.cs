using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace uAdventure.Core
{
    /**
     * The influence area for an item reference or active area
     */
    public class InfluenceArea : Rectangle, ICloneable
    {

        /**
         * True if the influence area exists (is defined)
         */
        private bool exists = false;

        /**
         * The x axis value of the influence area, relative to the objects top left
         * corner
         */
        private int x;

        /**
         * The y axis value of the influence area, relative to the objects top left
         * corner
         */
        private int y;

        /**
         * The width of the active area
         */
        private int width;

        /**
         * The height of the active area
         */
        private int height;

        /**
         * Is the area rectangular
         */
        private bool rectangular;

        private List<Vector2> points;

        public InfluenceArea()
        {
            points = new List<Vector2>();
            exists = true;
        }

        /**
         * Creates a new influence area with the given parameters
         * 
         * @param x
         *            The x axis value
         * @param y
         *            The y axis value
         * @param width
         *            The width of the influence area
         * @param height
         *            The height of the influence area
         */
        public InfluenceArea(int x, int y, int width, int height) : this()
        {
            rectangular = true;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        
        public InfluenceArea(List<Vector2d> points) : this()
        {
            rectangular = false;
        }

        /**
         * @return the exists
         */
        public bool isExists()
        {

            return exists;
        }

        /**
         * @param exists
         *            the exists to set
         */
        public void setExists(bool exists)
        {

            this.exists = exists;
        }


        /**
         * Returns the horizontal coordinate of the upper left corner of the exit
         * 
         * @return the horizontal coordinate of the upper left corner of the exit
         */
        public int getX()
        {

            if (rectangular)
                return x;
            else
            {
                int minX = int.MaxValue;
                foreach (Vector2 Vector2 in points)
                {
                    if (Vector2.x < minX)
                        minX = (int)Vector2.x;
                }
                return minX;
            }
        }

        /**
         * @param x
         *            the x to set
         */
        public void setX(int x)
        {

            if (x > 0)
                this.x = x;
        }


        /**
         * Returns the horizontal coordinate of the bottom right of the exit
         * 
         * @return the horizontal coordinate of the bottom right of the exit
         */
        public int getY()
        {

            if (rectangular)
                return y;
            else
            {
                int minY = int.MaxValue;
                foreach (Vector2 Vector2 in points)
                {
                    if (Vector2.y < minY)
                        minY = (int)Vector2.y;
                }
                return minY;
            }
        }

        /**
         * @param y
         *            the y to set
         */
        public void setY(int y)
        {

            if (y > 0)
                this.y = y;
        }


        /**
         * Returns the width of the exit
         * 
         * @return Width of the exit
         */
        public int getWidth()
        {

            if (rectangular)
                return width;
            else
            {
                int maxX = int.MinValue;
                int minX = int.MaxValue;
                foreach (Vector2 Vector2 in points)
                {
                    if (Vector2.x > maxX)
                        maxX = (int)Vector2.x;
                    if (Vector2.x < minX)
                        minX = (int)Vector2.x;
                }
                return maxX - minX;

            }
        }

        /**
         * @param width
         *            the width to set
         */
        public void setWidth(int width)
        {

            if (width > 0)
                this.width = width;
        }

        /**
         * @return the height
         */

        /**
         * Returns the height of the exit
         * 
         * @return Height of the exit
         */
        public int getHeight()
        {

            if (rectangular)
                return height;
            else
            {
                int maxY = int.MinValue;
                int minY = int.MaxValue;
                foreach (Vector2 Vector2 in points)
                {
                    if ((int)Vector2.y > maxY)
                        maxY = (int)Vector2.y;
                    if ((int)Vector2.y < minY)
                        minY = (int)Vector2.y;
                }
                return maxY - minY;
            }

        }

        /**
         * @param height
         *            the height to set
         */
        public void setHeight(int height)
        {

            if (height > 0)
                this.height = height;
        }

        public void setValues(int x, int y, int width, int height)
        {

            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public bool isRectangular()
        {
            return rectangular;
        }

        public void setRectangular(bool rectangular)
        {
            this.rectangular = rectangular;
        }

        public List<Vector2> getPoints()
        {
            return points;
        }

        public void addPoint(Vector2 Vector2)
        {
            points.Add(Vector2);
        }
        
        public Vector2 getLastPoint()
        {
            if (points.Count > 0)
                return points[points.Count - 1];
            return Vector2.zero;
        }

        public void deletePoint(Vector2 point)
        {
            points.Remove(point);
        }



        /**
         * Returns whether a point is inside the exit
         * 
         * @param x
         *            the horizontal positon
         * @param y
         *            the vertical position
         * @return true if the point (x, y) is inside the exit, false otherwise
         */
        public bool isPointInside(int x, int y)
        {

            if (rectangular)
                return x > getX0() && x < getX1() && y > getY0() && y < getY1();
            else
            {
                return PolygonHelper.ContainsPoint(getPoints(), new Vector2(x, y));
            }
        }

        /**
         * Returns the horizontal coordinate of the upper left corner of the exit
         * 
         * @return the horizontal coordinate of the upper left corner of the exit
         */
        public int getX0()
        {

            return getX();
        }

        /**
         * Returns the vertical coordinate of the upper left corner of the exit
         * 
         * @return the vertical coordinate of the upper left corner of the exit
         */
        public int getX1()
        {

            return getX() + getWidth();
        }

        /**
         * Returns the horizontal coordinate of the bottom right of the exit
         * 
         * @return the horizontal coordinate of the bottom right of the exit
         */
        public int getY0()
        {

            return getY();
        }

        /**
         * Returns the vertical coordinate of the bottom right of the exit
         * 
         * @return the vertical coordinate of the bottom right of the exit
         */
        public int getY1()
        {

            return getY() + getHeight();
        }

        public object Clone()
        {
            InfluenceArea ia = (InfluenceArea)this.MemberwiseClone();
            ia.exists = exists;
            ia.height = height;
            ia.width = width;
            ia.x = x;
            ia.y = y;
            return ia;
        }
    }
}