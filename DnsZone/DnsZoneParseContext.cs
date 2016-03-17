using System;
using System.Collections.Generic;
using DnsZone.Tokens;

namespace DnsZone {
    public class DnsZoneParseContext {

        public string Origin { get; set; }

        public TimeSpan? DefaultTtl { get; set; }

        public DnsZone Zone { get; }

        public Queue<Token> Tokens { get; }

        public bool IsEof => Tokens.Count == 0;

        public DnsZoneParseContext(DnsZone zone, IEnumerable<Token> tokens) {
            Zone = zone;
            Tokens = new Queue<Token>(tokens);
        }

    }
}
