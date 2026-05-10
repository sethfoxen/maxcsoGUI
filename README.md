# maxcsoGUI
A simple GUI for maxcso.

maxcso uses great compression for ISO files, but is commandline only. This program fixes that.
Release builds now use a bundled native `maxcsoBridge.dll` built from the original `maxcso` library, so the GUI no longer needs `maxcso.exe` beside it. The old exe lookup is still kept as a fallback for source/dev scenarios.

Current GUI options include:

- Thread selection with readable `Thread` / `Threads` labels
- Output format selection for `cso1`, `cso2`, `zso`, and `dax`
- Fast mode, Zopfli, alternate block size, decompression, and custom output directory
- Advanced flags for CRC32, measure-only runs, libdeflate, LZ4 brute force, and cost tuning

Thanks to unknownbrackets for maxcso https://github.com/unknownbrackets/maxcso
