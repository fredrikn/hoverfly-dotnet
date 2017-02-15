# hoverfly-dotnet
A .Net Library for Hoverfly

http://hoverfly.io/

# What is Hoverfly DotNet?
Hoverfly is a lightweight service virtualisation tool which allows you to stub / simulate HTTP(S) services. It is a proxy written in Go which responds to HTTP(S) requests with stored responses, pretending to be it’s real counterpart.

It enables you to get around common testing problems caused by external dependencies, such as non-deterministic data, flakiness, not yet implemented API’s, licensing fees, slow tests and more.

Hoverfly .Net is a native language binding which gives you an expressive API for managing Hoverfly in .Net. It gives you a Hoverfly class which abstracts away the binary and API calls.

This project doesn't include the hoverfly.exe
You can download it from http://hoverfly.io/#download
