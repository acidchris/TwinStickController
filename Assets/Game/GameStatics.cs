
using System.Collections.Generic;


namespace Game
{

    public sealed class GameStatics
    {
        /// <summary>
        /// Register a game module (use in AWAKE)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="_s"></param>
        static public void AddGameModule<T>(string name, T _s) where T : class, IGameModule
        {
            if (_modules.ContainsKey(name))
            {
                return;
            }

            _modules.Add(name, _s);
        }
        /// <summary>
        /// Query a specific game module (cache at least in START)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        static public T QueryGameModule<T>(string name) where T : class, IGameModule
        {
            if (_modules.ContainsKey(name))
            {
                return (T)_modules[name];
            }

            return default(T);
        }

        /// <summary>
        /// Removes a module from the dictionary (best in OnDestroy)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        static public void RemoveGameModule<T>(string name)
        {
            if (_modules.ContainsKey(name))
            {
                _modules.Remove(name);
            }
        }

        static private Dictionary<string, IGameModule> _modules = new Dictionary<string, IGameModule>();

    }

}
