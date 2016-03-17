using System;
using System.Linq;
using DnsZone.Records;
using DnsZone.Tokens;
using NUnit.Framework;

namespace DnsZone.Tests {
    [TestFixture]
    public class DnsZoneTests {

        [Test]
        public void ParseTest() {
            const string str = @"
$ORIGIN example.com.     ; designates the start of this zone file in the namespace
$TTL 1h                  ; default expiration time of all resource records without their own TTL value
example.com.  IN  SOA   ns.example.com. username.example.com. ( 2007120710 1d 2h 4w 1h )
example.com.  IN  NS    ns                    ; ns.example.com is a nameserver for example.com
example.com.  IN  NS    ns.somewhere.example. ; ns.somewhere.example is a backup nameserver for example.com
example.com.  IN  MX    10 mail.example.com.  ; mail.example.com is the mailserver for example.com
@             IN  MX    20 mail2.example.com. ; equivalent to above line, ""@"" represents zone origin
@             IN  MX    50 mail3              ; equivalent to above line, but using a relative host name
example.com.  IN  A     192.0.2.1             ; IPv4 address for example.com
              IN  AAAA  2001:db8:10::1        ; IPv6 address for example.com
ns            IN  A     192.0.2.2             ; IPv4 address for ns.example.com
              IN  AAAA  2001:db8:10::2        ; IPv6 address for ns.example.com
www           IN  CNAME example.com.          ; www.example.com is an alias for example.com
wwwtest       IN  CNAME www                   ; wwwtest.example.com is another alias for www.example.com
mail          IN  A     192.0.2.3             ; IPv4 address for mail.example.com
mail2         IN  A     192.0.2.4             ; IPv4 address for mail2.example.com
mail3         IN  A     192.0.2.5             ; IPv4 address for mail3.example.com";
            try {
                var zone = DnsZone.Parse(str);
                Assert.AreEqual(1, zone.Records.OfType<SoaResourceRecord>().Count());
                Assert.AreEqual(2, zone.Records.OfType<NsResourceRecord>().Count());
                Assert.AreEqual(3, zone.Records.OfType<MxResourceRecord>().Count());
                Assert.AreEqual(5, zone.Records.OfType<AResourceRecord>().Count());
                Assert.AreEqual(2, zone.Records.OfType<CNameResourceRecord>().Count());
            } catch (TokenException exc) {
                Console.WriteLine(exc.Token.Position.GetLine());
                throw;
            }
        }

        [Test]
        public void TxtRecordParse() {
            const string str = @"
; multiple quotes strings on a single line
; generates a single text string of 
; Located in a black hole somewhere
$TTL 1h                  ; default expiration time of all resource records without their own TTL value
joe        IN      TXT    ""Located in a black hole"" "" somewhere""
; multiple quoted strings on multiple lines
joe IN      TXT (""Located in a black hole""
                    "" somewhere over the rainbow"")
; generates a single text string of
; Located in a black hole somewhere over the rainbow";
            var zone = DnsZone.Parse(str);
            Assert.AreEqual(2, zone.Records.Count);

            Assert.IsAssignableFrom<TxtResourceRecord>(zone.Records.First());
        }
    }
}
