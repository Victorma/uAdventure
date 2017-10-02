using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace uAdventure.Core
{
    /**
     * This class holds the data of an active area in eAdventure
     */
    public class ActiveArea : Item, HasInfluenceArea, ICloneable
    {

        /**
         * Conditions of the active area
         */
        private Conditions conditions;

        private InfluenceArea influenceArea;

        /**
         * Creates a new Exit
         * 
         * @param rectangular
         * 
         * @param x
         *            The horizontal coordinate of the upper left corner of the exit
         * @param y
         *            The vertical coordinate of the upper left corner of the exit
         * @param width
         *            The width of the exit
         * @param height
         *            The height of the exit
         */
        public ActiveArea(string id, bool rectangular, int x, int y, int width, int height) : base(id)
        {
            conditions = new Conditions();
            influenceArea = new InfluenceArea(x, y, width, height);
            influenceArea.setRectangular(rectangular);
        }

        /**
         * @return the conditions
         */
        public Conditions getConditions()
        {

            return conditions;
        }

        /**
         * @param conditions
         *            the conditions to set
         */
        public void setConditions(Conditions conditions)
        {

            this.conditions = conditions;
        }

        public InfluenceArea getInfluenceArea()
        {

            return influenceArea;
        }

        public void setInfluenceArea(InfluenceArea influeceArea)
        {

            this.influenceArea = influeceArea;
        }


        public override object Clone()
        {
            ActiveArea aa = (ActiveArea)base.Clone();
            //can not be two identical id
            string id = aa.getId() + "-" + (new System.Random().Next(1000).ToString());
            aa.setId(id);
            aa.conditions = (conditions != null ? (Conditions)conditions.Clone() : null);
            aa.influenceArea = (influenceArea != null ? (InfluenceArea)influenceArea.Clone() : null);
            return aa;
        }
    }
}