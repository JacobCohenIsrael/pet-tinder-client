using System;

namespace Gamefather.Pet
{
    [Serializable]
    public class PetVoteRequest
    {
        public long petId;
        public bool like;
    }
}