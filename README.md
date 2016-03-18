# About

DnsZone is a tool for parsing and building dns zone file.
https://tools.ietf.org/html/rfc1034
https://tools.ietf.org/html/rfc1035
https://tools.ietf.org/html/rfc2782

# Usage

Parse dns zone from existing string:

`
var zone = DnsZoneFile.Parse(string);
`

Load zone from external uri:

`
var zone = await DnsZoneFile.LoadAsync(uri)
`