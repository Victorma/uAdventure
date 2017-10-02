using UnityEngine;
using System.Collections;

using uAdventure.Core;

namespace uAdventure.Editor
{
    public class DeletePointTool : Tool
    {
        private HasInfluenceArea holder;

        private Vector2 oldPoint;

        private int oldIndex;

        private InfluenceAreaDataControl iadc;

        private InfluenceArea oldInfluenceArea;

        private InfluenceArea newInfluenceArea;

        public DeletePointTool(HasInfluenceArea holder, Vector2 point)
        {

            this.holder = holder;
            this.oldPoint = point;
            this.oldIndex = holder.getInfluenceArea().getPoints().IndexOf(point);
        }

        public DeletePointTool(HasInfluenceArea holder, Vector2 point, InfluenceAreaDataControl iadc)
        {

            this.holder = holder;
            this.oldPoint = point;
            this.oldIndex = holder.getInfluenceArea().getPoints().IndexOf(point);
            this.iadc = iadc;
            oldInfluenceArea = (InfluenceArea)iadc.getContent();
        }

        public override bool canRedo()
        {

            return true;
        }


        public override bool canUndo()
        {

            return true;
        }


        public override bool combine(Tool other)
        {

            return false;
        }


        public override bool doTool()
        {
            var rectangle = holder.getInfluenceArea();
            if (rectangle.isRectangular())
                return false;
            if (rectangle.getPoints().Contains(oldPoint))
            {
                rectangle.getPoints().Remove(oldPoint);

                if (iadc != null && rectangle.getPoints().Count > 3)
                {
                    int minX = int.MaxValue;
                    int minY = int.MaxValue;
                    int maxX = 0;
                    int maxY = 0;
                    foreach (Vector2 point in rectangle.getPoints())
                    {
                        if (point.x > maxX)
                            maxX = (int)point.x;
                        if (point.x < minX)
                            minX = (int)point.x;
                        if (point.y > maxY)
                            maxY = (int)point.y;
                        if (point.y < minY)
                            minY = (int)point.y;
                    }
                    newInfluenceArea = new InfluenceArea();
                    newInfluenceArea.setX(-20);
                    newInfluenceArea.setY(-20);
                    newInfluenceArea.setHeight(maxY - minY + 40);
                    newInfluenceArea.setWidth(maxX - minX + 40);
                    
                    holder.setInfluenceArea(newInfluenceArea);
                    iadc.setInfluenceArea(newInfluenceArea);
                }

                return true;
            }
            return false;
        }


        public override bool redoTool()
        {

            if (holder.getInfluenceArea().getPoints().Contains(oldPoint))
            {
                holder.getInfluenceArea().getPoints().Remove(oldPoint);
                if (iadc != null)
                {
                    holder.setInfluenceArea(newInfluenceArea);
                    iadc.setInfluenceArea(newInfluenceArea);
                }
                Controller.Instance.reloadPanel();
                return true;
            }
            return false;
        }


        public override bool undoTool()
        {

            holder.getInfluenceArea().getPoints().Insert(oldIndex, oldPoint);
            if (iadc != null)
            {
                holder.setInfluenceArea(oldInfluenceArea);
                iadc.setInfluenceArea(oldInfluenceArea);
            }
            Controller.Instance.reloadPanel();
            return true;
        }
    }
}