using Casual.Ravenhill.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill {
    public class JournalService : RavenhillGameBehaviour, IJournalService, ISaveable {

        private List<JournalEntryInfo> entries { get; } = new List<JournalEntryInfo>();

        public void Setup(object data) {

        }

        public override void Start() {
            base.Start();
            engine.GetService<ISaveService>().Register(this);
        }

        #region ISaveable
        public string saveId => "journal";

        public bool isLoaded { get; private set; }

        public string GetSave() {
            UXMLWriteElement root = new UXMLWriteElement(saveId);
            foreach(var entry in entries ) {
                root.Add(entry.GetSave());
            }
            return root.ToString();
        }

        public void InitSave() {
            entries.Clear();
            isLoaded = true;
        }

        public bool Load(string saveStr) {
            if(saveStr.IsValid() ) {
                UXMLDocument document = UXMLDocument.FromXml(saveStr);
                entries.Clear();
                document.Element(saveId).Elements("entry").ForEach(entryElement => {
                    JournalEntryInfo info = new JournalEntryInfo();
                    info.Load(entryElement);
                    if(info.IsValid) {
                        entries.Add(info);
                    }
                });
                isLoaded = true;
            } else {
                InitSave();
            }
            return isLoaded;
        }

        public void OnLoaded() {
            Debug.Log($"{saveId} is loaded".Colored(ColorType.aqua));
        }

        public void OnRegister() {
            Debug.Log($"{saveId} is registered for save".Colored(ColorType.aqua));
        }
        #endregion


        #region IJournalService
        public void AddEntry(JournalEntryInfo entry) {
            if(!Contains(entry.Id)) {
                entries.Add(entry);
                RavenhillEvents.OnJournalEntryAdded(entry);
            }
        }

        public JournalEntryInfo GetEntry(string id) {
            return entries.FirstOrDefault(entry => entry.Id == id);
        }

        public bool RemoveEntry(string id) {
            if (Contains(id)) {
                int index = entries.FindIndex(entry => entry.Id == id);
                if (index >= 0) {
                    var entry = entries[index];
                    entries.RemoveAt(index);
                    RavenhillEvents.OnJournalEntryRemoved(entry);
                    return true;
                }
            }
            return false;
        }

        public bool Contains(string id ) {
            return (entries.Find(entry => entry.Id == id) != null);
        }

        public void OpenEndText(string id ) {
            var entry = GetEntry(id);
            if(entry != null ) {
                if( entry.OpenEndText() ) {
                    RavenhillEvents.OnJournalEntryEndTextOpened(entry);
                }
            }
        }

        public void CloseEndText(string id) {
            var entry = GetEntry(id);
            if(entry != null ) {
                if( entry.CloseEndText()) {
                    RavenhillEvents.OnJournalEntryEndTextClosed(entry);
                }
            }
        }

        public bool IsLast(JournalEntryInfo info) {
            if(entries.Count > 0 ) {
                return entries[entries.Count - 1].Id == info.Id;
            }
            return false;
        }

        public bool IsFirst(JournalEntryInfo info) {
            if(entries.Count > 0 ) {
                return entries[0].Id == info.Id;
            }
            return false;
        }

        public JournalEntryInfo GetNext(JournalEntryInfo info) {
            if(IsLast(info)) {
                return null;
            } else {
                int index = entries.IndexOf(info);
                if(index >= 0 && (index + 1) < entries.Count) {
                    return entries[index + 1];
                }
                return null;
            }
        }

        public JournalEntryInfo GetPrev(JournalEntryInfo info ) {
            if(IsFirst(info)) {
                return null;
            } else {
                int index = entries.IndexOf(info);
                if(index > 0 ) {
                    return entries[index - 1];
                }
                return null;
            }
        }

        public JournalEntryInfo GetAt(int index) {
            if(index < entries.Count) {
                return entries[index];
            }
            return null;
        }

        public JournalEntryInfo GetEntry(QuestData questData ) {
            return entries.FirstOrDefault(entry => entry.Id == questData.journalId);
        }

        public int GetEntryIndex(JournalEntryInfo info) {
            for(int i = 0; i < entries.Count; i++ ) {
                if(entries[i].Id == info.Id ) {
                    return i;
                }
            }
            return -1;
        }
        #endregion


    }
}
