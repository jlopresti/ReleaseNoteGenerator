using System;
using System.Collections.Generic;
using Ranger.NetCore.Models;

namespace Ranger.NetCore.Helpers
{
    public class ReleaseNoteKeyComparer : IEqualityComparer<IReleaseNoteKey>
    {
        public bool Equals(IReleaseNoteKey x, IReleaseNoteKey y)
        {
            return x != null && y != null && x.Id.Equals(y.Id, StringComparison.CurrentCultureIgnoreCase);
        }

        public int GetHashCode(IReleaseNoteKey obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}