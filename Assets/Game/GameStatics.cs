
using System.Collections.Generic;


namespace Game
{

    public sealed class GameStatics
    {
        static public void AddGameStatic<T>(string name, T _s) where T : class, IGameStaticModule
        {
            if (_modules.ContainsKey(name))
            {
                return;
            }

            _modules.Add(name, _s);
        }

        static public T QueryGameStatic<T>(string name) where T : class, IGameStaticModule
        {
            if (_modules.ContainsKey(name))
            {
                return (T)_modules[name];
            }

            return default(T);
        }


        static private Dictionary<string, IGameStaticModule> _modules = new Dictionary<string, IGameStaticModule>();

    }

}
