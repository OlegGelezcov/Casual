﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Data {
    public abstract class InventoryItemData : IconData {

        public abstract InventoryItemType type { get; }
    }
}