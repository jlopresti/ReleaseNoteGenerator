using System;
using System.Collections.Generic;

namespace ReleaseNoteGenerator.Console.Common
{
    public class ReleaseNoteKeyComparer : IEqualityComparer<IReleaseNoteKey>
    {
        public bool Equals(IReleaseNoteKey x, IReleaseNoteKey y)
        {
            return x != null && y != null && x.Id.Equals(y.Id, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(IReleaseNoteKey obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}