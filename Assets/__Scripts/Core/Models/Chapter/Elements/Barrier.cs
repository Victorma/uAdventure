using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace uAdventure.Core
{
    /**
     * This class holds the data of an exit in eAdventure
     */
    public class Barrier : Element, HasInfluenceArea, ICloneable
    {
        private InfluenceArea influenceArea;


        /**
         * Conditions of the active area
         */
        private Conditions conditions;

        /**
         * Creates a new Exit
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
        public Barrier(string id, bool rectangular, int x, int y, int width, int height) : base(id)
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

        public override object Clone()
        {
            Barrier b = (Barrier)base.Clone();
            b.conditions = (conditions != null ? (Conditions)conditions.Clone() : null);
            b.influenceArea = (influenceArea != null ? (InfluenceArea)influenceArea.Clone() : null);
            return b;
        }

        public InfluenceArea getInfluenceArea()
        {
            return influenceArea;
        }

        public void setInfluenceArea(InfluenceArea influenceArea)
        {
            this.influenceArea = influenceArea;
        }
    }
}