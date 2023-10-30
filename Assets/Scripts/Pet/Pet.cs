using System;

namespace Gamefather.Pet
{
    [Serializable]
    public class Pet
    {
        public long id;
        public string type;
        public string name;
        public string gender;
        public string[] photos;
    }
}