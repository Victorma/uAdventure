using System;
using System.Collections.Generic;
using uAdventure.Editor;

namespace uAdventure.Geo
{
	public class GeoElementRefDataControl : MapElementDataControl
    {
        private readonly GeoReference geoReference;

        public GeoElementRefDataControl(GeoReference geoReference) : base(geoReference)
        {
            this.geoReference = geoReference;
        }

        public override bool isValid(string currentPath, List<string> incidences)
        {
            return controller.IdentifierSummary.isType<GeoElement>(geoReference.getTargetId());
        }

        public override void setTargetId(string id)
        {
            if (!controller.IdentifierSummary.isType<GeoElement>(id))
            {
                return;
            }

            controller.AddTool(new ChangeTargetIdTool(geoReference, id));
        }
    }
}
