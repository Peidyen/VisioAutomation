﻿using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioAutomation.Scripting.Models
{
    public enum ConnectionPointType
    {
        Inward = IVisio.VisCellVals.visCnnctTypeInward,
        Outward = IVisio.VisCellVals.visCnnctTypeOutward,
        InwardOutward = IVisio.VisCellVals.visCnnctTypeInwardOutward
    }
}