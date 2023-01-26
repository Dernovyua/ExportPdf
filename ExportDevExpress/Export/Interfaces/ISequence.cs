using System;
using System.Collections.Generic;

namespace Export.Interfaces
{
    public interface ISequence : IExport
    {
        void GetCallSequenceMethods(IEnumerable<Action> actions);
    }
}
