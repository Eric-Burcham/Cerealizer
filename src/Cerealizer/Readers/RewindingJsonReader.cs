namespace Cerealizer
{
    using Newtonsoft.Json;

    public abstract class RewindingJsonReader : JsonReader, IRewind
    {
        public abstract void Rewind();
    }
}
