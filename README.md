# maxcsoGUI
A simple GUI for maxcso.

maxcso uses great compression for ISO files, but is commandline only. This program fixes that.
The GUI looks for `maxcso.exe` or `maxcso32.exe` next to the app first, and also checks for a sibling `maxcso` repo when you're running it from source.

Current GUI options include:

- Thread selection with readable `Thread` / `Threads` labels
- Output format selection for `cso1`, `cso2`, `zso`, and `dax`
- Fast mode, Zopfli, alternate block size, decompression, and custom output directory
- Advanced flags for CRC32, measure-only runs, libdeflate, LZ4 brute force, and cost tuning

Thanks to unknownbrackets for maxcso https://github.com/unknownbrackets/maxcso
