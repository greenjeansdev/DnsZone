namespace DnsZone.Tokens {
    public struct Token {

        public TokenPosition Position { get; set; }

        public TokenType Type { get; set; }

        public string StringValue { get; set; }

        public override string ToString() {
            switch (Type) {
                case TokenType.At:
                    return "@";
                case TokenType.Control:
                    return $"${StringValue}";
                case TokenType.Literal:
                    return StringValue;
                case TokenType.NewLine:
                    return "\\r\\n";
            }
            return StringValue;
        }

    }
}
