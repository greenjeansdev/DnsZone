# About

DnsZone is a tool for parsing and building dns zone file.
https://tools.ietf.org/html/rfc1035

# Usage

Parse dns zone from existing string:

`
var zone = DnsZone.Parse(string);
`

Load zone from external uri:

`
var zone = await DnsZone.LoadAsync(uri)
`