using DnsZone.Records;
using NUnit.Framework;

namespace DnsZone.Tests.Records {
    [TestFixture]
    public class SrvResourceRecordTests {

        [Test]
        public void NameTest() {
            var record = new SrvResourceRecord();
            record.Name = "_foobar._tcp.example.com";
            Assert.AreEqual("foobar", record.Service);
            Assert.AreEqual("tcp", record.Protocol);
            Assert.AreEqual("example.com", record.Host);

            record.Service = "test";
            Assert.AreEqual("_test._tcp.example.com", record.Name);

            record.Protocol = "udp";
            Assert.AreEqual("_test._udp.example.com", record.Name);

            record.Host = "vcap.me";
            Assert.AreEqual("_test._udp.vcap.me", record.Name);
        }
    }
}
