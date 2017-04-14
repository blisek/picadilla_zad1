namespace Assets.Scripts.Utilities
{

    /// <summary>
    /// Class used by, singleton pattern implementing, classes as base class.
    /// </summary>
    /// <typeparam name="T">Type of managed class. Only requirement is having by this type public, parameterless ctor.</typeparam>
    public abstract class Singleton<T> where T : new()
    {
        private static readonly object _monitor = new object();
        private static T _instance;

        /// <summary>
        /// Returns singleton of type T and creates new instance if this property is used first time.
        /// Calling this property is thread safe.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_monitor)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                        }
                    }
                }

                return _instance;
            }
        }
    }
}
