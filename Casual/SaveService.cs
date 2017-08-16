using Casual.Ravenhill;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Casual {

    public class SaveService : GameBehaviour, ISaveService {

        private readonly Dictionary<string, ISaveable> m_Saveables = new Dictionary<string, ISaveable>();

        public void Setup(object data) { }

        public virtual void Register(ISaveable saveable) {
            if (!m_Saveables.ContainsKey(saveable.saveId)) {
                m_Saveables.Add(saveable.saveId, saveable);
                saveable.OnRegister();
                LoadSaveable(saveable);
            }
        }

        private void LoadSaveable(ISaveable saveable) {
            string fileName = GetSaveableFilePath(saveable);
            if (System.IO.File.Exists(fileName)) {
                using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open))) {
                    string saveString = reader.ReadString();
                    if (!string.IsNullOrEmpty(saveString)) {
                        saveable.Load(saveString);
                        saveable.OnLoaded();
                        engine?.GetService<IEventService>()?.SendEvent(new SaveableLoadedEventArgs(saveable));
                    } else {
                        saveable.InitSave();
                        saveable.OnLoaded();
                        engine?.GetService<IEventService>()?.SendEvent(new SaveableLoadedEventArgs(saveable));
                    }
                }
            } else {
                saveable.InitSave();
                saveable.OnLoaded();
                engine?.GetService<IEventService>()?.SendEvent(new SaveableLoadedEventArgs(saveable));
            }
        }

        private string GetSaveableFilePath(ISaveable saveable) {
            return GetOSSaveDirPath() + saveable.saveId + ".xml";
        }

        private string GetOSSaveDirPath() {
            if (Application.isEditor) {
                return Application.persistentDataPath + "/";
            } else if (Application.isWebPlayer) {
                return System.IO.Path.GetDirectoryName(Application.absoluteURL).Replace("\\", "/") + "/";
            } else if (Application.isMobilePlatform || Application.isConsolePlatform) {
                return Application.persistentDataPath;
            } else {
                return Application.persistentDataPath + "/";
            }
        }

        public virtual void Unregister(ISaveable saveable) {
            if (m_Saveables.ContainsKey(saveable.saveId)) {
                m_Saveables.Remove(saveable.saveId);
            }
        }

        public virtual void Save() {
            foreach (var kvp in m_Saveables) {
                var saveable = kvp.Value;
                if (saveable.isLoaded) {
                    WriteSaveable(saveable);
                }
            }
        }

        private void WriteSaveable(ISaveable saveable) {
            using (BinaryWriter writer = new BinaryWriter(System.IO.File.Open(GetSaveableFilePath(saveable), System.IO.FileMode.Create))) {
                writer.Write(saveable.GetSave());
            }
        }

        public virtual void Load() {
            foreach (var kvp in m_Saveables) {
                var saveable = kvp.Value;
                if (!saveable.isLoaded) {
                    LoadSaveable(saveable);
                }
            }
        }

    }
}
