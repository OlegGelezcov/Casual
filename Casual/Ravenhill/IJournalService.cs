using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill {
    public interface IJournalService : IService {
        void AddEntry(JournalEntryInfo entry);
        bool RemoveEntry(string id);
        JournalEntryInfo GetEntry(string id);
        bool Contains(string id);
        void OpenEndText(string id);
        void CloseEndText(string id);
        bool IsLast(JournalEntryInfo info);
        bool IsFirst(JournalEntryInfo info);
        JournalEntryInfo GetNext(JournalEntryInfo info);
        JournalEntryInfo GetPrev(JournalEntryInfo info);
        JournalEntryInfo GetAt(int index);
        JournalEntryInfo GetEntry(QuestData questData);
        int GetEntryIndex(JournalEntryInfo info);
    }
}
