# crc-checker

The idea is to build a fast implementation for creating checksums from binary
files in C#. The solution should be able to read all files in a directory and
its subdirectories, calculate a CRC32-checksum for each file and print the
result to the console.

I am aware that there are already a lot of good CRC-Checkers available. My
primary goal is not to have a fast implementation, but to learn something about
optimization (multi-threading) on Windows with .NET and performance analysis
with the [Windows Performance Toolkit](https://msdn.microsoft.com/en-us/windows/hardware/commercialize/test/wpt/index).

This repository contains all the source code and also the article that I am
going to submit to [CODE Project](https://www.codeproject.com/) as soon as it is
finished. In the meanwhile you can find the [draft](doc/article.md) in the doc
folder.
