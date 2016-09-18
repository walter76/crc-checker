# crc-checker

The idea is to build a fast implementation for creating checksums from binary files in C#. The solution
should be able to read all files in a directory and its subdirectories, calculate a CRC32-checksum for
each file and print the result to the console.

I am aware that there are already a lot of CRC-Checkers available. My goal is not only to have a fast
implementation, but also to learn something along the way about optimization and performance analysis
with the [Windows Performance Toolkit](https://msdn.microsoft.com/en-us/windows/hardware/commercialize/test/wpt/index).
