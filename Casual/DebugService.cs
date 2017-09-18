namespace Casual
{

    public interface IDebugService : IService {
        void Add(string name, BaseCommand command);
        bool Execute(string command);
        void AddMessage(string message, ColorType color);
        void AddMessage(string message, string color = "white");
    }

    /// <summary>
    /// Abstract class for any command
    /// </summary>
    public abstract class BaseCommand : Ravenhill.RavenhillGameElement {

        public BaseCommand(string name) {
            this.name = name.ToLower();
        }

        public string name {
            get;
        }

        protected string GetToken(string source, int index) {
            string[] tokens = source.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

            if (index < tokens.Length) {
                return tokens[index];
            }

            return string.Empty;
        }

        protected int GetInt(string source, int index) {
            return Utility.GetIntOrDefault(GetToken(source, index), 0);
        }

        protected float GetFloat(string source, int index) {
            return Utility.GetFloatOrDefault(GetToken(source, index), 0.0f);
        }

        protected bool GetBool(string source, int index) {
            return Utility.GetBoolOrDefault(GetToken(source, index), false);
        }

        public virtual bool Execute(string source) {
            return false;
        }

    }
}
