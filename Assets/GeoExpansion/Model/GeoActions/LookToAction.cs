﻿using UnityEngine;
using System.Collections;
using System;

namespace uAdventure.Geo
{
    public class LookToAction : GeoAction
    {
        private readonly string[] parameters = {"Inside", "Center", "Direction"};

        public LookToAction() : base()
        {
            Inside = true;
            Center = true;
            Direction = Vector2.zero;
        }


        public override string Name
        {
            get
            {
                return "LookTo";
            }
        }

        public bool Inside { get; set; }
        public bool Center { get; set; }
        public Vector2 Direction { get; set; }

        public override string[] Parameters
        {
            get { return parameters; }
        }

        public override object Clone()
        {
            return base.Clone();
        }
    }
}