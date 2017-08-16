namespace Casual {
    public interface ISaveElement {
        UXMLWriteElement GetSave();
        void Load(UXMLElement element);
        void InitSave();
    }
}
