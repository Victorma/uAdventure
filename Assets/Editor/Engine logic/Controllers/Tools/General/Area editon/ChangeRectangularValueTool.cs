using UnityEngine;
using System.Collections;

using uAdventure.Core;

namespace uAdventure.Editor
{
    public class ChangeRectangularValueTool : Tool
    {

        private HasInfluenceArea holder;

        private bool rectangular;

        public ChangeRectangularValueTool(HasInfluenceArea holder, bool rectangular)
        {

            this.holder = holder;
            this.rectangular = rectangular;
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

            if (holder.getInfluenceArea().isRectangular() != rectangular)
            {
                holder.getInfluenceArea().setRectangular(rectangular);
                return true;
            }
            return false;
        }


        public override bool redoTool()
        {

            holder.getInfluenceArea().setRectangular(rectangular);
            Controller.Instance.updatePanel();
            return true;
        }


        public override bool undoTool()
        {

            holder.getInfluenceArea().setRectangular(!rectangular);
            Controller.Instance.updatePanel();
            return true;
        }
    }
}