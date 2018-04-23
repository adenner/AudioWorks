using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace AudioWorks.Common.Tests
{
    public sealed class AudioUnsupportedExceptionTests
    {
        [Fact(DisplayName = "AudioUnsupportedException is an AudioException")]
        public void IsAudioException()
        {
            Assert.IsAssignableFrom<AudioException>(new AudioUnsupportedException());
        }

        [Fact(DisplayName = "AudioUnsupportedException's Path property is properly serialized")]
        public void PathIsSerialized()
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, new AudioUnsupportedException(null, "Foo"));
                stream.Position = 0;
                Assert.Equal("Foo", ((AudioUnsupportedException) formatter.Deserialize(stream)).Path);
            }
        }
    }
}