using System;
using System.Collections.Generic;

namespace Export.Interfaces
{
    public interface ISequence : IMethodAdding
    {
        void GetCallSequenceMethods(IEnumerable<Action> actions);
    }
}
