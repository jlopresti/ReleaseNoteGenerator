using System;
using System.Collections.Generic;
using Ranger.Core.Models;

namespace Ranger.Core.Helpers
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