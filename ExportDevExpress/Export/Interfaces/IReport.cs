﻿using System;
using System.Collections.Generic;

namespace Export.Interfaces
{
    public interface IReport : IExport
    {
        void GetCallSequenceMethods(List<Action> actions);
    }
}
