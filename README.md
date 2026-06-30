# maxcsoGUI
A Windows GUI for maxcso.

<img width="1688" height="612" alt="Image" src="https://github.com/user-attachments/assets/6d0a8ec5-2044-4b60-b1a0-1a93269a462c" />
<br><br>
maxcso is a Command Line tool that dynamically uses high-quality compression algorithms to shrink (primarily) PSP & PS2 ISO files into a variety of different resulting formats. This program adds a GUI layer on top of it for easier use.
<br><br>
The GUI version replicates all available flags/options in the original Command Line <code>maxcso.exe</code> program, including:

- Thread count selection (which now defaults to the maximum available, like the original Command Line app).
- All available Output formats (`cso1`, `cso2`, `zso`, `dax`).
- Enable/disable all available algorithms in the Compression Algorithms Trial Pool.
- Options for fast mode, alternate block size, decompression, CRC32, measure-only runs, cost tuning, and custom output directory.

Thanks to unknownbrackets for maxcso: https://github.com/unknownbrackets/maxcso
