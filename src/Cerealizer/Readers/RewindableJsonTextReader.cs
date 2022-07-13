namespace Cerealizer
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public class RewindableJsonTextReader : JsonReader, IJsonLineInfo, IRewind
    {
        private readonly string _value;

        private JsonTextReader _jsonReader;

        public RewindableJsonTextReader(string value)
        {
            _value = value;
            InitializeReaders();
        }

        public override int Depth => _jsonReader.Depth;

        public int LineNumber => _jsonReader.LineNumber;

        public int LinePosition => _jsonReader.LinePosition;

        public override string Path => _jsonReader.Path;

        public override JsonToken TokenType => _jsonReader.TokenType;

        public override object Value => _jsonReader.Value;

        public override Type ValueType => _jsonReader.ValueType;

        public override void Close()
        {
            _jsonReader.Close();
        }

        public bool HasLineInfo()
        {
            return _jsonReader.HasLineInfo();
        }

        public override bool Read()
        {
            return _jsonReader.Read();
        }

        public override bool? ReadAsBoolean()
        {
            return _jsonReader.ReadAsBoolean();
        }

        public override Task<bool?> ReadAsBooleanAsync(CancellationToken cancellationToken = new ())
        {
            return _jsonReader.ReadAsBooleanAsync(cancellationToken);
        }

        public override byte[] ReadAsBytes()
        {
            return _jsonReader.ReadAsBytes();
        }

        public override Task<byte[]> ReadAsBytesAsync(CancellationToken cancellationToken = new ())
        {
            return _jsonReader.ReadAsBytesAsync(cancellationToken);
        }

        public override DateTime? ReadAsDateTime()
        {
            return _jsonReader.ReadAsDateTime();
        }

        public override Task<DateTime?> ReadAsDateTimeAsync(CancellationToken cancellationToken = new ())
        {
            return _jsonReader.ReadAsDateTimeAsync(cancellationToken);
        }

        public override DateTimeOffset? ReadAsDateTimeOffset()
        {
            return _jsonReader.ReadAsDateTimeOffset();
        }

        public override Task<DateTimeOffset?> ReadAsDateTimeOffsetAsync(CancellationToken cancellationToken = new ())
        {
            return _jsonReader.ReadAsDateTimeOffsetAsync(cancellationToken);
        }

        public override decimal? ReadAsDecimal()
        {
            return _jsonReader.ReadAsDecimal();
        }

        public override Task<decimal?> ReadAsDecimalAsync(CancellationToken cancellationToken = new ())
        {
            return _jsonReader.ReadAsDecimalAsync(cancellationToken);
        }

        public override double? ReadAsDouble()
        {
            return _jsonReader.ReadAsDouble();
        }

        public override Task<double?> ReadAsDoubleAsync(CancellationToken cancellationToken = new ())
        {
            return _jsonReader.ReadAsDoubleAsync(cancellationToken);
        }

        public override int? ReadAsInt32()
        {
            return _jsonReader.ReadAsInt32();
        }

        public override Task<int?> ReadAsInt32Async(CancellationToken cancellationToken = new ())
        {
            return _jsonReader.ReadAsInt32Async(cancellationToken);
        }

        public override string ReadAsString()
        {
            return _jsonReader.ReadAsString();
        }

        public override Task<string> ReadAsStringAsync(CancellationToken cancellationToken = new ())
        {
            return _jsonReader.ReadAsStringAsync(cancellationToken);
        }

        public override Task<bool> ReadAsync(CancellationToken cancellationToken = new ())
        {
            return _jsonReader.ReadAsync(cancellationToken);
        }

        public void Rewind()
        {
            InitializeReaders();
        }

        public override string ToString()
        {
            return _jsonReader.ToString();
        }

        private void InitializeReaders()
        {
            _jsonReader = new JsonTextReader(new StringReader(_value));
        }
    }
}
