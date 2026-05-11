# maxcsoGUI
A GUI for maxcso.

<img width="1528" height="568" alt="Image" src="https://github.com/user-attachments/assets/08ff9621-058c-4d39-af22-be2944f274b9" />
<br>
maxcso is a Command Line tool that dynamically uses high-quality compression algorithms to shrink ISO files into a variety of different resulting formats. This program adds a GUI layer on top of it for easier use.
This branch's <a href="https://github.com/wad11656/maxcsoGUI/releases/latest">Release</a> provides a standalone executable that is pre-bundled with the original `maxcso.exe` program.

This GUI version replicates all available flags/options available in the original Command Line `maxcso.exe` program, including:

- Thread count selection (which now defaults to the maximum available, like the original Command Line app).
- All available Output formats (`cso1`, `cso2`, `zso`, `dax`)
- Fast mode, optional [Zopfli](https://github.com/google/zopfli) compression, alternate block size, decompression, and custom output directory
- Advanced flags for CRC32, measure-only runs, [libdeflate](https://github.com/ebiggers/libdeflate), [LZ4](https://github.com/lz4/lz4) brute force, and cost tuning

Thanks to unknownbrackets for maxcso https://github.com/unknownbrackets/maxcso
