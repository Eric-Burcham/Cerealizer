namespace Cerealizer
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public class RewindableJsonTextReader : JsonReader, IJsonLineInfo, IRewind
    {
        private readonly RewindableStringReader _stringReader;

        private JsonTextReader _jsonReader;

        public RewindableJsonTextReader(RewindableStringReader stringReader)
        {
            _stringReader = stringReader;
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

        public override bool Equals(object obj)
        {
            return _jsonReader.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _jsonReader.GetHashCode();
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

        public override Task<bool?> ReadAsBooleanAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return _jsonReader.ReadAsBooleanAsync(cancellationToken);
        }

        public override byte[] ReadAsBytes()
        {
            return _jsonReader.ReadAsBytes();
        }

        public override Task<byte[]> ReadAsBytesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return _jsonReader.ReadAsBytesAsync(cancellationToken);
        }

        public override DateTime? ReadAsDateTime()
        {
            return _jsonReader.ReadAsDateTime();
        }

        public override Task<DateTime?> ReadAsDateTimeAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return _jsonReader.ReadAsDateTimeAsync(cancellationToken);
        }

        public override DateTimeOffset? ReadAsDateTimeOffset()
        {
            return _jsonReader.ReadAsDateTimeOffset();
        }

        public override Task<DateTimeOffset?> ReadAsDateTimeOffsetAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return _jsonReader.ReadAsDateTimeOffsetAsync(cancellationToken);
        }

        public override decimal? ReadAsDecimal()
        {
            return _jsonReader.ReadAsDecimal();
        }

        public override Task<decimal?> ReadAsDecimalAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return _jsonReader.ReadAsDecimalAsync(cancellationToken);
        }

        public override double? ReadAsDouble()
        {
            return _jsonReader.ReadAsDouble();
        }

        public override Task<double?> ReadAsDoubleAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return _jsonReader.ReadAsDoubleAsync(cancellationToken);
        }

        public override int? ReadAsInt32()
        {
            return _jsonReader.ReadAsInt32();
        }

        public override Task<int?> ReadAsInt32Async(CancellationToken cancellationToken = new CancellationToken())
        {
            return _jsonReader.ReadAsInt32Async(cancellationToken);
        }

        public override string ReadAsString()
        {
            return _jsonReader.ReadAsString();
        }

        public override Task<string> ReadAsStringAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return _jsonReader.ReadAsStringAsync(cancellationToken);
        }

        public override Task<bool> ReadAsync(CancellationToken cancellationToken = new CancellationToken())
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
            _stringReader.Rewind();
            _jsonReader = new JsonTextReader(_stringReader);
        }
    }

    public class RewindableStringReader : TextReader, IRewind
    {
        private string _s;

        private StringReader _stringReader;

        public RewindableStringReader(string s)
        {
            _s = s;

            InitializeReader();
        }

        public override void Close()
        {
            _stringReader.Close();
        }

        public override int Peek()
        {
            return _stringReader.Peek();
        }

        public override int Read()
        {
            return _stringReader.Read();
        }

        public override int Read(char[] buffer, int index, int count)
        {
            return _stringReader.Read(buffer, index, count);
        }

        public override Task<int> ReadAsync(char[] buffer, int index, int count)
        {
            return _stringReader.ReadAsync(buffer, index, count);
        }

        public override Task<int> ReadBlockAsync(char[] buffer, int index, int count)
        {
            return _stringReader.ReadBlockAsync(buffer, index, count);
        }

        public override string? ReadLine()
        {
            return _stringReader.ReadLine();
        }

        public override Task<string> ReadLineAsync()
        {
            return _stringReader.ReadLineAsync();
        }

        public override string ReadToEnd()
        {
            return _stringReader.ReadToEnd()!;
        }

        public override Task<string> ReadToEndAsync()
        {
            return _stringReader.ReadToEndAsync();
        }

        public void Rewind()
        {
            InitializeReader();
        }

        protected override void Dispose(bool disposing)
        {
            _s = null;
            _stringReader.Dispose();
        }

        private void InitializeReader()
        {
            _stringReader?.Dispose();
            _stringReader = new StringReader(_s);
        }
    }

    public interface IRewind
    {
        void Rewind();
    }
}
